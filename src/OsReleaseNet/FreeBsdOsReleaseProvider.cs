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
using System.Threading;
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
    /// Retrieves the value of a specific property in the FreeBSD OS Release Information.
    /// </summary>
    /// <param name="propertyName">The name of the property to retrieve.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The value of the specified property if found; a null string otherwise.</returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System
    /// that isn't FreeBSD-based.</exception>
    [SupportedOSPlatform("freebsd")]
    public async Task<string?> GetReleaseInfoPropertyValueAsync(string propertyName,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(propertyName);
        
        if (!OperatingSystem.IsFreeBSD())
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);
            
        string[] osReleaseInfoLines = await File.ReadAllLinesAsync("/etc/os-release", cancellationToken).ConfigureAwait(true);
        
        string? result = ParserHelper.RemoveUnwantedCharacters(osReleaseInfoLines)
            .FirstOrDefault(x => x.ToUpper().Contains(propertyName.ToUpper(), StringComparison.OrdinalIgnoreCase));

        result = result?.Replace(propertyName, string.Empty)
            .Replace("=", string.Empty);

        return result;
    }

    /// <summary>
    /// Retrieves the FreeBSD OS release information.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>The FreeBSD OS release information.</returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System
    /// that isn't FreeBSD-based.</exception>
    [SupportedOSPlatform("freebsd")]
    public async Task<FreeBsdOsReleaseInfo> GetReleaseInfoAsync(CancellationToken cancellationToken)
    {
        if (!OperatingSystem.IsFreeBSD())
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);

        string[] osReleaseInfoLines = await File.ReadAllLinesAsync("/etc/os-release", cancellationToken).ConfigureAwait(true);

        FreeBsdOsReleaseInfo result = _freeBsdOsReleaseParser.ParseFreeBsdRelease(osReleaseInfoLines);
        
        return await Task.FromResult(result).ConfigureAwait(true);
    }
}