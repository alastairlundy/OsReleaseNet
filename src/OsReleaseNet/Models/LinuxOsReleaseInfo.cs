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

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace AlastairLundy.OsReleaseNet;

/// <summary>
/// Represents a Linux Distribution's OsRelease file and information contained therein.
/// </summary>
/// <remarks>All trademarks mentioned belong to their respective owners.</remarks>
public class LinuxOsReleaseInfo : IEquatable<LinuxOsReleaseInfo>
{
    
    /// <summary>
    /// 
    /// </summary>
    internal LinuxOsReleaseInfo()
    {
        VersionId = string.Empty;
        Name = string.Empty;
        Identifier = string.Empty;
        IdentifierLike = [];
        Version = string.Empty;
        PrettyName = string.Empty;
        Version = string.Empty;
        VersionCodename = string.Empty;
        HomeUrl = string.Empty;
        SupportUrl = string.Empty;
        BugReportUrl = string.Empty;    
        PrivacyPolicyUrl = string.Empty;
    }
    
    /// <summary>
    /// Instantiates the LinuxOsReleaseInfo object.
    /// </summary>
    /// <param name="name">The name of the distribution.</param>
    /// <param name="version">The distribution display version.</param>
    /// <param name="identifier">The linux distribution's identifier.</param>
    /// <param name="identifierLike">A list of distribution identifiers that the distribution has self-identified as being based on.</param>
    /// <param name="prettyName">The pretty name/display name for the Linux distribution.</param>
    /// <param name="versionId">The distribution's version number.</param>
    /// <param name="versionCodeName">The distribution version codename (if specified).</param>
    /// <param name="homeUrl">The distribution's homepage/website.</param>
    /// <param name="bugReportUrl">The distribution's bug reporting website url (if provided).</param>
    /// <param name="privacyPolicyUrl">The distribution's privacy policy url (if provided).</param>
    /// <param name="supportUrl">The distribution's support website url (if provided).</param>
    public LinuxOsReleaseInfo(string name, string version, string identifier, string[] identifierLike,
        string prettyName, string versionId, string versionCodeName, string? homeUrl = null, string? bugReportUrl = null,
        string? privacyPolicyUrl = null, string? supportUrl = null)
    {
        Name = name;
        Version = version;
        Identifier = identifier;
        IdentifierLike = identifierLike;
        PrettyName = prettyName;
        VersionId = versionId;
        VersionCodename = versionCodeName;
        HomeUrl = homeUrl ?? string.Empty;
        SupportUrl = supportUrl ?? string.Empty;
        BugReportUrl = bugReportUrl ?? string.Empty;
        PrivacyPolicyUrl = privacyPolicyUrl ?? string.Empty;
    }

    /// <summary>
    /// The name of the Linux Distribution.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The distribution display version. 
    /// </summary>
    /// <remarks>This string should not be parsed into a <see cref="Version"/> object as it
    /// may contain a version number and version name. Use <see cref="VersionId"/> instead.</remarks>
    public string Version { get; set; }

    /// <summary>
    /// The linux distribution's identifier.
    /// </summary>
    public string Identifier { get; set; }

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// A list of distribution identifiers that the distribution has self-identified as being based on.
    /// </summary>
    public string[] IdentifierLike { get; set; }

    /// <summary>
    /// The pretty name/display name for the Linux distribution.
    /// </summary>
    public string PrettyName { get; set; }

    /// <summary>
    /// The distribution's version number.
    /// </summary>
    public string VersionId { get; set; }

    /// <summary>
    /// The distribution's homepage/website.
    /// </summary>
    public string HomeUrl { get; set; }
    
    /// <summary>
    /// The distribution's support website url (if provided).
    /// </summary>
    public string SupportUrl { get; set; }
    
    /// <summary>
    /// The distribution's bug reporting website url (if provided).
    /// </summary>
    public string BugReportUrl { get; set; }
    
    /// <summary>
    /// The distribution's privacy policy url (if provided).
    /// </summary>
    public string PrivacyPolicyUrl { get; set; }

