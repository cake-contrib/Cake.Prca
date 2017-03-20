# Cake Pull Request Code Analysis Addin Release Process

This document describes the necessary steps to release a new version of the Cake Pull Request Code Analysis Addin.

* Create a release branch (eg. `release\1.2.3`).
* Make sure that a GitHub milestone exists for this release.
* Make sure there were issues for all changes with the appropriate labels and the correct milestone set.
* Make sure that you have the following environment variables set in your local development environment:
  * `GITHUB_USERNAME`: Your GitHub user name.
  * `GITHUB_PASSWORD`: Your GitHub password or personal access token.
* Create a GitHub release draft by running: `build -target releasenotes`.
* Check the generated release notes and make required manual changes.
* If release is ready finish release (merge back into `master` and `develop`) but don't tag the release yet.
* Publish the draft release on GitHub.

The last step will tag the release and trigger another build including the publishing.
The build will automatically publish the build artifacts to the GitHub release, publish to NuGet
and notify about the new release through Twitter and Gitter.