# OsReleaseNet
Linux and FreeBSD OsRelease detection, SteamOS detection, and helpful detection code for Linux.

[![NuGet](https://img.shields.io/nuget/v/OsReleaseNet.svg)](https://www.nuget.org/packages/OsReleaseNet/)  [![NuGet](https://img.shields.io/nuget/dt/OsReleaseNet.svg)](https://www.nuget.org/packages/OsReleaseNet/)

## Table of Contents
* [Features](#features)
* [Installing](#how-to-install-and-use-osreleasenet)
    * [Compatibility](#compatibility)
* [Contributing](#how-to-contribute)
* [Roadmap](#roadmap)

## Features
* Support for detecting Linux OsRelease information
* Support for detecting SteamOS and SteamDeck information

## How to install and use OsReleaseNet
OsReleaseNet can be installed via the .NET SDK CLI, Nuget via your IDE or code editor's package interface, or via the Nuget website.

| Package Name                                | Nuget Link                                                                                                                  | .NET SDK CLI command                                               |
|---------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------|
| OsReleaseNet                                | [OsReleaseNet Nuget](https://nuget.org/packages/OsReleaseNet)                                                               | ``dotnet add package OsReleaseNet``                                |
| OsReleaseNet.Extensions.DependencyInjection | [OsReleaseNet.Extensions.DependencyInjection Nuget](https://nuget.org/packages/OsReleaseNet.Extensions.DependencyInjection) | ``dotnet add package OsReleaseNet.Extensions.DependencyInjection`` |


### Compatibility
OsReleaseNet supports:
* .NET Standard 2.0
* .NET 8
* .NET 9
* .NET 10

## How to build the code
To build the project in Debug mode, run the following command: ``dotnet build -c Debug``.

To build the project in Release mode, run the following command: ``dotnet build -C Release``

## Roadmap
OsReleaseNet aims to make working with different types in the System namespace in C# easier.

All stable releases must be stable and should not contain regressions.

Future updates should focus on one or more of the following:
* Improving or adding Linux detection code
* Possibly adding support for other Unix or BSD-based operating systems

## How to contribute
If you'd like to contribute to the code, please fork the project, make your changes, and then open a Pull Request.

## Credits/ Acknowledgements

### Projects
This project would like to thank the following projects for their work:
* [Polyfill](https://github.com/SimonCropp/Polyfill) for simplifying .NET Standard 2.0 support

For more information, please see the [THIRD_PARTY_NOTICES file](https://codeberg.org/alastairlundy/OsReleaseNet/blob/main/THIRD_PARTY_NOTICES.txt).
