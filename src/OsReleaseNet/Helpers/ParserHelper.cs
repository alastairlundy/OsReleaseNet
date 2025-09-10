using System.Collections.Generic;
using System.Linq;

namespace AlastairLundy.OsReleaseNet.Helpers;

internal static class ParserHelper
{
    /// <summary>
    /// Removes unwanted characters from an array of strings.
    /// </summary>
    /// <param name="resultArray">The input array containing strings that may contain unwanted characters.</param>
    /// <returns>An array of strings with unwanted characters removed.</returns>
    internal static IEnumerable<string> RemoveUnwantedCharacters(IEnumerable<string> resultArray)
    {
        IEnumerable<string> newResults = resultArray
            .Where(x => string.IsNullOrWhiteSpace(x) == false && x.Equals(string.Empty) == false)
            .Select(x => x.Replace('"'.ToString(), string.Empty));
        
        return newResults;
    }
}