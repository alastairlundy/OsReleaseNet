/*
    OsReleaseNet
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Linq;
using System.Threading.Tasks;

using AlastairLundy.OsReleaseNet.Abstractions;
using AlastairLundy.OsReleaseNet.Abstractions.Parsers;
using AlastairLundy.OsReleaseNet.Helpers;
using AlastairLundy.OsReleaseNet.Internal.Localizations;

using System.Runtime.Versioning;

#if NETSTANDARD2_0
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
using File = Polyfills.FilePolyfill;
#else
using System.IO;
#endif

namespace AlastairLundy.OsReleaseNet;

/// <summary>
/// A class that provides information about the current FreeBSD operating system.
/// </summary>
public class FreeBsdOsReleaseProvider : IFreeBsdOsReleaseProvider
{
    private readonly IFreeBsdOsReleaseParser _freeBsdOsReleaseParser;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="freeBsdOsReleaseParser"></param>
    public FreeBsdOsReleaseProvider(IFreeBsdOsReleaseParser freeBsdOsReleaseParser)
    {
        _freeBsdOsReleaseParser = freeBsdOsReleaseParser;
    }

    /// <summary>
    /// Retrieves the value of a specific property in the FreeBSD OS Release Info.
    /// </summary>
    /// <param name="propertyName">The name of the property to retrieve.</param>
    /// <returns>The value of the specified property if found; a null string otherwise.</returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System
    /// that isn't FreeBSD-based.</exception>
    [SupportedOSPlatform("freebsd")]
    public async Task<string?> GetReleaseInfoPropertyValueAsync(string propertyName)
    {
        if (OperatingSystem.IsFreeBSD() == false)
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);
            
        string[] resultArray = await File.ReadAllLinesAsync("/etc/os-release");
        
        string? result = ParserHelper.RemoveUnwantedCharacters(resultArray)
            .FirstOrDefault(x => x.ToUpper().Contains(propertyName.ToUpper()));

        if (result is not null)
        {
            result = result.Replace(propertyName, string.Empty)
                .Replace("=", string.Empty);
        }

        return result;
    }

    /// <summary>
    /// Retrieves the FreeBSD OS release information.
    /// </summary>
    /// <returns>The FreeBSD OS release information.</returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System
    /// that isn't FreeBSD-based.</exception>
    [SupportedOSPlatform("freebsd")]
    public async Task<FreeBsdOsReleaseInfo> GetReleaseInfoAsync()
    {
        if (OperatingSystem.IsFreeBSD() == false)
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);

        string[] resultArray = await File.ReadAllLinesAsync("/etc/os-release");
        
        return await Task.FromResult(_freeBsdOsReleaseParser.ParseFreeBsdRelease(resultArray));
    }
}