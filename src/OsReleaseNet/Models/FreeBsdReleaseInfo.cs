/*
    OsReleaseNet
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

namespace AlastairLundy.OsReleaseNet;

/// <summary>
/// Represents a FreeBSD Distribution's OsRelease file and information contained therein.
/// </summary>
/// <remarks>All trademarks mentioned belong to their respective owners.</remarks>
public class FreeBsdReleaseInfo
{
    internal FreeBsdReleaseInfo()
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
    public FreeBsdReleaseInfo(string name, string version, string identifier, string prettyName, string versionId, string homeUrl, string bugReportUrl, string ansiColor, string cpeName)
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
    
    public string AnsiColor { get; set; }

    public string CpeName { get; set; }
    
    /// <summary>
    /// The name of the FreeBSD Distribution.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The FreeBSD distribution display version. 
    /// </summary>
    /// <remarks>This string should not be parsed into a <see cref="Version"/> object as it may contain a version number and version name. Use <see cref="VersionId"/> instead.</remarks>
    public string Version { get; set; }

    /// <summary>
    /// The FreeBSD distribution's identifier.
    /// </summary>
    public string Identifier { get; set; }

    /// <summary>
    /// The pretty name/display name for the FreeBSD distribution.
    /// </summary>
    public string PrettyName { get; set; }

    /// <summary>
    /// The FreeBSD distribution's version number.
    /// </summary>
    public string VersionId { get; set; }

    /// <summary>
    /// The FreeBSD distribution's homepage/website.
    /// </summary>
    public string HomeUrl { get; set; }
    
    /// <summary>
    /// The FreeBSD distribution's bug reporting website url (if provided).
    /// </summary>
    public string BugReportUrl { get; set; }
}