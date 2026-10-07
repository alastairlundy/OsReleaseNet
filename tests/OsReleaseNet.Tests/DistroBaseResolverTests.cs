using OsReleaseNet.Internal;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace OsReleaseNet.Tests;

/// <summary>
/// Canned os-release texts and the test-local conversion the unit suite drives the distro-base
/// core with.
/// </summary>
/// <remarks>
/// The production parser is platform-guarded and throws off-Linux, so canned text is converted
/// to <see cref="LinuxOsReleaseInfo"/> here - on every operating system. The real parser and the
/// real file are exercised by the tagged integration leg on ubuntu.
/// </remarks>
internal static class CannedOsRelease
{
    internal const string Arch = "ID=arch";

    internal const string LinuxMintWithMappableIdLike = "ID=linuxmint\nID_LIKE=ubuntu debian";

    internal const string DebianWithTwoMappableIdLikeEntries = "ID=debian\nID_LIKE=arch manjaro";

    internal const string UbuntuWithLaterMappableIdLikeEntry = "ID=ubuntu\nID_LIKE=notarealdistro debian";

    internal const string UbuntuWithUnmappableIdLike = "ID=ubuntu\nID_LIKE=notarealdistro";

    internal const string CentosWithEmptyIdLike = "ID=centos\nID_LIKE=";

    internal const string SuseWithWhitespaceIdLike = "ID=suse\nID_LIKE=   notarealdistro";

    internal const string WeirdosWithUnmappableIdLike = "ID=weirdos\nID_LIKE=alsonotreal";

    /// <summary>Splits canned os-release text into the raw lines a line source would return.</summary>
    internal static string[] ToLines(string osReleaseText) =>
        osReleaseText.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

    /// <summary>Converts canned os-release text into the parsed info the distro-base core consumes.</summary>
    internal static LinuxOsReleaseInfo ToLinuxOsReleaseInfo(string osReleaseText) =>
        ToLinuxOsReleaseInfo(ToLines(osReleaseText));

    /// <summary>Converts raw os-release lines into the parsed info the distro-base core consumes.</summary>
    internal static LinuxOsReleaseInfo ToLinuxOsReleaseInfo(string[] lines)
    {
        string identifier = string.Empty;
        string[] identifierLike = [];
        string name = string.Empty;
        string prettyName = string.Empty;

        foreach (string line in lines)
        {
            if (line.StartsWith("ID_LIKE=", StringComparison.Ordinal))
            {
                identifierLike = line["ID_LIKE=".Length..].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            }
            else if (line.StartsWith("ID=", StringComparison.Ordinal))
            {
                identifier = line["ID=".Length..];
            }
            else if (line.StartsWith("NAME=", StringComparison.Ordinal))
            {
                name = line["NAME=".Length..];
            }
            else if (line.StartsWith("PRETTY_NAME=", StringComparison.Ordinal))
            {
                prettyName = line["PRETTY_NAME=".Length..];
            }
        }

        return new LinuxOsReleaseInfo(name, string.Empty, identifier, identifierLike, prettyName,
            string.Empty, string.Empty);
    }
}

/// <summary>
/// Direct unit coverage for the internal <c>DistroBaseResolver</c> core, driven by canned
/// os-release texts: the full identifier mapping table, <see cref="LinuxDistroBase.NotDetected"/>,
/// and the D002 fallback order.
/// </summary>
public class DistroBaseResolverTests
{
    [Test]
    [Arguments("ID=debian", LinuxDistroBase.Debian)]
    [Arguments("ID=ubuntu", LinuxDistroBase.Ubuntu)]
    [Arguments("ID=arch", LinuxDistroBase.Arch)]
    [Arguments("ID=manjaro", LinuxDistroBase.Manjaro)]
    [Arguments("ID=fedora", LinuxDistroBase.Fedora)]
    [Arguments("ID=rhel", LinuxDistroBase.RHEL)]
    [Arguments("ID=oracle", LinuxDistroBase.RHEL)]
    [Arguments("ID=centos", LinuxDistroBase.RHEL)]
    [Arguments("ID=suse", LinuxDistroBase.SUSE)]
    public async Task Resolve_MapsEveryKnownIdentifier(string cannedOsReleaseText, LinuxDistroBase expected)
    {
        LinuxOsReleaseInfo info = CannedOsRelease.ToLinuxOsReleaseInfo(cannedOsReleaseText);

        await Assert.That(DistroBaseResolver.Resolve(info)).IsEqualTo(expected);
    }

