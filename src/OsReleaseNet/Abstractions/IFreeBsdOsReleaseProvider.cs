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

using System.Threading.Tasks;

namespace AlastairLundy.OsReleaseNet.Abstractions;

/// <summary>
/// An interface for retrieving information about a FreeBSD operating system.
/// </summary>
public interface IFreeBsdOsReleaseProvider
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