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

using System.Linq;

namespace AlastairLundy.OsReleaseNet.Parsers;

/// <summary>
/// A class to parse the contents of a FreeBSD OsRelease file.
/// </summary>
public class FreeBsdOsReleaseParser : IFreeBsdOsReleaseParser
{
    
    /// <summary>
    /// Parses the contents of a FreeBSD OsRelease file.
    /// </summary>
    /// <param name="fileContents">The FreeBSD OsRelease file contents.</param>
    /// <returns>The parsed FreeBSD OsRelease file contents as a FreeBsdReleaseInfo object.</returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if run on an operating system that is not FreeBSD based.</exception>
    /// <exception cref="ArgumentException">Thrown if the string array provided hasn't come from an os-release file.</exception>
    [SupportedOSPlatform("freebsd")]
    public FreeBsdOsReleaseInfo ParseFreeBsdRelease(string[] fileContents)
    {
        if (OperatingSystem.IsFreeBSD() == false)
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);

        if (fileContents.Any(x => x.ToLower().Contains("id=")) == false)
            throw new ArgumentException(Resources.Exceptions_Arguments_NotOsReleaseContents);
        
        FreeBsdOsReleaseInfo freeBsdReleaseInfo = new FreeBsdOsReleaseInfo();
            
        fileContents = ParserHelper.RemoveUnwantedCharacters(fileContents).ToArray();
        
        foreach (string resultLine in fileContents)
        {
            string line = resultLine.ToUpper();

            if(line.Contains("ANSI_"))
            {
                if (line.StartsWith("ANSI_COLOR="))
                {
                    freeBsdReleaseInfo.AnsiColor = resultLine.Replace("ANSI_COLOR=", string.Empty);
                }   
            }
            if (line.Contains("NAME=") && !line.Contains("VERSION"))
            {

                if (line.StartsWith("CPE_"))
                {
                    freeBsdReleaseInfo.CpeName = resultLine.Replace("CPE_NAME=", string.Empty);
                }
                if (line.StartsWith("PRETTY_"))
                {
                    freeBsdReleaseInfo.PrettyName =
                        resultLine.Replace("PRETTY_NAME=", string.Empty);
                }

                if (!line.Contains("PRETTY") && !line.Contains("CODE"))
                {
                    freeBsdReleaseInfo.Name = resultLine
                        .Replace("NAME=", string.Empty);
                }
            }

            if (line.Contains("VERSION="))
            {

                if (line.Contains("ID="))
                {
                    freeBsdReleaseInfo.VersionId =
                        resultLine.Replace("VERSION_ID=", string.Empty);
                }
                else if (!line.Contains("ID=") && !line.Contains("CODE"))
                {
                    freeBsdReleaseInfo.Version = resultLine.Replace("VERSION=", string.Empty)
                        .Replace("LTS", string.Empty);
                }
            }

            if (line.Contains("ID"))
            {
                if (!line.Contains("VERSION"))
                {
                    freeBsdReleaseInfo.Identifier = resultLine.Replace("ID=", string.Empty);
                }
            }

            if (line.Contains("URL="))
            {
                if (line.StartsWith("HOME_"))
                {
                    freeBsdReleaseInfo.HomeUrl = resultLine.Replace("HOME_URL=", string.Empty);
                }
                else if (line.StartsWith("BUG_"))
                {
                    freeBsdReleaseInfo.BugReportUrl =
                        resultLine.Replace("BUG_REPORT_URL=", string.Empty);
                }
            }
        }

        return freeBsdReleaseInfo;
    }
}