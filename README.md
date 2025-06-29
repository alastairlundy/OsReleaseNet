# OsReleaseNet (formerly PlatformKit.Linux)
Linux OsRelease detection, SteamOS detection, and helpful detection code for Linux.

[OsReleaseNet](https://www.nuget.org/packages/AlastairLundy.OsReleaseNet/) [![NuGet](https://img.shields.io/nuget/v/AlastairLundy.OsReleaseNet.svg)](https://www.nuget.org/packages/AlastairLundy.OsReleaseNet/)  [![NuGet](https://img.shields.io/nuget/dt/AlastairLundy.OsReleaseNet.svg)](https://www.nuget.org/packages/AlastairLundy.OsReleaseNet/)

## Table of Contents
* [Features](#features)
* [Installing](#how-to-install-and-use-osreleasenet)
    * [Compatibility](#compatibility)
* [Contributing](#how-to-contribute)
* [Roadmap](#roadmap)
* [License](#license)

## Features
* Support for detecting Linux OsRelease information
* Support for detecting SteamOS and SteamDeck information


## How to install and use OsReleaseNet
OsReleaseNet can be installed via the .NET SDK CLI, Nuget via your IDE or code editor's package interface, or via the Nuget website.

| Package Name                | Nuget Link                                                                                  | .NET SDK CLI command                               |
|-----------------------------|---------------------------------------------------------------------------------------------|----------------------------------------------------|
| AlastairLundy.OsReleaseNet | [AlastairLundy.OsReleaseNet Nuget](https://nuget.org/packages/AlastairLundy.OsReleaseNet) | ``dotnet add package AlastairLundy.OsReleaseNet`` |

### Compatibility
OsReleaseNet supports:
* .NET 8
* .NET 9
* .NET Standard 2.0
* .NET Standard 2.1

## How to build the code
To build the project in Debug mode run the following command: ``dotnet build -c Debug``.

To build the project in Release mode run the following command: ``dotnet build -C Release``

## Roadmap
OsReleaseNet aims to make working with different types in the System namespace in C# easier.

All stable releases must be stable and should not contain regressions.

Future updates should aim focus on one or more of the following:
* Improving or adding Linux detection code
* Possibly adding support for FreeBSD and/or other BSD operating systems

## How to contribute to the project
If you'd like to contribute to the code, please fork the project, make your changes, and then open a Pull Request.

## Credits/ Acknowledgements

### Projects
This project would like to thank the following projects for their work:
* [Polyfill](https://github.com/SimonCropp/Polyfill) for simplifying .NET Standard 2.0 & 2.1 support

For more information, please see the [THIRD_PARTY_NOTICES file](https://github.com/alastairlundy/OsReleaseNet/blob/main/THIRD_PARTY_NOTICES.txt).