    [Test]
    [Arguments("ID=weirdos")]
    [Arguments("ID_LIKE=notarealdistro")]
    [Arguments("NAME=Anonymous Distro")]
    [Arguments(CannedOsRelease.WeirdosWithUnmappableIdLike)]
    public async Task Resolve_ReturnsNotDetected_WhenNoIdentifierMaps(string cannedOsReleaseText)
    {
        LinuxOsReleaseInfo info = CannedOsRelease.ToLinuxOsReleaseInfo(cannedOsReleaseText);

        await Assert.That(DistroBaseResolver.Resolve(info)).IsEqualTo(LinuxDistroBase.NotDetected);
    }

    [Test]
    [Arguments(CannedOsRelease.LinuxMintWithMappableIdLike, LinuxDistroBase.Ubuntu)]
    [Arguments(CannedOsRelease.DebianWithTwoMappableIdLikeEntries, LinuxDistroBase.Arch)]
    [Arguments(CannedOsRelease.UbuntuWithLaterMappableIdLikeEntry, LinuxDistroBase.Debian)]
    public async Task Resolve_ChecksIdentifierLikeBeforeIdentifier(string cannedOsReleaseText,
        LinuxDistroBase expected)
    {
        LinuxOsReleaseInfo info = CannedOsRelease.ToLinuxOsReleaseInfo(cannedOsReleaseText);

        await Assert.That(DistroBaseResolver.Resolve(info)).IsEqualTo(expected);
    }

    [Test]
    [Arguments(CannedOsRelease.UbuntuWithUnmappableIdLike, LinuxDistroBase.Ubuntu)]
    [Arguments(CannedOsRelease.CentosWithEmptyIdLike, LinuxDistroBase.RHEL)]
    [Arguments(CannedOsRelease.SuseWithWhitespaceIdLike, LinuxDistroBase.SUSE)]
    public async Task Resolve_FallsBackToIdentifier_WhenIdentifierLikeIsPresentButUnmappable(
        string cannedOsReleaseText, LinuxDistroBase expected)
    {
        // The deliberate behavior change versus the old asynchronous fast path: a present-but-
        // unmappable ID_LIKE falls back to Identifier instead of returning NotDetected.
        LinuxOsReleaseInfo info = CannedOsRelease.ToLinuxOsReleaseInfo(cannedOsReleaseText);

        await Assert.That(DistroBaseResolver.Resolve(info)).IsEqualTo(expected);
    }

    [Test]
    [Arguments("ID=DeBian", LinuxDistroBase.Debian)]
    [Arguments("ID=ARCH", LinuxDistroBase.Arch)]
    [Arguments("ID= debian", LinuxDistroBase.Debian)]
    public async Task Resolve_NormalizesIdentifierCasingAndWhitespace(string cannedOsReleaseText,
        LinuxDistroBase expected)
    {
        LinuxOsReleaseInfo info = CannedOsRelease.ToLinuxOsReleaseInfo(cannedOsReleaseText);

        await Assert.That(DistroBaseResolver.Resolve(info)).IsEqualTo(expected);
    }

    [Test]
    public async Task Resolve_ThrowsWhenOsReleaseInfoIsNull()
    {
        await Assert.That(() => DistroBaseResolver.Resolve(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task DefaultOsReleaseFilePath_IsEtcOsRelease()
    {
        await Assert.That(DistroBaseResolver.DefaultOsReleaseFilePath).IsEqualTo("/etc/os-release");
    }
}
