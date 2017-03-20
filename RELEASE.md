# Cake Pull Request Code Analysis Addin Release Process

This document describes the necessary steps to release a new version of the Cake Pull Request Code Analysis Addin.

* Create a release branch (eg. `release\1.2.3`).
* Make sure that a GitHub milestone exists for this release.
* Make sure there were issues for all changes with the appropriate labels and the correct milestone set.
* Make sure to have Git Release Manager installed locally `choco install gitreleasemanager.portable`.
* Run Git Release Manager locally to create a GitHub release draft:
  `grm create -o cake-contrib -r Cake.Prca -m 1.2.3 -u [yourname] -p [yourpassword]`
* Check the generated release notes and make required manual changes.
* If release is ready finish release (merge back into `master` and `develop`) but don't tag the release yet.
* Publish the draft release on GitHub.

The last step will tag the release and trigger another build including the publishing.
The build will automatically publish the build artifacts to the GitHub release, publish to NuGet
and notify about the new release through Twitter and Gitter.