    /// <summary>
    /// The distribution version codename (if specified).
    /// </summary>
    public string VersionCodename { get; set; }

    /// <summary>
    /// Determines whether the specified <see cref="LinuxOsReleaseInfo"/> object is equal to the current instance.
    /// </summary>
    /// <param name="other">The <see cref="LinuxOsReleaseInfo"/> instance to compare with the current instance.</param>
    /// <returns>True if the specified instance is equal to the current instance; otherwise, false.</returns>
    public bool Equals(LinuxOsReleaseInfo? other)
    {
        if (other is null) return false;

        if (ReferenceEquals(this, other)) return true;
        
        return Name == other.Name && Version == other.Version && Identifier == other.Identifier &&
               IdentifierLike.Equals(other.IdentifierLike) && PrettyName == other.PrettyName &&
               VersionId == other.VersionId && HomeUrl == other.HomeUrl &&
               SupportUrl == other.SupportUrl && BugReportUrl == other.BugReportUrl &&
               PrivacyPolicyUrl == other.PrivacyPolicyUrl && VersionCodename == other.VersionCodename;
    }

    /// <summary>
    /// Determines whether two instances of <see cref="LinuxOsReleaseInfo"/> are considered equal.
    /// </summary>
    /// <param name="left">The first instance of <see cref="LinuxOsReleaseInfo"/> to compare.</param>
    /// <param name="right">The second instance of <see cref="LinuxOsReleaseInfo"/> to compare.</param>
    /// <returns>Returns <c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
    public static bool Equals(LinuxOsReleaseInfo? left, LinuxOsReleaseInfo? right)
    {
        if (left is null || right is null)
            return false;
        
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current instance of <see cref="LinuxOsReleaseInfo"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>True if the specified object is equal to the current instance; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;

        if (obj is LinuxOsReleaseInfo osReleaseInfo)
            return Equals(osReleaseInfo);
        
        return false;
    }

    /// <summary>
    /// Returns the hash code for the current LinuxOsReleaseInfo object.
    /// </summary>
    /// <returns>An integer representing the hash code of the current LinuxOsReleaseInfo instance.</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Name.GetHashCode();
            hashCode = (hashCode * 397) ^ Version.GetHashCode();
            hashCode = (hashCode * 397) ^ Identifier.GetHashCode();
            hashCode = (hashCode * 397) ^ IdentifierLike.GetHashCode();
            hashCode = (hashCode * 397) ^ PrettyName.GetHashCode();
            hashCode = (hashCode * 397) ^ VersionId.GetHashCode();
            hashCode = (hashCode * 397) ^ HomeUrl.GetHashCode();
            hashCode = (hashCode * 397) ^ SupportUrl.GetHashCode();
            hashCode = (hashCode * 397) ^ BugReportUrl.GetHashCode();
            hashCode = (hashCode * 397) ^ PrivacyPolicyUrl.GetHashCode();
            hashCode = (hashCode * 397) ^ VersionCodename.GetHashCode();
            return hashCode;
        }
    }

    /// <summary>
    /// Determines whether two instances of <see cref="LinuxOsReleaseInfo"/> are equal.
    /// </summary>
    /// <param name="left">The first instance of <see cref="LinuxOsReleaseInfo"/> to compare.</param>
    /// <param name="right">The second instance of <see cref="LinuxOsReleaseInfo"/> to compare.</param>
    /// <returns>Returns <c>true</c> if the two instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(LinuxOsReleaseInfo? left, LinuxOsReleaseInfo? right) => Equals(left, right);

    /// <summary>
    /// Determines whether two instances of <see cref="LinuxOsReleaseInfo"/> are not equal.
    /// </summary>
    /// <param name="left">The first instance of <see cref="LinuxOsReleaseInfo"/> to compare.</param>
    /// <param name="right">The second instance of <see cref="LinuxOsReleaseInfo"/> to compare.</param>
    /// <returns>Returns <c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(LinuxOsReleaseInfo? left, LinuxOsReleaseInfo? right) => !Equals(left, right);
}