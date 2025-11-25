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
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AlastairLundy.OsReleaseNet;

/// <summary>
/// A class that provides information about the current Linux OS Release.
/// </summary>
public class LinuxOsReleaseProvider : ILinuxOsReleaseProvider
{
    private readonly ILinuxOsReleaseParser _linuxOsReleaseParser;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="linuxOsReleaseParser"></param>
    public LinuxOsReleaseProvider(ILinuxOsReleaseParser linuxOsReleaseParser)
    {
        _linuxOsReleaseParser = linuxOsReleaseParser;
    }

    /// <summary>
    /// Retrieves the value of a specific property in the Linux OS Release Info.
    /// </summary>
    /// <param name="propertyName">The name of the property to retrieve.</param>
    /// <returns>The value of the specified property if found; a null string otherwise.</returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System
    /// that isn't Linux-based.</exception>
    [SupportedOSPlatform("linux")]
    public async Task<string?> GetReleaseInfoPropertyValueAsync(string propertyName)
    {
        ArgumentException.ThrowIfNullOrEmpty(propertyName);
        
        if (!OperatingSystem.IsLinux())
            throw new PlatformNotSupportedException(Resources.
                Exceptions_PlatformNotSupported_LinuxOnly);
            
        string[] resultArray = await File.ReadAllLinesAsync("/etc/os-release");
        
        string? result = ParserHelper.RemoveUnwantedCharacters(resultArray)
        .FirstOrDefault(x => x.ToUpper().Contains(propertyName.ToUpper()));

        result = result?.Replace(propertyName, string.Empty)
            .Replace("=", string.Empty);

        return result;
    }

    /// <summary>
    /// Retrieves the Linux OS release information.
    /// </summary>
    /// <returns>The Linux OS release information.</returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System
    /// that isn't Linux-based.</exception>
    [SupportedOSPlatform("linux")]
    public async Task<LinuxOsReleaseInfo> GetReleaseInfoAsync()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            throw new PlatformNotSupportedException(
                Resources.Exceptions_PlatformNotSupported_LinuxOnly);

        string[] resultArray = await File.ReadAllLinesAsync("/etc/os-release");

        LinuxOsReleaseInfo result = _linuxOsReleaseParser.ParseLinuxOsRelease(resultArray);

        return await Task.FromResult(result);
    }

    /// <summary>
    /// Retrieves the base Linux distribution information.
    /// </summary>
    /// <returns>The base Linux distribution information.</returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System
    /// that isn't Linux-based.</exception>
    [SupportedOSPlatform("linux")]    
    public async Task<LinuxDistroBase> GetDistroBaseAsync()
    {
        if (!OperatingSystem.IsLinux())
            throw new PlatformNotSupportedException(Resources.
                Exceptions_PlatformNotSupported_LinuxOnly);

        LinuxOsReleaseInfo osReleaseInfo = new LinuxOsReleaseInfo();
        
        string? result =  await GetReleaseInfoPropertyValueAsync("ID_LIKE=");

        if (result is not null)
            osReleaseInfo.IdentifierLike = result.Split(" ");
        else
            osReleaseInfo = await GetReleaseInfoAsync();
        
        return GetDistroBase(osReleaseInfo);
    }

    /// <summary>
    /// Compares the Linux Os Release Identifier information to determine what distro it is based on and returns it as a LinuxDistroBase enum.
    /// </summary>
    /// <remarks>This method is, by design, not asynchronous. It performs no asynchronous operations.
    /// <para>Whilst this method does not throw an Exception if it is not run on Linux, there is no way to provide valid LinuxOsRelease from a  </para></remarks>
    /// <param name="osReleaseInfo">The LinuxOsReleaseInfo object to parse.</param>
    /// <returns>The detected LinuxDistroBase as an enum if successfully detected,
    /// the LinuxDistroBase.NotDetected enum value otherwise.</returns>
    [SupportedOSPlatform("linux")]
    public LinuxDistroBase GetDistroBase(LinuxOsReleaseInfo osReleaseInfo)
    {
        string identifierLike = osReleaseInfo.IdentifierLike.First().ToLower();
        
        return identifierLike switch
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