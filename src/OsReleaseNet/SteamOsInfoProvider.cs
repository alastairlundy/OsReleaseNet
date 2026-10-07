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

// ReSharper disable InconsistentNaming
// ReSharper disable ConvertToPrimaryConstructor

using System.Threading.Tasks;
using OsReleaseNet.Internal;

namespace OsReleaseNet;

/// <summary>
/// Provides information about Steam OS,
/// Valve's Linux-based operating system.
/// </summary>
public class SteamOsInfoProvider : ISteamOsInfoProvider
{
    private readonly ILinuxOsReleaseProvider _linuxOsReleaseProvider;

    /// <summary>
    /// Initialises a new instance of the <see cref="SteamOsInfoProvider"/> class.
    /// 
    /// This constructor takes an implementation of the ILinuxOsReleaseProvider interface as an argument,
    /// which provides the necessary information about the linux based operating system.
    /// </summary>
    /// <param name="linuxOsReleaseProvider">The provider of Linux OS release information.</param>
    public SteamOsInfoProvider(ILinuxOsReleaseProvider linuxOsReleaseProvider)
    {
        _linuxOsReleaseProvider = linuxOsReleaseProvider;
    }

    /// <summary>
    /// Detects whether a device running SteamOS 3.x is running in Desktop Mode or in Gaming Mode.
    /// </summary>
    /// <returns>the SteamOS mode being run if run on SteamOS; <see cref="SteamOSMode.NotSteamOS"/> otherwise.</returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't Linux-based.</exception>
    [SupportedOSPlatform("linux")]
    public async Task<SteamOSMode> GetSteamOSModeAsync() 
        => await GetSteamOSModeAsync(false).ConfigureAwait(false);
    
    /// <summary>
    /// Detects whether a device running SteamOS 3.x is running in Desktop Mode or in Gaming Mode.
    /// </summary>
    /// <param name="includeHoloIsoAsSteamOs">Whether to consider Holo ISO as Steam OS.</param>
    /// <returns>the SteamOS mode being run if run on SteamOS; <see cref="SteamOSMode.NotSteamOS"/> otherwise.</returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't Linux-based.</exception>
    [SupportedOSPlatform("linux")]
    public async Task<SteamOSMode> GetSteamOSModeAsync(bool includeHoloIsoAsSteamOs)
    {
        (LinuxOsReleaseInfo distroInfo, LinuxDistroBase distroBase) = await GetReleaseInfoAndDistroBaseAsync().ConfigureAwait(false);

        if (IsSteamOsRelease(distroInfo, distroBase, includeHoloIsoAsSteamOs))
        {
            return distroBase switch
            {
                LinuxDistroBase.Manjaro => SteamOSMode.DesktopMode,
                LinuxDistroBase.Arch => SteamOSMode.GamingMode,
                // Unreachable in practice: IsSteamOsRelease only returns true for Manjaro/Arch.
                // Required because the compiler cannot prove the switch is exhaustive.
                _ => SteamOSMode.NotSteamOS,
            };
        }

        //Fallback to NotSteamOS if it isn't detected as SteamOS.
        return SteamOSMode.NotSteamOS;
    }

    /// <summary>
    /// Detects if a Linux distro is Steam OS.
    /// </summary>
    /// <returns>true if running on a SteamOS 3.x based distribution, returns false otherwise.</returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux-based Operating System.</exception>
    // ReSharper disable once InconsistentNaming
    [SupportedOSPlatform("linux")]
    public async Task<bool> IsSteamOSAsync() 
        => await IsSteamOSAsync(false).ConfigureAwait(false);

    /// <summary>
    /// Detects if a Linux distro is Steam OS.
    /// </summary>
    /// <param name="includeHoloIsoAsSteamOs"></param>
    /// <returns>true if running on a SteamOS 3.x based distribution, returns false otherwise.</returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux-based Operating System.</exception>
    // ReSharper disable once InconsistentNaming
    [SupportedOSPlatform("linux")]
    public async Task<bool> IsSteamOSAsync(bool includeHoloIsoAsSteamOs)
    {
        (LinuxOsReleaseInfo distroInfo, LinuxDistroBase distroBase) = await GetReleaseInfoAndDistroBaseAsync().ConfigureAwait(false);

        return IsSteamOsRelease(distroInfo, distroBase, includeHoloIsoAsSteamOs);
    }

    /// <summary>
    /// Fetches the Linux release information once and resolves its distro base through the shared internal core.
    /// </summary>
    /// <remarks>This helper is the single-parse path for the SteamOS checks and mode question - exactly one
    /// <see cref="ILinuxOsReleaseProvider.GetReleaseInfoAsync"/> call per invocation, with the distro base
    /// resolved from that same information.</remarks>
    /// <returns>A tuple of the fetched <see cref="LinuxOsReleaseInfo"/> and the distro base resolved from it.</returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux-based Operating System.</exception>
    private async Task<(LinuxOsReleaseInfo DistroInfo, LinuxDistroBase DistroBase)> GetReleaseInfoAndDistroBaseAsync()
    {
        if (!OperatingSystem.IsLinux())
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);

        LinuxOsReleaseInfo distroInfo = await _linuxOsReleaseProvider.GetReleaseInfoAsync().ConfigureAwait(false);

        return (distroInfo, DistroBaseResolver.Resolve(distroInfo));
    }

    /// <summary>
    /// Determines whether a fetched release information and its resolved distro base identify Steam OS.
    /// </summary>
    /// <remarks>The distro base check plus the <see cref="LinuxOsReleaseInfo.PrettyName"/> check,
    /// including Holo ISO handling, mirrors the pre-rework detection logic unchanged.</remarks>
    /// <param name="distroInfo">The fetched Linux OS release information.</param>
    /// <param name="distroBase">The distro base resolved from <paramref name="distroInfo"/>.</param>
    /// <param name="includeHoloIsoAsSteamOs">Whether to consider Holo ISO as Steam OS.</param>
    /// <returns>true if the release information identifies a SteamOS 3.x based distribution; false otherwise.</returns>
    private static bool IsSteamOsRelease(LinuxOsReleaseInfo distroInfo, LinuxDistroBase distroBase, bool includeHoloIsoAsSteamOs)
    {
        if (distroBase is LinuxDistroBase.Manjaro or LinuxDistroBase.Arch)
        {
            string? prettyName = distroInfo.PrettyName;

            if (string.IsNullOrEmpty(prettyName))
                return false;

            return (includeHoloIsoAsSteamOs && prettyName.Contains("holo", StringComparison.OrdinalIgnoreCase)) ||
                   prettyName.Contains("steamos", StringComparison.OrdinalIgnoreCase);
        }

        //Fallback to false if it isn't detected as SteamOS.
        return false;
    }
}