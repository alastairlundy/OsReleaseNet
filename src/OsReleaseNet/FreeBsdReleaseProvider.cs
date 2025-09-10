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

using AlastairLundy.OsReleaseNet.Abstractions;

using AlastairLundy.OsReleaseNet.Internal.Localizations;

using System.IO;

namespace AlastairLundy.OsReleaseNet;

/// <summary>
/// 
/// </summary>
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

    /// <summary>
    /// Removes unwanted characters from an array of strings.
    /// </summary>
    /// <param name="resultArray">The input array containing strings that need to be processed.</param>
    /// <returns>The processed array of strings with unwanted characters removed.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the resultArray is null or empty.</exception>
    private string[] RemoveUnwantedCharacters(string[] resultArray)
    {
        return resultArray.Where(x => string.IsNullOrWhiteSpace(x) == false)
            .Select(x => x.Replace('"'.ToString(), string.Empty))
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

            if(line.Contains("ANSI_"))
            {
                if (line.StartsWith("ANSI_COLOR="))
                {
                    freeBsdReleaseInfo.AnsiColor = resultLine.Replace("ANSI_COLOR=", string.Empty);
                }   
            }
            if (line.Contains("NAME=") && !line.Contains("VERSION"))
            {

                if (line.StartsWith("CPE_"))
                {
                    freeBsdReleaseInfo.CpeName = resultLine.Replace("CPE_NAME=", string.Empty);
                }
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

                if (line.Contains("ID="))
                {
                    freeBsdReleaseInfo.VersionId =
                        resultLine.Replace("VERSION_ID=", string.Empty);
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
                else if (line.StartsWith("BUG_"))
                {
                    freeBsdReleaseInfo.BugReportUrl =
                        resultLine.Replace("BUG_REPORT_URL=", string.Empty);
                }
            }
        }

        return freeBsdReleaseInfo;
    }
}