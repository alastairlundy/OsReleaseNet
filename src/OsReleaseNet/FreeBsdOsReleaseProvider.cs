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
using System.Threading.Tasks;

namespace OsReleaseNet;

/// <summary>
/// A class that provides information about the current FreeBSD operating system.
/// </summary>
public class FreeBsdOsReleaseProvider : IFreeBsdOsReleaseProvider
{
    private readonly IFreeBsdOsReleaseParser _freeBsdOsReleaseParser;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="freeBsdOsReleaseParser"></param>
    public FreeBsdOsReleaseProvider(IFreeBsdOsReleaseParser freeBsdOsReleaseParser)
    {
        _freeBsdOsReleaseParser = freeBsdOsReleaseParser;
    }

    /// <summary>
    /// Retrieves the value of a specific property in the FreeBSD OS Release Info.
    /// </summary>
    /// <param name="propertyName">The name of the property to retrieve.</param>
    /// <returns>The value of the specified property if found; a null string otherwise.</returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System
    /// that isn't FreeBSD-based.</exception>
    [SupportedOSPlatform("freebsd")]
    public async Task<string?> GetReleaseInfoPropertyValueAsync(string propertyName)
    {
        ArgumentException.ThrowIfNullOrEmpty(propertyName);
        
        if (!OperatingSystem.IsFreeBSD())
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);
            
        string[] resultArray = await File.ReadAllLinesAsync("/etc/os-release");
        
        string? result = ParserHelper.RemoveUnwantedCharacters(resultArray)
            .FirstOrDefault(x => x.ToUpper().Contains(propertyName.ToUpper()));

        result = result?.Replace(propertyName, string.Empty)
            .Replace("=", string.Empty);

        return result;
    }

    /// <summary>
    /// Retrieves the FreeBSD OS release information.
    /// </summary>
    /// <returns>The FreeBSD OS release information.</returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System
    /// that isn't FreeBSD-based.</exception>
    [SupportedOSPlatform("freebsd")]
    public async Task<FreeBsdOsReleaseInfo> GetReleaseInfoAsync()
    {
        if (!OperatingSystem.IsFreeBSD())
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);

        string[] resultArray = await File.ReadAllLinesAsync("/etc/os-release");
        
        return await Task.FromResult(_freeBsdOsReleaseParser.ParseFreeBsdRelease(resultArray));
    }
}