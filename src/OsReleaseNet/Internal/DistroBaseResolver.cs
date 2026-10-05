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

namespace OsReleaseNet.Internal;

/// <summary>
/// The single authoritative source for the distro base verdict of a Linux distribution
/// and for reading the raw lines of an os-release file.
/// </summary>
/// <remarks>
/// <para>
/// The distro base verdict is resolved from a parsed <see cref="LinuxOsReleaseInfo"/>:
/// the <see cref="LinuxOsReleaseInfo.IdentifierLike"/> entries are checked first - the first
/// entry that maps to a known base family wins - and <see cref="LinuxOsReleaseInfo.Identifier"/>
/// is used as the fallback. A present-but-unmappable <c>ID_LIKE</c> value still falls back
/// to <see cref="LinuxOsReleaseInfo.Identifier"/>.
/// </para>
///
/// <para>The line-source seam replaces the providers' duplicated inline os-release file
/// reads. A line source is a <see cref="Func{TResult}"/> delegate that reads and returns the
/// raw lines of an os-release file. The default line source reads the system-wide os-release
/// file at <see cref="DefaultOsReleaseFilePath"/>; consumers fall back to it when no line
/// source is injected, keeping file reads as the default behavior.</para>
///
/// <para>This module is internal and is consumed in-assembly by the OsReleaseNet providers;
/// it adds nothing to the public API surface.</para>
/// </remarks>
internal static class DistroBaseResolver
{
    /// <summary>
    /// The default os-release file path that <see cref="DefaultReadOsReleaseLines"/> reads from.
    /// </summary>
    internal const string DefaultOsReleaseFilePath = "/etc/os-release";

    /// <summary>
    /// The default line source, reading the raw lines of the os-release file at <see cref="DefaultOsReleaseFilePath"/>.
    /// </summary>
    /// <remarks>
    /// This is the <see cref="Func{TResult}"/> shape the injectable line-source seam relies on;
    /// consumers use it as the fallback when no custom line source is injected.
    /// </remarks>
    internal static readonly Func<Task<string[]>> DefaultReadOsReleaseLines = ReadDefaultOsReleaseFileLinesAsync;

    /// <summary>
    /// Reads the raw lines of the os-release file at <see cref="DefaultOsReleaseFilePath"/>.
    /// </summary>
    /// <returns>The raw lines of the os-release file, in the order they appear in the file.</returns>
    private static async Task<string[]> ReadDefaultOsReleaseFileLinesAsync()
    {
        return await File.ReadAllLinesAsync(DefaultOsReleaseFilePath).ConfigureAwait(false);
    }

    /// <summary>
    /// Resolves the <see cref="LinuxDistroBase"/> of the base distribution family the specified Linux distribution is based on.
    /// </summary>
    /// <remarks>
    /// The <see cref="LinuxOsReleaseInfo.IdentifierLike"/> entries are resolved first,
    /// with the first entry mapping to a known base family winning.
    /// <see cref="LinuxOsReleaseInfo.Identifier"/> is the fallback - including when
    /// <c>ID_LIKE</c> is present but none of its entries maps to a known base family.
    /// </remarks>
    /// <param name="osReleaseInfo">The parsed Linux OS release information to resolve the distro base from.</param>
    /// <returns>The resolved <see cref="LinuxDistroBase"/> if the base family was detected;
    /// <see cref="LinuxDistroBase.NotDetected"/> otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="osReleaseInfo"/> is null.</exception>
    internal static LinuxDistroBase Resolve(LinuxOsReleaseInfo osReleaseInfo)
    {
        ArgumentNullException.ThrowIfNull(osReleaseInfo);

        if (osReleaseInfo.IdentifierLike is not null)
        {
            foreach (string entry in osReleaseInfo.IdentifierLike)
            {
                if (string.IsNullOrWhiteSpace(entry))
                    continue;

                LinuxDistroBase mapped = MapIdentifierToDistroBase(entry.Trim());

                if (mapped != LinuxDistroBase.NotDetected)
                    return mapped;
            }
        }

        if (!string.IsNullOrWhiteSpace(osReleaseInfo.Identifier))
        {
            LinuxDistroBase fromIdentifier = MapIdentifierToDistroBase(osReleaseInfo.Identifier.Trim());

            if (fromIdentifier != LinuxDistroBase.NotDetected)
                return fromIdentifier;
        }

        return LinuxDistroBase.NotDetected;
    }

    /// <summary>
    /// Maps a distribution identifier to the <see cref="LinuxDistroBase"/> of the base distribution family it belongs to.
    /// </summary>
    /// <remarks>The comparison is culture-invariant, normalizing with <see cref="string.ToLowerInvariant"/>.</remarks>
    /// <param name="identifier">The distribution identifier to map.</param>
    /// <returns>The mapped <see cref="LinuxDistroBase"/> if the identifier belongs to a known
    /// base distribution family; <see cref="LinuxDistroBase.NotDetected"/> otherwise.</returns>
    private static LinuxDistroBase MapIdentifierToDistroBase(string identifier)
    {
        string normalized = identifier.ToLowerInvariant();

        return normalized switch
        {
            "debian" => LinuxDistroBase.Debian,
            "ubuntu" => LinuxDistroBase.Ubuntu,
            "arch" => LinuxDistroBase.Arch,
            "manjaro" => LinuxDistroBase.Manjaro,
            "fedora" => LinuxDistroBase.Fedora,
            "rhel" or "oracle" or "centos" => LinuxDistroBase.RHEL,
            "suse" => LinuxDistroBase.SUSE,
            _ => LinuxDistroBase.NotDetected
        };
    }
}
