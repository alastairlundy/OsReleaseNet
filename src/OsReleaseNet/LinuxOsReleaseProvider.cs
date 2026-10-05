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
using OsReleaseNet.Internal;

namespace OsReleaseNet;

/// <summary>
/// A class that provides information about the current Linux OS Release.
/// </summary>
public class LinuxOsReleaseProvider : ILinuxOsReleaseProvider
{
    private readonly ILinuxOsReleaseParser _linuxOsReleaseParser;
    private readonly Func<Task<string[]>> _readOsReleaseLines;

    /// <summary>
    /// Initialises a new instance of the <see cref="LinuxOsReleaseProvider"/> class.
    /// </summary>
    /// <param name="linuxOsReleaseParser">The parser used to turn the raw os-release lines into a <see cref="LinuxOsReleaseInfo"/>.</param>
    /// <param name="readOsReleaseLines">The optional line source returning the raw os-release lines.
    /// When omitted, the system-wide os-release file is read.</param>
    public LinuxOsReleaseProvider(ILinuxOsReleaseParser linuxOsReleaseParser,
        Func<Task<string[]>>? readOsReleaseLines = null)
    {
        _linuxOsReleaseParser = linuxOsReleaseParser;
        _readOsReleaseLines = readOsReleaseLines ?? DistroBaseResolver.DefaultReadOsReleaseLines;
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
        if (!OperatingSystem.IsLinux())
            throw new PlatformNotSupportedException(
                Resources.Exceptions_PlatformNotSupported_LinuxOnly);

        string[] resultArray = await _readOsReleaseLines().ConfigureAwait(false);

        return _linuxOsReleaseParser.ParseLinuxOsRelease(resultArray);
    }

    /// <summary>
    /// Retrieves the base Linux distribution information.
    /// </summary>
    /// <remarks>The full release information is parsed once and the distro base is resolved from it
    /// by the shared internal core - the <see cref="LinuxOsReleaseInfo.IdentifierLike"/> entries are
    /// checked first and <see cref="LinuxOsReleaseInfo.Identifier"/> is the fallback.</remarks>
    /// <returns>The base Linux distribution information.</returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System
    /// that isn't Linux-based.</exception>
    [SupportedOSPlatform("linux")]
    public async Task<LinuxDistroBase> GetDistroBaseAsync()
    {
        if (!OperatingSystem.IsLinux())
            throw new PlatformNotSupportedException(Resources.
                Exceptions_PlatformNotSupported_LinuxOnly);

        LinuxOsReleaseInfo fullInfo = await GetReleaseInfoAsync().ConfigureAwait(false);

        return DistroBaseResolver.Resolve(fullInfo);
    }
}
