/*
    OsReleaseNet
    Copyright 2020-2025 Alastair Lundy

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
 */

using System.Linq;

namespace OsReleaseNet.Parsers;

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
        ArgumentNullException.ThrowIfNull(fileContents);

        if (!OperatingSystem.IsFreeBSD())
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);

        if (!fileContents.Any(x => x.ToLower().Contains("id=")))
            throw new ArgumentException(Resources.Exceptions_Arguments_NotOsReleaseContents);

        FreeBsdOsReleaseInfo freeBsdReleaseInfo = new();

        fileContents = ParserHelper.RemoveUnwantedCharacters(fileContents).ToArray();

        foreach (string line in fileContents)
        {
            string lineUpper = line.ToUpper();

            if (lineUpper.Contains("ANSI_"))
            {
                if (lineUpper.StartsWith("ANSI_COLOR="))
                {
                    freeBsdReleaseInfo.AnsiColor = line.Replace("ANSI_COLOR=", string.Empty);
                }
            }

            if (lineUpper.Contains("NAME=") && !lineUpper.Contains("VERSION"))
            {
                if (lineUpper.StartsWith("CPE_"))
                {
                    freeBsdReleaseInfo.CpeName = line.Replace("CPE_NAME=", string.Empty);
                }

                if (lineUpper.StartsWith("PRETTY_"))
                {
                    freeBsdReleaseInfo.PrettyName =
                        line.Replace("PRETTY_NAME=", string.Empty);
                }

                if (!lineUpper.Contains("PRETTY") && !lineUpper.Contains("CODE"))
                {
                    freeBsdReleaseInfo.Name = line
                        .Replace("NAME=", string.Empty);
                }
            }

            if (lineUpper.Contains("VERSION="))
            {
                if (lineUpper.Contains("ID="))
                {
                    freeBsdReleaseInfo.VersionId =
                        line.Replace("VERSION_ID=", string.Empty);
                }
                else if (!lineUpper.Contains("ID=") && !lineUpper.Contains("CODE"))
                {
                    freeBsdReleaseInfo.Version = line.Replace("VERSION=", string.Empty)
                        .Replace("LTS", string.Empty);
                }
            }

            if (lineUpper.Contains("ID"))
            {
                if (!lineUpper.Contains("VERSION"))
                {
                    freeBsdReleaseInfo.Identifier = line.Replace("ID=", string.Empty);
                }
            }

            if (lineUpper.Contains("URL="))
            {
                if (lineUpper.StartsWith("HOME_"))
                {
                    freeBsdReleaseInfo.HomeUrl = line.Replace("HOME_URL=", string.Empty);
                }
                else if (lineUpper.StartsWith("BUG_"))
                {
                    freeBsdReleaseInfo.BugReportUrl =
                        line.Replace("BUG_REPORT_URL=", string.Empty);
                }
            }
        }

        return freeBsdReleaseInfo;
    }
}