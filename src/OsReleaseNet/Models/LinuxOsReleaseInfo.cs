/*
    OsReleaseNet
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

// ReSharper disable once CheckNamespace
namespace AlastairLundy.OsReleaseNet;

/// <summary>
/// Represents a Linux Distribution's OsRelease file and information contained therein.
/// </summary>
/// <remarks>All trademarks mentioned belong to their respective owners.</remarks>
public class LinuxOsReleaseInfo
{
    
    /// <summary>
    /// 
    /// </summary>
    internal LinuxOsReleaseInfo()
    {
        VersionId = string.Empty;
        Name = string.Empty;
        Identifier = string.Empty;
        Identifier_Like = string.Empty;
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
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="version"></param>
    /// <param name="identifier"></param>
    /// <param name="identifierLike"></param>
    /// <param name="prettyName"></param>
    /// <param name="versionId"></param>
    /// <param name="versionCodeName"></param>
    /// <param name="homeUrl"></param>
    /// <param name="bugReportUrl"></param>
    /// <param name="privacyPolicyUrl"></param>
    /// <param name="supportUrl"></param>
    public LinuxOsReleaseInfo(string name, string version, string identifier, string identifierLike, string prettyName, string versionId, string versionCodeName, string homeUrl, string bugReportUrl, string privacyPolicyUrl, string supportUrl)
    {
        Name = name;
        Version = version;
        Identifier = identifier;
        Identifier_Like = identifierLike;
        PrettyName = prettyName;
        VersionId = versionId;
        VersionCodename = versionCodeName;
        HomeUrl = homeUrl;
        SupportUrl = supportUrl;
        BugReportUrl = bugReportUrl;
        PrivacyPolicyUrl = privacyPolicyUrl;
    }

    /// <summary>
    /// The name of the Linux Distribution.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The distribution display version. 
    /// </summary>
    /// <remarks>This string should not be parsed into a <see cref="Version"/> object as it may contain a version number and version name. Use <see cref="VersionId"/> instead.</remarks>
    public string Version { get; set; }

    /// <summary>
    /// The linux distribution's identifier.
    /// </summary>
    public string Identifier { get; set; }

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// A list of distribution identifiers that the distribution has self identified as being based on.
    /// </summary>
    public string Identifier_Like { get; set; }

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
}