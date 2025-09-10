/*
    OsReleaseNet
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Runtime.Versioning;

namespace AlastairLundy.OsReleaseNet.Abstractions.Parsers;

/// <summary>
/// An interface for parsing the contents of a Linux OsRelease file.
/// </summary>
public interface ILinuxOsReleaseParser
{
    /// <summary>
    /// Parses the contents of a Linux OsRelease file.
    /// </summary>
    /// <param name="fileContents">The Linux OsRelease file contents.</param>
    /// <returns>The parsed Linux OsRelease file contents as a LinuxOsReleaseInfo object.</returns>
    [SupportedOSPlatform("linux")]
    LinuxOsReleaseInfo ParseLinuxOsRelease(string[] fileContents);
}