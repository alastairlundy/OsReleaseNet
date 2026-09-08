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
/// A class to parse the contents of a Linux OsRelease file.
/// </summary>
public class LinuxOsReleaseParser : ILinuxOsReleaseParser
{
    /// <summary>
    /// Parses the contents of a Linux OsRelease file.
    /// </summary>
    /// <param name="fileContents">The Linux OsRelease file contents.</param>
    /// <returns>The parsed Linux OsRelease file contents as a LinuxOsReleaseInfo object.</returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if run on an operating system that is not Linux-based.</exception>
    /// <exception cref="ArgumentException">Thrown if the string array provided hasn't come from an os-release file.</exception>
    [SupportedOSPlatform("linux")]
    public LinuxOsReleaseInfo ParseLinuxOsRelease(string[] fileContents)
    {
        ArgumentNullException.ThrowIfNull(fileContents);
        
        if (!OperatingSystem.IsLinux())
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);

        if (!fileContents.Any(x => x.ToLower(CultureInfo.CurrentCulture).Contains("id=", StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException(Resources.Exceptions_Arguments_NotOsReleaseContents, nameof(fileContents));
        
        LinuxOsReleaseInfo linuxDistroInfo = new();
        
        fileContents = ParserHelper.RemoveUnwantedCharacters(fileContents).ToArray();
        
        foreach (string line in fileContents)
        {
            string lineUpper = line.ToUpper(CultureInfo.CurrentCulture);

            if (lineUpper.Contains("name=", StringComparison.OrdinalIgnoreCase) &&
                !lineUpper.Contains("version", StringComparison.OrdinalIgnoreCase))
            {
                if (lineUpper.StartsWith("pretty_", StringComparison.OrdinalIgnoreCase))
                {
                    linuxDistroInfo.PrettyName =
                        line.Replace("PRETTY_NAME=", string.Empty);
                }

                if (!lineUpper.Contains("pretty", StringComparison.OrdinalIgnoreCase) &&
                    !lineUpper.Contains("CODE", StringComparison.OrdinalIgnoreCase))
                {
                    linuxDistroInfo.Name = line.Replace("NAME=", string.Empty);
                }
            }

            if (lineUpper.Contains("version=", StringComparison.OrdinalIgnoreCase))
            {
                if (lineUpper.Contains("id=", StringComparison.OrdinalIgnoreCase))
                {
                    linuxDistroInfo.VersionId =
                        line.Replace("VERSION_ID=", string.Empty);
                }
                else if (!lineUpper.Contains("id=", StringComparison.OrdinalIgnoreCase) && 
                         lineUpper.Contains("code", StringComparison.OrdinalIgnoreCase))
                {
                    linuxDistroInfo.VersionCodename =
                        line.Replace("VERSION_CODENAME=", string.Empty);
                }
                else if (!lineUpper.Contains("id=", StringComparison.OrdinalIgnoreCase) &&
                         !lineUpper.Contains("code", StringComparison.OrdinalIgnoreCase))
                {
                    linuxDistroInfo.Version = line.Replace("VERSION=", string.Empty);
                }
            }

            if (lineUpper.Contains("id", StringComparison.OrdinalIgnoreCase))
            {
                if (lineUpper.Contains("id_like=", StringComparison.OrdinalIgnoreCase))
                {
                    string identifiers = line.Replace("ID_LIKE=", string.Empty);

                    if (identifiers.Contains(" ", StringComparison.OrdinalIgnoreCase) ||
                        identifiers.Contains(' ', StringComparison.OrdinalIgnoreCase))
                    {
                        linuxDistroInfo.IdentifierLike = identifiers.Split(" ");
                    }
                    else
                    {
                        linuxDistroInfo.IdentifierLike = [line];
                    }
                }
                else if (!lineUpper.Contains("version", StringComparison.OrdinalIgnoreCase))
                {
                    linuxDistroInfo.Identifier = line.Replace("ID=", string.Empty);
                }
            }

            if (lineUpper.Contains("url=", StringComparison.OrdinalIgnoreCase))
            {
                if (lineUpper.StartsWith("home_", StringComparison.OrdinalIgnoreCase))
                {
                    linuxDistroInfo.HomeUrl = line.Replace("HOME_URL=", string.Empty);
                }
                else if (lineUpper.StartsWith("support_", StringComparison.OrdinalIgnoreCase))
                {
                    linuxDistroInfo.SupportUrl =
                        line.Replace("SUPPORT_URL=", string.Empty);
                }
                else if (lineUpper.StartsWith("bug_", StringComparison.OrdinalIgnoreCase))
                {
                    linuxDistroInfo.BugReportUrl =
                        line.Replace("BUG_REPORT_URL=", string.Empty);
                }
                else if (lineUpper.StartsWith("privacy_", StringComparison.OrdinalIgnoreCase))
                {
                    linuxDistroInfo.PrivacyPolicyUrl =
                        line.Replace("PRIVACY_POLICY_URL=", string.Empty);
                }
            }
        }

        return linuxDistroInfo;
    }
}