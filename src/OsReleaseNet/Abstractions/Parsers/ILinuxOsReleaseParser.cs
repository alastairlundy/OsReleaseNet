/*
    OsReleaseNet
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Generic;
using System.Runtime.Versioning;

namespace AlastairLundy.OsReleaseNet.Abstractions.Parsers;

/// <summary>
/// 
/// </summary>
public interface ILinuxOsReleaseParser
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileContents"></param>
    /// <returns></returns>
    [SupportedOSPlatform("linux")]
    LinuxOsReleaseInfo ParseLinuxOsRelease(string[] fileContents);
}