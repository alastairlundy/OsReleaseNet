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

using System.Globalization;
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

        if (!fileContents.Any(x => x.ToLower(CultureInfo.CurrentCulture).Contains("id=", StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException(Resources.Exceptions_Arguments_NotOsReleaseContents, nameof(fileContents));

        FreeBsdOsReleaseInfo freeBsdReleaseInfo = new();

        fileContents = [.. ParserHelper.RemoveUnwantedCharacters(fileContents)];

        foreach (string line in fileContents)
        {
            string lineUpper = line.ToUpper(CultureInfo.CurrentCulture);

            if (lineUpper.Contains("ansi_", StringComparison.OrdinalIgnoreCase))
            {
                if (lineUpper.StartsWith("ansi_color=", StringComparison.OrdinalIgnoreCase))
                {
                    freeBsdReleaseInfo.AnsiColor = line.Replace("ANSI_COLOR=", string.Empty);
                }
            }

            if (lineUpper.Contains("name=", StringComparison.OrdinalIgnoreCase) &&
                !lineUpper.Contains("version", StringComparison.OrdinalIgnoreCase))
            {
                if (lineUpper.StartsWith("cpe_", StringComparison.OrdinalIgnoreCase))
                {
                    freeBsdReleaseInfo.CpeName = line.Replace("CPE_NAME=", string.Empty);
                }

                if (lineUpper.StartsWith("pretty_", StringComparison.OrdinalIgnoreCase))
                {
                    freeBsdReleaseInfo.PrettyName =
                        line.Replace("PRETTY_NAME=", string.Empty);
                }

                if (!lineUpper.Contains("pretty", StringComparison.OrdinalIgnoreCase) &&
                    !lineUpper.Contains("code", StringComparison.OrdinalIgnoreCase))
                {
                    freeBsdReleaseInfo.Name = line
                        .Replace("NAME=", string.Empty);
                }
            }

            if (lineUpper.Contains("version=", StringComparison.OrdinalIgnoreCase))
            {
                if (lineUpper.Contains("id=", StringComparison.OrdinalIgnoreCase))
                {
                    freeBsdReleaseInfo.VersionId =
                        line.Replace("VERSION_ID=", string.Empty);
                }
                else if (!lineUpper.Contains("id=", StringComparison.OrdinalIgnoreCase) &&
                         !lineUpper.Contains("code", StringComparison.OrdinalIgnoreCase))
                {
                    freeBsdReleaseInfo.Version = line.Replace("VERSION=", string.Empty)
                        .Replace("LTS", string.Empty);
                }
            }

            if (lineUpper.Contains("id", StringComparison.OrdinalIgnoreCase))
            {
                if (!lineUpper.Contains("version", StringComparison.OrdinalIgnoreCase))
                {
                    freeBsdReleaseInfo.Identifier = line.Replace("ID=", string.Empty);
                }
            }

            if (lineUpper.Contains("url=", StringComparison.OrdinalIgnoreCase))
            {
                if (lineUpper.StartsWith("home_", StringComparison.OrdinalIgnoreCase))
                {
                    freeBsdReleaseInfo.HomeUrl = line.Replace("HOME_URL=", string.Empty);
                }
                else if (lineUpper.StartsWith("bug_", StringComparison.OrdinalIgnoreCase))
                {
                    freeBsdReleaseInfo.BugReportUrl =
                        line.Replace("BUG_REPORT_URL=", string.Empty);
                }
            }
        }

        return freeBsdReleaseInfo;
    }
}