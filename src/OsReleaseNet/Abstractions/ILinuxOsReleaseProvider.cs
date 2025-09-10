/*
    OsReleaseNet
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace AlastairLundy.OsReleaseNet.Abstractions;

/// <summary>
/// Defines an interface for retrieving information about a Linux operating system.
/// </summary>
[SupportedOSPlatform("linux")]
public interface ILinuxOsReleaseProvider
{
    /// <summary>
    /// Retrieves the value of the specified property from the current system.
    /// </summary>
    /// <param name="propertyName">The name of the property to retrieve.</param>
    /// <returns>The value of the specified property as a string.</returns>
    [SupportedOSPlatform("linux")]
    Task<string?> GetReleaseInfoPropertyValueAsync(string propertyName);

    /// <summary>
    /// Retrieves information about the current Linux operating system release.
    /// </summary>
    /// <returns>An object containing information about the Linux operating system release.</returns>
    [SupportedOSPlatform("linux")]
    Task<LinuxOsReleaseInfo> GetReleaseInfoAsync();

    /// <summary>
    /// Retrieves information about the base distribution of the current Linux operating system.
    /// </summary>
    /// <returns>The base distribution of the Linux operating system.</returns>
    [SupportedOSPlatform("linux")]
    Task<LinuxDistroBase> GetDistroBaseAsync();

    /// <summary>
    /// Retrieves information about the base distribution of the <see cref="LinuxOsReleaseInfo"/>.
    /// </summary>
    /// <param name="osReleaseInfo"></param>
    /// <returns>The base distribution of the Linux operating system.</returns>
    [SupportedOSPlatform("linux")]
    LinuxDistroBase GetDistroBase(LinuxOsReleaseInfo osReleaseInfo);
}