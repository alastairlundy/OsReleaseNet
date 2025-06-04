/*
    OsReleaseNet
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
#else
using System.Runtime.Versioning;
#endif

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AlastairLundy.DotExtensions.Strings;

using AlastairLundy.OsReleaseNet.Abstractions;

using AlastairLundy.OsReleaseNet.Internal.Localizations;

namespace AlastairLundy.OsReleaseNet
{
    /// <summary>
    /// Provides information about the current Linux OS Release.
    /// </summary>
    public class LinuxOsReleaseProvider : ILinuxOsReleaseProvider
    {
        /// <summary>
        /// Retrieves the value of a specific property in the Linux OS Release Info.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve.</param>
        /// <returns>The value of the specified property if found; a null string otherwise.</returns>
        /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System
        /// that isn't Linux-based.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
        public async Task<string?> GetReleaseInfoPropertyValueAsync(string propertyName)
        {
            if (OperatingSystem.IsLinux() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
            }
            
#if NET6_0_OR_GREATER
        string[] resultArray = await File.ReadAllLinesAsync("/etc/os-release");
#else
            string[] resultArray = await Task.Run(() => File.ReadAllLines("/etc/os-release"));
#endif

            resultArray = RemoveUnwantedCharacters(resultArray);
        
            string? result = resultArray.FirstOrDefault(x => x.Contains(propertyName));

            if (result is not null)
            {
                result = result.Replace(propertyName, string.Empty)
                    .Replace("=", string.Empty);
            }

            return result;
            
        }

        /// <summary>
        /// Retrieves the Linux OS release information.
        /// </summary>
        /// <returns>The Linux OS release information.</returns>
        /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System
        /// that isn't Linux-based.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
        public async Task<LinuxOsReleaseInfo> GetReleaseInfoAsync()
        {
            if (OperatingSystem.IsLinux() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
            }

#if NET6_0_OR_GREATER
            string[] resultArray = await File.ReadAllLinesAsync("/etc/os-release");
#else
            string[] resultArray = await Task.Run(() => File.ReadAllLines("/etc/os-release"));
#endif
        
            resultArray = RemoveUnwantedCharacters(resultArray);

            return await Task.FromResult(ParseOsReleaseInfo(resultArray));
        }

        /// <summary>
        /// Retrieves the base Linux distribution information.
        /// </summary>
        /// <returns>The base Linux distribution information.</returns>
        /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System
        /// that isn't Linux-based.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]    
#endif
        public async Task<LinuxDistroBase> GetDistroBaseAsync()
        {
            if (OperatingSystem.IsLinux() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
            }
        
            LinuxOsReleaseInfo osReleaseInfo = await GetReleaseInfoAsync();
        
            return GetDistroBase(osReleaseInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="osReleaseInfo"></param>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("linux")]
#endif
        public LinuxDistroBase GetDistroBase(LinuxOsReleaseInfo osReleaseInfo)
        {
            string identifierLike = osReleaseInfo.Identifier_Like.ToLower();
            
            return identifierLike switch
            {
                "debian" => LinuxDistroBase.Debian,
                "ubuntu" => LinuxDistroBase.Ubuntu,
                "arch" => LinuxDistroBase.Arch,
                "manjaro" => LinuxDistroBase.Manjaro,
                "fedora" => LinuxDistroBase.Fedora,
                "rhel" or "oracle" or "centos" => LinuxDistroBase.RHEL,
                "suse" => LinuxDistroBase.SUSE,
                _ => LinuxDistroBase.NotDetected
            };
        }

        /// <summary>
        /// Parses an array of strings to extract the Linux OS release information.
        /// </summary>
        /// <param name="resultArray">The input array containing strings that need to be parsed for OS release information.</param>
        /// <returns>The extracted Linux OS release information.</returns>
        private LinuxOsReleaseInfo ParseOsReleaseInfo(string[] resultArray)
        {
            LinuxOsReleaseInfo linuxDistroInfo = new LinuxOsReleaseInfo();
            
            for (int index = 0; index < resultArray.Length; index++)
            {
                string line = resultArray[index].ToUpper();

                if (line.Contains("NAME=") && !line.Contains("VERSION"))
                {

                    if (line.StartsWith("PRETTY_"))
                    {
                        linuxDistroInfo.PrettyName =
                            resultArray[index].Replace("PRETTY_NAME=", string.Empty);
                    }

                    if (!line.Contains("PRETTY") && !line.Contains("CODE"))
                    {
                        linuxDistroInfo.Name = resultArray[index]
                            .Replace("NAME=", string.Empty);
                    }
                }

                if (line.Contains("VERSION="))
                {
                    if (line.Contains("LTS"))
                    {
                        linuxDistroInfo.IsLongTermSupportRelease = true;
                    }
                    else
                    {
                        linuxDistroInfo.IsLongTermSupportRelease = false;
                    }

                    if (line.Contains("ID="))
                    {
                        linuxDistroInfo.VersionId =
                            resultArray[index].Replace("VERSION_ID=", string.Empty);
                    }
                    else if (!line.Contains("ID=") && line.Contains("CODE"))
                    {
                        linuxDistroInfo.VersionCodename =
                            resultArray[index].Replace("VERSION_CODENAME=", string.Empty);
                    }
                    else if (!line.Contains("ID=") && !line.Contains("CODE"))
                    {
                        linuxDistroInfo.Version = resultArray[index].Replace("VERSION=", string.Empty)
                            .Replace("LTS", string.Empty);
                    }
                }

                if (line.Contains("ID"))
                {
                    if (line.Contains("ID_LIKE="))
                    {
                        linuxDistroInfo.Identifier_Like =
                            resultArray[index].Replace("ID_LIKE=", string.Empty);

                        if (linuxDistroInfo.Identifier_Like.ToLower().Contains("ubuntu") &&
                            linuxDistroInfo.Identifier_Like.ToLower().Contains("debian"))
                        {
                            linuxDistroInfo.Identifier_Like = "ubuntu";
                        }
                    }
                    else if (!line.Contains("VERSION"))
                    {
                        linuxDistroInfo.Identifier = resultArray[index].Replace("ID=", string.Empty);
                    }
                }

                if (line.Contains("URL="))
                {
                    if (line.StartsWith("HOME_"))
                    {
                        linuxDistroInfo.HomeUrl = resultArray[index].Replace("HOME_URL=", string.Empty);
                    }
                    else if (line.StartsWith("SUPPORT_"))
                    {
                        linuxDistroInfo.SupportUrl =
                            resultArray[index].Replace("SUPPORT_URL=", string.Empty);
                    }
                    else if (line.StartsWith("BUG_"))
                    {
                        linuxDistroInfo.BugReportUrl =
                            resultArray[index].Replace("BUG_REPORT_URL=", string.Empty);
                    }
                    else if (line.StartsWith("PRIVACY_"))
                    {
                        linuxDistroInfo.PrivacyPolicyUrl =
                            resultArray[index].Replace("PRIVACY_POLICY_URL=", string.Empty);
                    }
                }
            }

            return linuxDistroInfo;
        }

        /// <summary>
        /// Removes unwanted characters from an array of strings.
        /// </summary>
        /// <param name="resultArray">The input array containing strings that may contain unwanted characters.</param>
        /// <returns>An array of strings with unwanted characters removed.</returns>
        private string[] RemoveUnwantedCharacters(string[] resultArray)
        {
            char[] delimiter = ['\t', '"'];

            IEnumerable<string> newResultArray = resultArray
                .Where(x => string.IsNullOrWhiteSpace(x) == false)
                .Select(x => x.RemoveEscapeCharacters());
            
            foreach (char c in delimiter)
            {
                newResultArray = newResultArray.Select(x => x.Replace(c.ToString(), string.Empty));
            }

            return newResultArray.ToArray();
        }
    }
}