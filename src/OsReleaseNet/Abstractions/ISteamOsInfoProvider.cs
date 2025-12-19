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

// ReSharper disable InconsistentNaming

namespace OsReleaseNet.Abstractions;

/// <summary>
/// An interface for retrieving information about a SteamOS based operating system.
/// </summary>
public interface ISteamOsInfoProvider
{
    /// <summary>
    /// Retrieves the current SteamOS mode.
    /// </summary>
    /// <returns>The current SteamOS mode.</returns>
    [SupportedOSPlatform("linux")]
    Task<SteamOSMode> GetSteamOSModeAsync();
    
    /// <summary>
    /// Retrieves the current SteamOS mode, including whether to include HoloISO as Steam OS.
    /// </summary>
    /// <param name="includeHoloIsoAsSteamOs">Whether HoloISO should be considered as Steam OS.</param>
    /// <returns>The current SteamOS mode.</returns>
    [SupportedOSPlatform("linux")]
    Task<SteamOSMode> GetSteamOSModeAsync(bool includeHoloIsoAsSteamOs);
    
    /// <summary>
    /// Checks whether the current OS is Steam OS.
    /// </summary>
    /// <returns>True if the current OS is Steam OS; false otherwise.</returns>
    [SupportedOSPlatform("linux")]
    Task<bool> IsSteamOSAsync();
    
    /// <summary>
    /// Checks whether the current system is running a Steam OS, including an option to include HoloISO as Steam OS.
    /// </summary>
    /// <param name="includeHoloIsoAsSteamOs">Whether HoloISO should be considered as Steam OS.</param>
    /// <returns>A boolean indicating whether the system is running a Steam OS.</returns>
    [SupportedOSPlatform("linux")]
    Task<bool> IsSteamOSAsync(bool includeHoloIsoAsSteamOs);
}