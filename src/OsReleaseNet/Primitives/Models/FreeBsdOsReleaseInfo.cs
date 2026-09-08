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
/// Represents a FreeBSD Distribution's OsRelease file and information contained therein.
/// </summary>
/// <remarks>All trademarks mentioned belong to their respective owners.</remarks>
public class FreeBsdOsReleaseInfo : IEquatable<FreeBsdOsReleaseInfo>
{
    internal FreeBsdOsReleaseInfo()
    {
        AnsiColor = string.Empty;
        VersionId = string.Empty;
        CpeName = string.Empty;
        Name = string.Empty;
        Identifier = string.Empty;
        Version = string.Empty;
        PrettyName = string.Empty;
        Version = string.Empty;
        HomeUrl = string.Empty;
        BugReportUrl = string.Empty;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="version"></param>
    /// <param name="identifier"></param>
    /// <param name="prettyName"></param>
    /// <param name="versionId"></param>
    /// <param name="homeUrl"></param>
    /// <param name="bugReportUrl"></param>
    /// <param name="ansiColor"></param>
    /// <param name="cpeName"></param>
    public FreeBsdOsReleaseInfo(string name, string version, string identifier, string prettyName, string versionId,
        string homeUrl, string bugReportUrl, string ansiColor, string cpeName)
    {
        Name = name;
        Version = version;
        Identifier = identifier;
        PrettyName = prettyName;
        VersionId = versionId;
        HomeUrl = homeUrl;
        BugReportUrl = bugReportUrl;
        AnsiColor = ansiColor;
        CpeName = cpeName;
    }

    /// <summary>
    /// Represents the ANSI colour code associated with the FreeBSD distribution,
    /// typically used to add colour formatting to terminal output or logs.
    /// </summary>
    public string AnsiColor { get; internal set; }

    /// <summary>
    /// The Common Platform Enumeration (CPE) name of the FreeBSD Distribution,
    /// representing a standardised method of identifying and describing software or operating systems.
    /// </summary>
    public string CpeName { get; internal set; }
    
    /// <summary>
    /// The name of the FreeBSD Distribution.
    /// </summary>
    public string Name { get; internal set; }

    /// <summary>
    /// The FreeBSD distribution display version. 
    /// </summary>
    /// <remarks>This string should not be parsed into a <see cref="Version"/> object
    /// as it may contain a version number and version name. Use <see cref="VersionId"/> instead.</remarks>
    public string Version { get; internal set; }

    /// <summary>
    /// The FreeBSD distribution's identifier.
    /// </summary>
    public string Identifier { get; internal set; }

    /// <summary>
    /// The pretty name/display name for the FreeBSD distribution.
    /// </summary>
    public string PrettyName { get; internal set; }

    /// <summary>
    /// The FreeBSD distribution's version number.
    /// </summary>
    public string VersionId { get; internal set; }

    /// <summary>
    /// The FreeBSD distribution's homepage/website.
    /// </summary>
    public string HomeUrl { get; internal set; }
    
    /// <summary>
    /// The FreeBSD distribution's bug reporting website url (if provided).
    /// </summary>
    public string BugReportUrl { get; internal set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(FreeBsdOsReleaseInfo? other)
    {
        if (other is null)
            return false;
        
        return string.Equals(AnsiColor, other.AnsiColor, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(CpeName, other.CpeName, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Version, other.Version, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Identifier, other.Identifier, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(PrettyName, other.PrettyName, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(VersionId, other.VersionId, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(HomeUrl, other.HomeUrl, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(BugReportUrl, other.BugReportUrl, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;

        if (obj is FreeBsdOsReleaseInfo other)
            return Equals(other);

        return false;
    }
    
    /// <inheritdoc/>
    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(AnsiColor, StringComparer.OrdinalIgnoreCase);
        hashCode.Add(CpeName, StringComparer.OrdinalIgnoreCase);
        hashCode.Add(Name, StringComparer.OrdinalIgnoreCase);
        hashCode.Add(Version, StringComparer.OrdinalIgnoreCase);
        hashCode.Add(Identifier, StringComparer.OrdinalIgnoreCase);
        hashCode.Add(PrettyName, StringComparer.OrdinalIgnoreCase);
        hashCode.Add(VersionId, StringComparer.OrdinalIgnoreCase);
        hashCode.Add(HomeUrl, StringComparer.OrdinalIgnoreCase);
        hashCode.Add(BugReportUrl, StringComparer.OrdinalIgnoreCase);
        return hashCode.ToHashCode();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool Equals(FreeBsdOsReleaseInfo? left, FreeBsdOsReleaseInfo? right)
    {
        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(FreeBsdOsReleaseInfo? left, FreeBsdOsReleaseInfo? right) => Equals(left, right);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(FreeBsdOsReleaseInfo? left, FreeBsdOsReleaseInfo? right) => !Equals(left, right);
}