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
/// Represents a FreeBSD Distribution's OsRelease file and information contained therein.
/// </summary>
/// <remarks>All trademarks mentioned belong to their respective owners.</remarks>
public class FreeBsdOsReleaseInfo
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
    public string AnsiColor { get; set; }

    /// <summary>
    /// The Common Platform Enumeration (CPE) name of the FreeBSD Distribution,
    /// representing a standardized method of identifying and describing software or operating systems.
    /// </summary>
    public string CpeName { get; set; }
    
    /// <summary>
    /// The name of the FreeBSD Distribution.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The FreeBSD distribution display version. 
    /// </summary>
    /// <remarks>This string should not be parsed into a <see cref="Version"/> object
    /// as it may contain a version number and version name. Use <see cref="VersionId"/> instead.</remarks>
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