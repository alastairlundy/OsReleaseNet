/*
    OsReleaseNet
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Threading.Tasks;

namespace AlastairLundy.OsReleaseNet.Abstractions;

using System.Runtime.Versioning;

/// <summary>
/// /
/// </summary>
public interface IFreeBsdReleaseProvider
{
    /// <summary>
    /// Retrieves the value of the specified property from the current system.
    /// </summary>
    /// <param name="propertyName">The name of the property to retrieve.</param>
    /// <returns>The value of the specified property as a string.</returns>
    [SupportedOSPlatform("freebsd")]
    Task<string?> GetReleaseInfoPropertyValueAsync(string propertyName);
    
    /// <summary>
    /// Retrieves information about the current FreeBSD operating system release.
    /// </summary>
    /// <returns>An object containing information about the FreeBSD operating system release.</returns>
    [SupportedOSPlatform("freebsd")]
    Task<FreeBsdOsReleaseInfo> GetReleaseInfoAsync();
}