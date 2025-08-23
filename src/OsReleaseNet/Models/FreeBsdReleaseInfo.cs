namespace AlastairLundy.OsReleaseNet;

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
        IsLongTermSupportRelease = false;
    }
    
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
        IsLongTermSupportRelease = VersionId.ToLower().Contains("lts");
    }
    
    public bool IsLongTermSupportRelease { get; set; }

    public string AnsiColor { get; set; }

    public string CpeName { get; set; }
    
    public string Name { get; set; }

    public string Version { get; set; }
    
    public string Identifier { get; set; }

    public string PrettyName { get; set; }

    public string VersionId { get; set; }

    public string HomeUrl { get; set; }
    public string BugReportUrl { get; set; }
}