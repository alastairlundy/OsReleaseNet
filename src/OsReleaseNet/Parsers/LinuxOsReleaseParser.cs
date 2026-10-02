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

        if (!fileContents.Any(x => x.Contains("id=", StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException(Resources.Exceptions_Arguments_NotOsReleaseContents, nameof(fileContents));
        
        LinuxOsReleaseInfo linuxDistroInfo = new();
        
        fileContents = ParserHelper.RemoveUnwantedCharacters(fileContents).ToArray();
        
        foreach (string line in fileContents)
        {
            string lineUpper = line.ToUpperInvariant();

            if (lineUpper.StartsWith("PRETTY_NAME=", StringComparison.Ordinal))
            {
                linuxDistroInfo.PrettyName =
                    line.Substring("PRETTY_NAME=".Length);
            }
            else if (lineUpper.StartsWith("NAME=", StringComparison.Ordinal))
            {
                linuxDistroInfo.Name = line.Substring("NAME=".Length);
            }
            // CPE_NAME= and *CODENAME= lines are intentionally ignored here;
            // they are handled (or explicitly skipped) in their own blocks.

            if (lineUpper.StartsWith("VERSION_ID=", StringComparison.Ordinal))
            {
                linuxDistroInfo.VersionId =
                    line.Substring("VERSION_ID=".Length);
            }
            else if (lineUpper.StartsWith("VERSION_CODENAME=", StringComparison.Ordinal))
            {
                linuxDistroInfo.VersionCodename =
                    line.Substring("VERSION_CODENAME=".Length);
            }
            else if (lineUpper.StartsWith("VERSION=", StringComparison.Ordinal))
            {
                linuxDistroInfo.Version = line.Substring("VERSION=".Length);
            }
            else if (lineUpper.StartsWith("UBUNTU_CODENAME=", StringComparison.Ordinal) &&
                     string.IsNullOrEmpty(linuxDistroInfo.VersionCodename))
            {
                linuxDistroInfo.VersionCodename =
                    line.Substring("UBUNTU_CODENAME=".Length);
            }

            if (lineUpper.StartsWith("ID_LIKE=", StringComparison.Ordinal))
            {
                string identifiers = line.Substring("ID_LIKE=".Length);

                linuxDistroInfo.IdentifierLike = identifiers.Split([' '],
                    StringSplitOptions.RemoveEmptyEntries);
            }
            else if (lineUpper.StartsWith("ID=", StringComparison.Ordinal))
            {
                linuxDistroInfo.Identifier = line.Substring("ID=".Length);
            }
            // VERSION_ID=, VARIANT_ID= and VARIANT= are intentionally not mapped to Identifier.

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