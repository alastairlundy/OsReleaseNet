/*
    OsReleaseNet
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Linq;
using System.Runtime.Versioning;

using AlastairLundy.OsReleaseNet.Abstractions.Parsers;
using AlastairLundy.OsReleaseNet.Helpers;
using AlastairLundy.OsReleaseNet.Internal.Localizations;

#if NETSTANDARD2_0
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
#endif

namespace AlastairLundy.OsReleaseNet.Parsers;

/// <summary>
/// A class to parse the contents of a FreeBSD OsRelease file.
/// </summary>
public class FreeBsdOsReleaseParser : IFreeBsdOsReleaseParser
{
    
    /// <summary>
    /// Parses the contents of a FreeBSD OsRelease file.
    /// </summary>
    /// <param name="fileContents">The FreeBSD OsRelease file contents.</param>
    /// <returns>The parsed FreeBSD OsRelease file contents as a FreeBsdReleaseInfo object.</returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if run on an operating system that is not FreeBSD based.</exception>
    /// <exception cref="ArgumentException">Thrown if the string array provided hasn't come from an os-release file.</exception>
    [SupportedOSPlatform("freebsd")]
    public FreeBsdOsReleaseInfo ParseFreeBsdRelease(string[] fileContents)
    {
        if (OperatingSystem.IsFreeBSD() == false)
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);

        if (fileContents.Any(x => x.ToLower().Contains("id=")) == false)
            throw new ArgumentException(Resources.Exceptions_Arguments_NotOsReleaseContents);
        
        FreeBsdOsReleaseInfo freeBsdReleaseInfo = new FreeBsdOsReleaseInfo();
            
        fileContents = ParserHelper.RemoveUnwantedCharacters(fileContents).ToArray();
        
        foreach (string resultLine in fileContents)
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