using System.Collections.Generic;
using AlastairLundy.DotExtensions.Strings;

namespace AlastairLundy.OsReleaseNet.Helpers;

internal class OsReleaseParser
{
    
    /// <summary>
    /// Parses an array of strings to extract the Linux OS release information.
    /// </summary>
    /// <param name="results">The input array containing strings that need to be parsed for OS release information.</param>
    /// <returns>The extracted Linux OS release information.</returns>
    internal LinuxOsReleaseInfo ParseOsReleaseInfo(IEnumerable<string> results)
    {
        LinuxOsReleaseInfo linuxDistroInfo = new LinuxOsReleaseInfo();
        
        foreach (string line in results)
        {
            string lineUpper = line.ToUpper();

            if (lineUpper.Contains("NAME=") && !lineUpper.Contains("VERSION"))
            {
                if (lineUpper.StartsWith("PRETTY_"))
                {
                    linuxDistroInfo.PrettyName =
                        line.Replace("PRETTY_NAME=", string.Empty);
                }

                if (!lineUpper.Contains("PRETTY") && !lineUpper.Contains("CODE"))
                {
                    linuxDistroInfo.Name = line.Replace("NAME=", string.Empty);
                }
            }

            if (lineUpper.Contains("VERSION="))
            {

                if (lineUpper.Contains("ID="))
                {
                    linuxDistroInfo.VersionId =
                        line.Replace("VERSION_ID=", string.Empty);
                }
                else if (!lineUpper.Contains("ID=") && lineUpper.Contains("CODE"))
                {
                    linuxDistroInfo.VersionCodename =
                        line.Replace("VERSION_CODENAME=", string.Empty);
                }
                else if (!lineUpper.Contains("ID=") && !lineUpper.Contains("CODE"))
                {
                    linuxDistroInfo.Version = line.Replace("VERSION=", string.Empty);
                }
            }

            if (lineUpper.Contains("ID"))
            {
                if (lineUpper.Contains("ID_LIKE="))
                {
                    string identifiers = line.Replace("ID_LIKE=", string.Empty);

                    if (identifiers.ContainsSpaceSeparatedSubStrings())
                    {
                        linuxDistroInfo.IdentifierLike = identifiers.Split(" ");
                    }
                    else
                    {
                        linuxDistroInfo.IdentifierLike = [line];
                    }
                }
                else if (!lineUpper.Contains("VERSION"))
                {
                    linuxDistroInfo.Identifier = line.Replace("ID=", string.Empty);
                }
            }

            if (lineUpper.Contains("URL="))
            {
                if (lineUpper.StartsWith("HOME_"))
                {
                    linuxDistroInfo.HomeUrl = line.Replace("HOME_URL=", string.Empty);
                }
                else if (lineUpper.StartsWith("SUPPORT_"))
                {
                    linuxDistroInfo.SupportUrl =
                        line.Replace("SUPPORT_URL=", string.Empty);
                }
                else if (lineUpper.StartsWith("BUG_"))
                {
                    linuxDistroInfo.BugReportUrl =
                        line.Replace("BUG_REPORT_URL=", string.Empty);
                }
                else if (lineUpper.StartsWith("PRIVACY_"))
                {
                    linuxDistroInfo.PrivacyPolicyUrl =
                        line.Replace("PRIVACY_POLICY_URL=", string.Empty);
                }
            }
        }

        return linuxDistroInfo;
    }
}