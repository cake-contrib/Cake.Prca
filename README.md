# Cake Pull Request Code Analysis Addin

This Addin for the Cake Build Automation System allows you to write issues found using any code analyzer or linter as comments to pull requests.
More about Cake at http://cakebuild.net

[![License](http://img.shields.io/:license-mit-blue.svg)](https://github.com/cake-contrib/Cake.Prca/blob/feature/build/LICENSE)

## Information

| | Stable | Pre-release |
|:--:|:--:|:--:|
|GitHub Release|-|[![GitHub release](https://img.shields.io/github/release/cake-contrib/Cake.Prca.svg)](https://github.com/cake-contrib/Cake.Prca/releases/latest)|
|NuGet|[![NuGet](https://img.shields.io/nuget/v/Cake.Prca.svg)](https://www.nuget.org/packages/Cake.Prca)|[![NuGet](https://img.shields.io/nuget/vpre/Cake.Prca.svg)](https://www.nuget.org/packages/Cake.Prca)|

## Build Status

|Develop|Master|
|:--:|:--:|
|[![Build status](https://ci.appveyor.com/api/projects/status/xxx/branch/develop?svg=true)](https://ci.appveyor.com/project/cakecontrib/cake-prca/branch/develop)|[![Build status](https://ci.appveyor.com/api/projects/status/xxx/branch/develop?svg=true)](https://ci.appveyor.com/project/cakecontrib/cake-prca/branch/master)|

## Code Coverage

[![Coverage Status](https://coveralls.io/repos/github/cake-contrib/Cake.Prca/badge.svg?branch=develop)](https://coveralls.io/github/cake-contrib/Cake.Prca?branch=develop)

## Quick Links

- [Documentation](https://cake-contrib.github.io/Cake.Prca)

## Chat Room

Come join in the conversation about Cake Pull Request Code Analysis in our Gitter Chat Room

[![Join the chat at https://gitter.im/cake-contrib/Lobby](https://badges.gitter.im/cake-contrib/Lobby.svg)](https://gitter.im/cake-contrib/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## Addins

This addin only implements the core logic.
You'll need additionally an addin for the pull request system you use and one or more issue providers.

### Supported Pull Request systems

|Addin|Developer|NuGet Package|Repository|Description|
|:--:|:--:|:--:|:--:|:--:|
|TFS / VSTS|[BBT Software AG]|[![NuGet](https://img.shields.io/nuget/v/Cake.Prca.PullRequests.Tfs.svg)](https://www.nuget.org/packages/Cake.Prca.PullRequests.Tfs)|[![GitHub stars](https://img.shields.io/github/stars/cake-contrib/Cake.Prca.PullRequests.Tfs.svg?style=social&label=Star)](https://github.com/cake-contrib/Cake.Prca.PullRequests.Tfs)|Adds support for the Pull Request Code Analysis Addin to write issues to Team Foundation Server or Visual Studio Team Services pull requests.|

[Full list of pull request system addins](https://www.nuget.org/packages?q=Tags%3A%22Cake-Prca-PullRequestSystem%22)

### Supported Issue Provider

|Addin|Developer|NuGet Package|Repository|Description|
|:--:|:--:|:--:|:--:|:--:|
|MsBuild|[BBT Software AG]|[![NuGet](https://img.shields.io/nuget/v/Cake.Prca.Issues.MsBuild.svg)](https://www.nuget.org/packages/Cake.Prca.Issues.MsBuild)|[![GitHub stars](https://img.shields.io/github/stars/cake-contrib/Cake.Prca.Issues.MsBuild.svg?style=social&label=Star)](https://github.com/cake-contrib/Cake.Prca.Issues.MsBuild)|Adds upport for the Pull Request Code Analysis Addin for Cake to write any issues logged as warnings in a MsBuild log to a pull request.|

[Full list of issue provider addins](https://www.nuget.org/packages?q=Tags%3A%22Cake-Prca-IssueProvider%22)

## Build

To build this package we are using Cake.

On Windows PowerShell run:

```powershell
./build
```

On OSX/Linux run:

```bash
./build.sh
```

[BBT Software AG]: https://github.com/BBTSoftwareAG
