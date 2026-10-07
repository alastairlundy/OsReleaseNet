using System.Reflection;
using OsReleaseNet.Abstractions.Parsers;
using OsReleaseNet.Internal;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace OsReleaseNet.Tests;

/// <summary>
/// Seam-injection tests for <see cref="LinuxOsReleaseProvider"/>: an injected fake line source
/// becomes the provider's stored seam instead of the file-reading default, and canned os-release
/// text flows through that seam without touching the filesystem.
/// </summary>
public class LinuxOsReleaseProviderSeamTests
{
    [Test]
    public async Task InjectedLineSource_IsStoredInsteadOfTheFileReadingDefault()
    {
        string[] cannedLines = CannedOsRelease.ToLines(CannedOsRelease.Arch);
        Func<Task<string[]>> injectedLineSource = () => Task.FromResult(cannedLines);

        LinuxOsReleaseProvider provider = new(new RecordingLinuxOsReleaseParser(), injectedLineSource);

        // The provider holds the injected source, so reads come from the canned text rather
        // than from /etc/os-release.
        await Assert.That(GetStoredLineSource(provider)).IsSameReferenceAs(injectedLineSource);
    }

    [Test]
    public async Task DefaultLineSource_IsStoredWhenNoLineSourceIsInjected()
    {
        LinuxOsReleaseProvider provider = new(new RecordingLinuxOsReleaseParser());

        await Assert.That(GetStoredLineSource(provider))
            .IsSameReferenceAs(DistroBaseResolver.DefaultReadOsReleaseLines);
    }

    [Test]
    public async Task InjectedLineSource_IsConsultedInsteadOfFileReads()
    {
        int consultCount = 0;
        string[] cannedLines = CannedOsRelease.ToLines(CannedOsRelease.Arch);
        Func<Task<string[]>> injectedLineSource = () =>
        {
            consultCount++;
            return Task.FromResult(cannedLines);
        };
        RecordingLinuxOsReleaseParser parser = new();
        LinuxOsReleaseProvider provider = new(parser, injectedLineSource);

        if (OperatingSystem.IsLinux())
        {
            LinuxDistroBase distroBase = await provider.GetDistroBaseAsync();

            // The verdict comes from the canned text (arch), not from the host's real
            // /etc/os-release - a file read would resolve this host's own distro instead.
            await Assert.That(distroBase).IsEqualTo(LinuxDistroBase.Arch);
            await Assert.That(consultCount).IsEqualTo(1);
            await Assert.That(parser.ReceivedLines).IsSameReferenceAs(cannedLines);
        }
        else
        {
            // Off-Linux the platform guard throws before the line source is ever consulted;
            // the guarded read paths themselves are exercised by the integration leg on ubuntu.
#pragma warning disable CA1416 // Deliberately calling a Linux-only member to prove the guard fires first.
            await Assert.That(async () => await provider.GetReleaseInfoAsync())
                .Throws<PlatformNotSupportedException>();
#pragma warning restore CA1416
            await Assert.That(consultCount).IsEqualTo(0);
        }
    }

    /// <summary>
    /// Observes the provider's stored line-source seam - the private field both read paths
    /// consume - without executing a platform-guarded read.
    /// </summary>
    private static object GetStoredLineSource(LinuxOsReleaseProvider provider)
    {
        FieldInfo field = typeof(LinuxOsReleaseProvider)
                             .GetField("_readOsReleaseLines", BindingFlags.Instance | BindingFlags.NonPublic)
                         ?? throw new InvalidOperationException(
                             "LinuxOsReleaseProvider no longer stores a line-source seam.");

        return field.GetValue(provider) as Func<Task<string[]>>
               ?? throw new InvalidOperationException(
                   "The stored line source is not a Func<Task<string[]>>.");
    }

    /// <summary>Stands in for the production parser, recording the lines the seam delivered.</summary>
    private sealed class RecordingLinuxOsReleaseParser : ILinuxOsReleaseParser
    {
        internal string[]? ReceivedLines { get; private set; }

        public LinuxOsReleaseInfo ParseLinuxOsRelease(string[] fileContents)
        {
            ReceivedLines = fileContents;

            return CannedOsRelease.ToLinuxOsReleaseInfo(fileContents);
        }
    }
}
