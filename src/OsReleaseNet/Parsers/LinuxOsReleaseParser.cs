/*
    OsReleaseNet
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Linq;

using AlastairLundy.DotExtensions.Strings;

using AlastairLundy.OsReleaseNet.Abstractions.Parsers;
using AlastairLundy.OsReleaseNet.Helpers;
using AlastairLundy.OsReleaseNet.Internal.Localizations;

namespace AlastairLundy.OsReleaseNet.Parsers;

/// <summary>
/// 
/// </summary>
public class LinuxOsReleaseParser : ILinuxOsReleaseParser
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileContents"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public LinuxOsReleaseInfo ParseLinuxOsRelease(string[] fileContents)
    {
        if (OperatingSystem.IsLinux() == false)
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);

        if (fileContents.Any(x => x.ToLower().Contains("id=")) == false)
            throw new ArgumentException(Resources.Exceptions_Arguments_NotOsReleaseContents);
        
        LinuxOsReleaseInfo linuxDistroInfo = new LinuxOsReleaseInfo();
        
        fileContents = ParserHelper.RemoveUnwantedCharacters(fileContents).ToArray();
        
        foreach (string line in fileContents)
        {
            string lineUpper = line.ToUpper();

            if (lineUpper.Contains("NAME=") && !lineUpper.Contains("VERSION"))
            {
                if (lineUpper.StartsWith("PRETTY_"))
                {
                    linuxDistroInfo.PrettyName =
                        line.Replace("PRETTY_NAME=", string.Empty);
                }

                if (!lineUpper.Contains("PRETTY") && !lineUpper.Contains("CODE"))
                {
                    linuxDistroInfo.Name = line.Replace("NAME=", string.Empty);
                }
            }

            if (lineUpper.Contains("VERSION="))
            {

                if (lineUpper.Contains("ID="))
                {
                    linuxDistroInfo.VersionId =
                        line.Replace("VERSION_ID=", string.Empty);
                }
                else if (!lineUpper.Contains("ID=") && lineUpper.Contains("CODE"))
                {
                    linuxDistroInfo.VersionCodename =
                        line.Replace("VERSION_CODENAME=", string.Empty);
                }
                else if (!lineUpper.Contains("ID=") && !lineUpper.Contains("CODE"))
                {
                    linuxDistroInfo.Version = line.Replace("VERSION=", string.Empty);
                }
            }

            if (lineUpper.Contains("ID"))
            {
                if (lineUpper.Contains("ID_LIKE="))
                {
                    string identifiers = line.Replace("ID_LIKE=", string.Empty);

                    if (identifiers.ContainsSpaceSeparatedSubStrings())
                    {
                        linuxDistroInfo.IdentifierLike = identifiers.Split(" ");
                    }
                    else
                    {
                        linuxDistroInfo.IdentifierLike = [line];
                    }
                }
                else if (!lineUpper.Contains("VERSION"))
                {
                    linuxDistroInfo.Identifier = line.Replace("ID=", string.Empty);
                }
            }

            if (lineUpper.Contains("URL="))
            {
                if (lineUpper.StartsWith("HOME_"))
                {
                    linuxDistroInfo.HomeUrl = line.Replace("HOME_URL=", string.Empty);
                }
                else if (lineUpper.StartsWith("SUPPORT_"))
                {
                    linuxDistroInfo.SupportUrl =
                        line.Replace("SUPPORT_URL=", string.Empty);
                }
                else if (lineUpper.StartsWith("BUG_"))
                {
                    linuxDistroInfo.BugReportUrl =
                        line.Replace("BUG_REPORT_URL=", string.Empty);
                }
                else if (lineUpper.StartsWith("PRIVACY_"))
                {
                    linuxDistroInfo.PrivacyPolicyUrl =
                        line.Replace("PRIVACY_POLICY_URL=", string.Empty);
                }
            }
        }

        return linuxDistroInfo;
    }
}