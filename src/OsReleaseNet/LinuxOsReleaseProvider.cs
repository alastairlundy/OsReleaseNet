/*
    OsReleaseNet
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Runtime.Versioning;
using System.IO;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using AlastairLundy.DotExtensions.Strings;

using AlastairLundy.OsReleaseNet.Abstractions;
using AlastairLundy.OsReleaseNet.Helpers;
using AlastairLundy.OsReleaseNet.Internal.Localizations;

namespace AlastairLundy.OsReleaseNet;

/// <summary>
/// Provides information about the current Linux OS Release.
/// </summary>
public class LinuxOsReleaseProvider : ILinuxOsReleaseProvider
{
    private readonly OsReleaseParser _osReleaseParser = new OsReleaseParser();

    /// <summary>
    /// Retrieves the value of a specific property in the Linux OS Release Info.
    /// </summary>
    /// <param name="propertyName">The name of the property to retrieve.</param>
    /// <returns>The value of the specified property if found; a null string otherwise.</returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System
    /// that isn't Linux-based.</exception>
    [SupportedOSPlatform("linux")]
    public async Task<string?> GetReleaseInfoPropertyValueAsync(string propertyName)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) == false)
            throw new PlatformNotSupportedException(Resources.
                Exceptions_PlatformNotSupported_LinuxOnly);
            
        string[] resultArray = await File.ReadAllLinesAsync("/etc/os-release");
        
        string? result = RemoveUnwantedCharacters(resultArray)
        .FirstOrDefault(x => x.ToUpper().Contains(propertyName.ToUpper()));
            
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
    [SupportedOSPlatform("linux")]
    public async Task<LinuxOsReleaseInfo> GetReleaseInfoAsync()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) == false)
            throw new PlatformNotSupportedException(
                Resources.Exceptions_PlatformNotSupported_LinuxOnly);

        string[] resultArray = await File.ReadAllLinesAsync("/etc/os-release");

        LinuxOsReleaseInfo result = _osReleaseParser.ParseOsReleaseInfo(
            RemoveUnwantedCharacters(resultArray));

        return await Task.FromResult(result);
    }

    /// <summary>
    /// Retrieves the base Linux distribution information.
    /// </summary>
    /// <returns>The base Linux distribution information.</returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System
    /// that isn't Linux-based.</exception>
    [SupportedOSPlatform("linux")]    
    public async Task<LinuxDistroBase> GetDistroBaseAsync()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) == false)
            throw new PlatformNotSupportedException(Resources.
                Exceptions_PlatformNotSupported_LinuxOnly);

        LinuxOsReleaseInfo osReleaseInfo = new LinuxOsReleaseInfo();
        
        string? result =  await GetReleaseInfoPropertyValueAsync("ID_LIKE=");

        if (result is not null)
            osReleaseInfo.IdentifierLike = result.Split(" ");
        else
            osReleaseInfo = await GetReleaseInfoAsync();
        
        return GetDistroBase(osReleaseInfo);
    }

    /// <summary>
    /// Compares the Linux Os Release Identifier information to determine what distro it is based on and returns it as a LinuxDistroBase enum.
    /// </summary>
    /// <remarks>This method is, by design, not asynchronous. It performs no asynchronous operations.
    /// <para>Whilst this method does not throw an Exception if it is not run on Linux, there is no way to provide valid LinuxOsRelease from a  </para></remarks>
    /// <param name="osReleaseInfo">The LinuxOsReleaseInfo object to parse.</param>
    /// <returns>The detected LinuxDistroBase as an enum if successfully detected,
    /// the LinuxDistroBase.NotDetected enum value otherwise.</returns>
    [SupportedOSPlatform("linux")]
    public LinuxDistroBase GetDistroBase(LinuxOsReleaseInfo osReleaseInfo)
    {
        bool containsMultipleIdentifiers = osReleaseInfo.IdentifierLike.Length > 1;

        string identifierLike;
        
        if (containsMultipleIdentifiers)
        {
            identifierLike = osReleaseInfo.IdentifierLike.First().ToLower();
        }
        else
        {
            identifierLike = osReleaseInfo.IdentifierLike.First().ToLower();
        }
         
            
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
    /// Removes unwanted characters from an array of strings.
    /// </summary>
    /// <param name="resultArray">The input array containing strings that may contain unwanted characters.</param>
    /// <returns>An array of strings with unwanted characters removed.</returns>
    private IEnumerable<string> RemoveUnwantedCharacters(string[] resultArray)
    {
        IEnumerable<string> newResults = resultArray
            .Where(x => string.IsNullOrWhiteSpace(x) == false && x.Equals(string.Empty) == false)
            .Select(x => x.RemoveEscapeCharacters())
            .Select(x => x.Replace('"'.ToString(), string.Empty));
        
        return newResults;
    }
}