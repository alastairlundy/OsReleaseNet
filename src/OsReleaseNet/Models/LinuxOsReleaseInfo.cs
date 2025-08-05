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
        IsLongTermSupportRelease = false;
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
        IsLongTermSupportRelease = VersionId.ToLower().Contains("lts");
    }
    
    /// <summary>
    /// 
    /// </summary>
    public bool IsLongTermSupportRelease { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Identifier { get; set; }

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// 
    /// </summary>
    public string Identifier_Like { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string PrettyName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string VersionId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string HomeUrl { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string SupportUrl { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string BugReportUrl { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string PrivacyPolicyUrl { get; set; }

    ///   
    public string VersionCodename { get; set; }
}