/*
    OsReleaseNet
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Linq;
using System.Threading.Tasks;

using AlastairLundy.DotExtensions.Strings;

using AlastairLundy.OsReleaseNet.Abstractions;

using AlastairLundy.OsReleaseNet.Internal.Localizations;

#if NETSTANDARD2_0
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
#else
using System.IO;
#endif

namespace AlastairLundy.OsReleaseNet;

public class FreeBsdReleaseProvider : IFreeBsdReleaseProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    public async Task<string?> GetReleaseInfoPropertyValueAsync(string propertyName)
    {
        if (OperatingSystem.IsFreeBSD() == false)
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
        }
            
#if NET6_0_OR_GREATER
        string[] resultArray = await File.ReadAllLinesAsync("/etc/os-release");
#else
        string[] resultArray = await FilePolyfill.ReadAllLinesAsync("/etc/os-release");
#endif

        resultArray = RemoveUnwantedCharacters(resultArray);
        
        string? result = resultArray.FirstOrDefault(x => x.ToUpper().Contains(propertyName.ToUpper()));

        if (result is not null)
        {
            result = result.Replace(propertyName, string.Empty)
                .Replace("=", string.Empty);
        }

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    public async Task<FreeBsdReleaseInfo> GetReleaseInfoAsync()
    {
        if (OperatingSystem.IsFreeBSD() == false)
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
        }

#if NET6_0_OR_GREATER
        string[] resultArray = await File.ReadAllLinesAsync("/etc/os-release");
#else
        string[] resultArray = await FilePolyfill.ReadAllLinesAsync("/etc/os-release");
#endif

        resultArray = RemoveUnwantedCharacters(resultArray);

        return await Task.FromResult(ParseOsReleaseInfo(resultArray));
    }

    private string[] RemoveUnwantedCharacters(string[] resultArray)
    {
        return resultArray
            .Where(x => string.IsNullOrWhiteSpace(x) == false)
            .Select(x => x.RemoveEscapeCharacters()
                .Replace('"'.ToString(), string.Empty)
            )
            .ToArray();
    }

    
    /// <summary>
    /// Parses an array of strings to extract the FreeBSD release information.
    /// </summary>
    /// <param name="resultArray">The input array containing strings that need to be parsed for FreeBSD release information.</param>
    /// <returns>The extracted FreeBSD release information.</returns>
    private FreeBsdReleaseInfo ParseOsReleaseInfo(string[] resultArray)
    {
        FreeBsdReleaseInfo freeBsdReleaseInfo = new FreeBsdReleaseInfo();
            
        foreach (string resultLine in resultArray)
        {
            string line = resultLine.ToUpper();

            if (line.Contains("NAME=") && !line.Contains("VERSION"))
            {

                if (line.StartsWith("PRETTY_"))
                {
                    freeBsdReleaseInfo.PrettyName =
                        resultLine.Replace("PRETTY_NAME=", string.Empty);
                }

                if (!line.Contains("PRETTY") && !line.Contains("CODE"))
                {
                    freeBsdReleaseInfo.Name = resultLine
                        .Replace("NAME=", string.Empty);
                }
            }

            if (line.Contains("VERSION="))
            {
                if (line.Contains("LTS"))
                {
                    freeBsdReleaseInfo.IsLongTermSupportRelease = true;
                }
                else
                {
                    freeBsdReleaseInfo.IsLongTermSupportRelease = false;
                }

                if (line.Contains("ID="))
                {
                    freeBsdReleaseInfo.VersionId =
                        resultLine.Replace("VERSION_ID=", string.Empty);
                }
                else if (!line.Contains("ID=") && line.Contains("CODE"))
                {
                    freeBsdReleaseInfo.VersionCodename =
                        resultLine.Replace("VERSION_CODENAME=", string.Empty);
                }
                else if (!line.Contains("ID=") && !line.Contains("CODE"))
                {
                    freeBsdReleaseInfo.Version = resultLine.Replace("VERSION=", string.Empty)
                        .Replace("LTS", string.Empty);
                }
            }

            if (line.Contains("ID"))
            {
                if (!line.Contains("VERSION"))
                {
                    freeBsdReleaseInfo.Identifier = resultLine.Replace("ID=", string.Empty);
                }
            }

            if (line.Contains("URL="))
            {
                if (line.StartsWith("HOME_"))
                {
                    freeBsdReleaseInfo.HomeUrl = resultLine.Replace("HOME_URL=", string.Empty);
                }
                else if (line.StartsWith("SUPPORT_"))
                {
                    freeBsdReleaseInfo.SupportUrl =
                        resultLine.Replace("SUPPORT_URL=", string.Empty);
                }
                else if (line.StartsWith("BUG_"))
                {
                    freeBsdReleaseInfo.BugReportUrl =
                        resultLine.Replace("BUG_REPORT_URL=", string.Empty);
                }
                else if (line.StartsWith("PRIVACY_"))
                {
                    freeBsdReleaseInfo.PrivacyPolicyUrl =
                        resultLine.Replace("PRIVACY_POLICY_URL=", string.Empty);
                }
            }
        }

        return freeBsdReleaseInfo;
    }
}