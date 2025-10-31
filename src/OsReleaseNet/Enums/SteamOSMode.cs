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

namespace AlastairLundy.OsReleaseNet;

/// <summary>
/// The mode that SteamOS 3.x and newer is running in.
/// </summary>
// ReSharper disable once InconsistentNaming
public enum SteamOSMode
{
    /// <summary>
    /// The normal UI of the SteamDeck without a desktop environment running.
    /// </summary>
    GamingMode,
    /// <summary>
    /// The mode where the Manjaro desktop environment is running.
    /// </summary>
    DesktopMode,
    // ReSharper disable once InconsistentNaming
    NotSteamOS
}