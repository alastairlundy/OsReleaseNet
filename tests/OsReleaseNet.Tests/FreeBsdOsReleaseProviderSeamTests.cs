using System.Reflection;
using OsReleaseNet.Abstractions.Parsers;
using OsReleaseNet.Internal;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace OsReleaseNet.Tests;

/// <summary>
/// Seam-injection tests for <see cref="FreeBsdOsReleaseProvider"/>.
/// </summary>
/// <remarks>
/// The FreeBSD guards throw on every host this suite runs on - no FreeBSD machine exists in the
/// ubuntu-only workflow - so the seam is asserted without executing the guarded reads: the stored
/// seam both read paths consume, the stored default when nothing is injected, and that the guard
/// is consulted before the line source. The branches that run on an actual FreeBSD host exercise
/// consumption of the injected canned text instead.
/// </remarks>
public class FreeBsdOsReleaseProviderSeamTests
{
    private const string CannedFreeBsdOsReleaseText = """
        NAME=FreeBSD
        ID=FreeBSD
        PRETTY_NAME="FreeBSD 14.1-RELEASE"
        """;

    [Test]
    public async Task InjectedLineSource_IsTheSeamBothReadPathsConsume()
    {
        string[] cannedLines = CannedOsRelease.ToLines(CannedFreeBsdOsReleaseText);
        Func<Task<string[]>> injectedLineSource = () => Task.FromResult(cannedLines);

        FreeBsdOsReleaseProvider provider = new(new RecordingFreeBsdOsReleaseParser(),
            injectedLineSource);

        // GetReleaseInfoPropertyValueAsync (the property reader) and GetReleaseInfoAsync (the
        // full-info read) both await this one stored seam; observed through the provider's
        // stored field because running either guarded read throws off-FreeBSD.
        await Assert.That(GetStoredLineSource(provider)).IsSameReferenceAs(injectedLineSource);
    }

    [Test]
    public async Task DefaultLineSource_IsStoredWhenNoLineSourceIsInjected()
    {
        FreeBsdOsReleaseProvider provider = new(new RecordingFreeBsdOsReleaseParser());

        await Assert.That(GetStoredLineSource(provider))
            .IsSameReferenceAs(DistroBaseResolver.DefaultReadOsReleaseLines);
    }

    [Test]
    public async Task FullInfoRead_ConsultsTheGuardBeforeTheLineSource()
    {
        int consultCount = 0;
        string[] cannedLines = CannedOsRelease.ToLines(CannedFreeBsdOsReleaseText);
        Func<Task<string[]>> injectedLineSource = () =>
        {
            consultCount++;
            return Task.FromResult(cannedLines);
        };
        FreeBsdOsReleaseProvider provider = new(new RecordingFreeBsdOsReleaseParser(),
            injectedLineSource);

        if (OperatingSystem.IsFreeBSD())
        {
            FreeBsdOsReleaseInfo info = await provider.GetReleaseInfoAsync();

            await Assert.That(info.Identifier).IsEqualTo("FreeBSD");
            await Assert.That(consultCount).IsEqualTo(1);
        }
        else
        {
#pragma warning disable CA1416 // Deliberately calling a FreeBSD-only member to prove the guard fires first.
            await Assert.That(async () => await provider.GetReleaseInfoAsync())
                .Throws<PlatformNotSupportedException>();
#pragma warning restore CA1416
            await Assert.That(consultCount).IsEqualTo(0);
        }
    }

    [Test]
    public async Task PropertyRead_ConsultsTheGuardBeforeTheLineSource()
    {
#pragma warning disable CS0618 // The obsolete property reader still ships until 3.0.0 - both read paths must be covered.
        int consultCount = 0;
        string[] cannedLines = CannedOsRelease.ToLines(CannedFreeBsdOsReleaseText);
        Func<Task<string[]>> injectedLineSource = () =>
        {
            consultCount++;
            return Task.FromResult(cannedLines);
        };
        FreeBsdOsReleaseProvider provider = new(new RecordingFreeBsdOsReleaseParser(),
            injectedLineSource);

        if (OperatingSystem.IsFreeBSD())
        {
            string? value = await provider.GetReleaseInfoPropertyValueAsync("ID");

            await Assert.That(value).IsEqualTo("FreeBSD");
            await Assert.That(consultCount).IsEqualTo(1);
        }
        else
        {
#pragma warning disable CA1416 // Deliberately calling a FreeBSD-only member to prove the guard fires first.
            await Assert.That(async () => await provider.GetReleaseInfoPropertyValueAsync("ID"))
                .Throws<PlatformNotSupportedException>();
#pragma warning restore CA1416
            await Assert.That(consultCount).IsEqualTo(0);
        }
#pragma warning restore CS0618
    }

    /// <summary>
    /// Observes the provider's stored line-source seam - the private field both read paths
    /// consume - without executing a platform-guarded read.
    /// </summary>
    private static object GetStoredLineSource(FreeBsdOsReleaseProvider provider)
    {
        FieldInfo field = typeof(FreeBsdOsReleaseProvider)
                             .GetField("_readOsReleaseLines", BindingFlags.Instance | BindingFlags.NonPublic)
                         ?? throw new InvalidOperationException(
                             "FreeBsdOsReleaseProvider no longer stores a line-source seam.");

        return field.GetValue(provider) as Func<Task<string[]>>
               ?? throw new InvalidOperationException(
                   "The stored line source is not a Func<Task<string[]>>.");
    }

    /// <summary>Stands in for the production parser, returning a canned FreeBSD release info.</summary>
    private sealed class RecordingFreeBsdOsReleaseParser : IFreeBsdOsReleaseParser
    {
        internal string[]? ReceivedLines { get; private set; }

        public FreeBsdOsReleaseInfo ParseFreeBsdRelease(string[] fileContents)
        {
            ReceivedLines = fileContents;

            return new FreeBsdOsReleaseInfo("FreeBSD", string.Empty, "FreeBSD", "FreeBSD",
                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }
    }
}
