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

namespace OsReleaseNet.Abstractions.Parsers;

/// <summary>
/// An interface for parsing the contents of a FreeBSD OsRelease file.
/// </summary>
public interface IFreeBsdOsReleaseParser
{
    /// <summary>
    /// Parses the contents of a FreeBSD OsRelease file.
    /// </summary>
    /// <param name="fileContents">The FreeBSD OsRelease file contents.</param>
    /// <returns>The parsed FreeBSD OsRelease file contents as a FreeBsdReleaseInfo object.</returns>
    [SupportedOSPlatform("freebsd")]
    FreeBsdOsReleaseInfo ParseFreeBsdRelease(string[] fileContents);
}