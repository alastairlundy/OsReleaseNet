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

namespace OsReleaseNet;

/// <summary>
/// Represents the base of a Linux distribution.
/// </summary>
/// <remarks>
/// This enum is used to identify the foundational Linux distribution upon which other distributions may be based.
/// It helps classify Linux distributions into major families such as Debian-based, Fedora-based, and others.
/// </remarks>
public enum LinuxDistroBase
{
    /// <summary>
    /// Represents the Debian family of Linux distributions.
    /// </summary>
    /// <remarks>
    /// The Debian value is used to classify Linux distributions that are based on or derived from
    /// the Debian operating system. Examples of such distributions include Debian itself, as well as
    /// distributions that use Debian's package management system.
    /// </remarks>
    Debian,

    /// <summary>
    /// Represents the Ubuntu family of Linux distributions.
    /// </summary>
    /// <remarks>
    /// The Ubuntu value identifies Linux distributions that are derived from or based on Ubuntu.
    /// Examples include Ubuntu itself, along with flavors and remixes that rely on its ecosystem
    /// and package management system.
    /// </remarks>
    Ubuntu,

    /// <summary>
    /// Represents the Arch family of Linux distributions.
    /// </summary>
    /// <remarks>
    /// The Arch value is used to classify Linux distributions that are based on Arch Linux.
    /// </remarks>
    Arch,

    /// <summary>
    /// Represents the Manjaro family of Linux distributions.
    /// </summary>
    /// <remarks>
    /// The Manjaro value is used to classify Linux distributions that are directly derived from or heavily
    /// inspired by the Manjaro operating system.
    /// </remarks>
    Manjaro,

    /// <summary>
    /// Represents the Fedora family of Linux distributions.
    /// </summary>
    /// <remarks>
    /// The Fedora value is used to classify Linux distributions that are based on or derived from
    /// the Fedora operating system. This includes distributions that utilize the Fedora package
    /// management system (RPM).
    /// </remarks>
    Fedora,

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Represents the Red Hat Enterprise Linux (RHEL) family of Linux distributions.
    /// </summary>
    /// <remarks>
    /// The RHEL value is used to classify Linux distributions that are based on or derived from
    /// Red Hat Enterprise Linux.
    /// </remarks>
    RHEL,

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Represents the SUSE family of Linux distributions.
    /// </summary>
    /// <remarks>
    /// The SUSE value is used to classify Linux distributions that are based on or derived from
    /// SUSE.
    /// </remarks>
    SUSE,

    /// <summary>
    /// Represents a state where the base of a Linux distribution could not be determined.
    /// </summary>
    /// <remarks>
    /// The NotDetected value is used when the system is unable to identify the foundational
    /// Linux distribution of an operating system. This may occur if the necessary system data
    /// is unavailable or incomplete.
    /// </remarks>
    NotDetected,

    /// <summary>
    /// Represents a base type for Linux distributions that are not supported by the classification.
    /// </summary>
    /// <remarks>
    /// The NotSupported value is used when the Linux distribution does not align with any recognized
    /// or supported base family of distributions. This serves as a fallback for unclassified or
    /// unsupported distributions.
    /// </remarks>
    NotSupported,
}