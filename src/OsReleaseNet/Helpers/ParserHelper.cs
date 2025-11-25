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

using System.Collections.Generic;
using System.Linq;

namespace AlastairLundy.OsReleaseNet.Helpers;

internal static class ParserHelper
{
    /// <summary>
    /// Removes unwanted characters from an array of strings.
    /// </summary>
    /// <param name="results">The input array containing strings that may contain unwanted characters.</param>
    /// <returns>An array of strings with unwanted characters removed.</returns>
    internal static IEnumerable<string> RemoveUnwantedCharacters(IEnumerable<string> results)
    {
        IEnumerable<string> newResults = results
            .Where(x => !string.IsNullOrWhiteSpace(x) && !x.Equals(string.Empty))
            .Select(x => x.Replace('"'.ToString(), string.Empty));
        
        return newResults;
    }
}