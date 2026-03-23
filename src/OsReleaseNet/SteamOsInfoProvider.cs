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

using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace OsReleaseNet;

/// <summary>
/// Provides information about Steam OS,
/// Valve's Linux-based operating system.
/// </summary>
public class SteamOsInfoProvider : ISteamOsInfoProvider
{
    private readonly ILinuxOsReleaseProvider _linuxOsReleaseProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SteamOsInfoProvider"/> class.
    /// 
    /// This constructor takes an implementation of the ILinuxOsReleaseProvider interface as an argument,
    /// which provides the necessary information about the Linux-based operating system.
    /// </summary>
    /// <param name="linuxOsReleaseProvider">The provider of Linux OS release information.</param>
    public SteamOsInfoProvider(ILinuxOsReleaseProvider linuxOsReleaseProvider)
    {
        _linuxOsReleaseProvider = linuxOsReleaseProvider;
    }

    /// <summary>
    /// Detects whether a device running SteamOS 3.x is running in Desktop Mode or in Gaming Mode.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>the SteamOS mode being run if run on SteamOS.</returns>
    /// <exception cref="ArgumentException">Thrown if Holo ISO is detected and if Holo ISO isn't counted as SteamOS.</exception>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't SteamOS 3 or newer</exception>
    [SupportedOSPlatform("linux")]
    public async Task<SteamOSMode> GetSteamOSModeAsync(CancellationToken cancellationToken) 
        => await GetSteamOSModeAsync(false, cancellationToken).ConfigureAwait(true);

    /// <summary>
    /// Detects whether a device running SteamOS 3.x is running in Desktop Mode or in Gaming Mode.
    /// </summary>
    /// <param name="includeHoloIsoAsSteamOs">Whether to consider Holo ISO as Steam OS.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>the SteamOS mode being run if run on SteamOS.</returns>
    /// <exception cref="ArgumentException">Thrown if Holo ISO is detected and if Holo ISO isn't counted as SteamOS.</exception>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't SteamOS 3 or newer</exception>
    [SupportedOSPlatform("linux")]
    public async Task<SteamOSMode> GetSteamOSModeAsync(bool includeHoloIsoAsSteamOs,
        CancellationToken cancellationToken)
    {
        bool isSteamOs = await IsSteamOSAsync(includeHoloIsoAsSteamOs, cancellationToken).ConfigureAwait(true);
        
        if (!isSteamOs)
            throw new PlatformNotSupportedException(
                Resources.Exceptions_PlatformNotSupported_LinuxOnly);
        
        LinuxDistroBase distroBase = await _linuxOsReleaseProvider.GetDistroBaseAsync(cancellationToken)
            .ConfigureAwait(true);

        bool isSteamOsExcludingHolo = await IsSteamOSAsync(false, cancellationToken).ConfigureAwait(true);

        if (distroBase == LinuxDistroBase.Manjaro)
        {
            if (includeHoloIsoAsSteamOs || isSteamOsExcludingHolo)
            {
                return SteamOSMode.DesktopMode;
            }
        }

        if (distroBase == LinuxDistroBase.Arch)
        {
            if (includeHoloIsoAsSteamOs || isSteamOsExcludingHolo)
            {
                return SteamOSMode.GamingMode;
            }
        }

        return SteamOSMode.NotSteamOS;
    }

    /// <summary>
    /// Detects if a Linux distro is Steam OS.
    /// </summary>
    /// <returns>true if running on a SteamOS 3.x-based distribution, returns false otherwise.</returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux-based Operating System.</exception>
    // ReSharper disable once InconsistentNaming
    [SupportedOSPlatform("linux")]
    public async Task<bool> IsSteamOSAsync(CancellationToken cancellationToken) 
        => await IsSteamOSAsync(false, cancellationToken).ConfigureAwait(true);

    /// <summary>
    /// Detects if a Linux distro is Steam OS.
    /// </summary>
    /// <param name="includeHoloIsoAsSteamOs"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>true if running on a SteamOS 3.x-based distribution, returns false otherwise.</returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux-based Operating System.</exception>
    // ReSharper disable once InconsistentNaming
    [SupportedOSPlatform("linux")]
    public async Task<bool> IsSteamOSAsync(bool includeHoloIsoAsSteamOs, CancellationToken cancellationToken)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);

        LinuxOsReleaseInfo distroInfo = await _linuxOsReleaseProvider.GetReleaseInfoAsync(cancellationToken).ConfigureAwait(true);
        LinuxDistroBase distroBase = _linuxOsReleaseProvider.GetDistroBase(distroInfo);

        if (distroBase == LinuxDistroBase.Manjaro || distroBase == LinuxDistroBase.Arch)
        {
            return (includeHoloIsoAsSteamOs && distroInfo.PrettyName.ToLower().Contains("holo")) ||
                   distroInfo.PrettyName.ToLower().Contains("steamos");
        }

        //Fallback to false if it isn't detected as SteamOS.
        return false;
    }
}