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
/// 
/// </summary>
public enum LinuxDistroBase
{
    /// <summary>
    /// 
    /// </summary>
    Debian,
    /// <summary>
    /// 
    /// </summary>
    Ubuntu,
    /// <summary>
    /// 
    /// </summary>
    Arch,
    /// <summary>
    /// 
    /// </summary>
    Manjaro,
    /// <summary>
    /// 
    /// </summary>
    Fedora,
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// 
    /// </summary>
    RHEL,
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// 
    /// </summary>
    SUSE,
    /// <summary>
    /// 
    /// </summary>
    NotDetected,
    /// <summary>
    /// 
    /// </summary>
    NotSupported,
}