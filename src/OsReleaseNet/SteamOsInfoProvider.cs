﻿/*
    OsReleaseNet
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

// ReSharper disable InconsistentNaming
// ReSharper disable ConvertToPrimaryConstructor

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using AlastairLundy.OsReleaseNet.Abstractions;

using AlastairLundy.OsReleaseNet.Internal.Localizations;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

namespace AlastairLundy.OsReleaseNet;

/// <summary>
/// Provides information about Steam OS,
/// Valve's linux based operating system.
/// </summary>
public class SteamOsInfoProvider : ISteamOsInfoProvider
{
    private readonly ILinuxOsReleaseProvider _linuxOsReleaseProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SteamOsInfoProvider"/> class.
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
    /// <returns>the SteamOS mode being run if run on SteamOS.</returns>
    /// <exception cref="ArgumentException">Thrown if Holo ISO is detected and if Holo ISO isn't counted as SteamOS.</exception>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't SteamOS 3 or newer</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public async Task<SteamOSMode> GetSteamOSModeAsync()
    {
        return await GetSteamOSModeAsync(false);
    }
    
    /// <summary>
    /// Detects whether a device running SteamOS 3.x is running in Desktop Mode or in Gaming Mode.
    /// </summary>
    /// <param name="includeHoloIsoAsSteamOs">Whether to consider Holo ISO as Steam OS.</param>
    /// <returns>the SteamOS mode being run if run on SteamOS.</returns>
    /// <exception cref="ArgumentException">Thrown if Holo ISO is detected and if Holo ISO isn't counted as SteamOS.</exception>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't SteamOS 3 or newer</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public async Task<SteamOSMode> GetSteamOSModeAsync(bool includeHoloIsoAsSteamOs)
    {
        bool isSteamOs = await IsSteamOSAsync(includeHoloIsoAsSteamOs);
        
        if (isSteamOs == false)
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);    
        }
        
        LinuxDistroBase distroBase = await _linuxOsReleaseProvider.GetDistroBaseAsync();

        bool isSteamOsExcludingHolo = await IsSteamOSAsync(false);

        if (distroBase == LinuxDistroBase.Manjaro)
        {
            if (includeHoloIsoAsSteamOs || isSteamOsExcludingHolo)
            {
                return SteamOSMode.DesktopMode;
            }

            return SteamOSMode.NotSteamOS;
        }

        if (distroBase == LinuxDistroBase.Arch)
        {
            if (includeHoloIsoAsSteamOs || isSteamOsExcludingHolo)
            {
                return SteamOSMode.GamingMode;
            }

            return SteamOSMode.NotSteamOS;
        }

        return SteamOSMode.NotSteamOS;
    }

    /// <summary>
    /// Detects if a Linux distro is Steam OS.
    /// </summary>
    /// <returns>true if running on a SteamOS 3.x based distribution, returns false otherwise.</returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux-based Operating System.</exception>
    // ReSharper disable once InconsistentNaming
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public async Task<bool> IsSteamOSAsync()
    {
        return await IsSteamOSAsync(false);
    }

    /// <summary>
    /// Detects if a Linux distro is Steam OS.
    /// </summary>
    /// <param name="includeHoloIsoAsSteamOs"></param>
    /// <returns>true if running on a SteamOS 3.x based distribution, returns false otherwise.</returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux-based Operating System.</exception>
    // ReSharper disable once InconsistentNaming
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public async Task<bool> IsSteamOSAsync(bool includeHoloIsoAsSteamOs)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) == false)
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
        }

        LinuxOsReleaseInfo distroInfo = await _linuxOsReleaseProvider.GetReleaseInfoAsync();
        LinuxDistroBase distroBase = await _linuxOsReleaseProvider.GetDistroBaseAsync();

        if (distroBase == LinuxDistroBase.Manjaro || distroBase == LinuxDistroBase.Arch)
        {
            return includeHoloIsoAsSteamOs && distroInfo.PrettyName.ToLower().Contains("holo") ||
                   distroInfo.PrettyName.ToLower().Contains("steamos");
        }
        if (distroBase == LinuxDistroBase.Debian && distroInfo.PrettyName.ToLower().Contains("steamos"))
        {
            // ReSharper disable once DuplicatedStatements
            return false;
        }

        //Fallback to false if it isn't detected as SteamOS.
        return false;
    }
}