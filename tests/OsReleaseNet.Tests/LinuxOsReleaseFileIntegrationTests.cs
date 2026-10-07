using OsReleaseNet.Internal;
using OsReleaseNet.Parsers;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace OsReleaseNet.Tests;

/// <summary>
/// The real-file integration leg: reads the actual <c>/etc/os-release</c> through the Linux
/// provider's platform-guarded paths.
/// </summary>
/// <remarks>
/// Tagged <c>Integration</c> and marked explicit, so the default local run excludes it. The CI
/// workflow's integration job selects it on ubuntu with
/// <c>--treenode-filter "/*/*/*/*[Category=Integration]"</c>; the unit jobs exclude the tag.
/// </remarks>
[Category("Integration")]
[Explicit]
public class LinuxOsReleaseFileIntegrationTests
{
    [Test]
    public async Task RealOsReleaseFile_IsReadParsedAndResolvedThroughTheLinuxProvider()
    {
        if (OperatingSystem.IsLinux())
        {
            LinuxOsReleaseProvider provider = new(new LinuxOsReleaseParser());

            LinuxOsReleaseInfo releaseInfo = await provider.GetReleaseInfoAsync();
            LinuxDistroBase distroBase = await provider.GetDistroBaseAsync();

            await Assert.That(releaseInfo.Identifier).IsNotEmpty();

            // One authority for the verdict: the provider's answer equals the core's answer
            // resolved from the same parsed info.
            await Assert.That(distroBase).IsEqualTo(DistroBaseResolver.Resolve(releaseInfo));
        }
        else
        {
            Skip.Test("Requires Linux with a real /etc/os-release file.");
        }
    }
}
