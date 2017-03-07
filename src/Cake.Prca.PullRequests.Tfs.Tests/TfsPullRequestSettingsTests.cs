namespace Cake.Prca.PullRequests.Tfs.Tests
{
    using System;
    using Prca.Tests;
    using Xunit;

    public class TfsPullRequestSettingsTests
    {
        public sealed class TheTfsPullRequestSettings
        {
            [Fact]
            public void Should_Throw_If_RepositoryUrl_For_SourceBranch_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() => new TfsPullRequestSettings((Uri)null, "foo"));

                // Then
                result.IsArgumentNullException("repositoryUrl");
            }

            [Fact]
            public void Should_Throw_If_SourceBranch_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() => new TfsPullRequestSettings(new Uri("http://example.com"), (string)null));

                // Then
                result.IsArgumentNullException("sourceBranch");
            }

            [Fact]
            public void Should_Throw_If_SourceBranch_Is_Empty()
            {
                // Given / When
                var result = Record.Exception(() => new TfsPullRequestSettings(new Uri("http://example.com"), string.Empty));

                // Then
                result.IsArgumentOutOfRangeException("sourceBranch");
            }

            [Fact]
            public void Should_Throw_If_SourceBranch_Is_WhiteSpace()
            {
                // Given / When
                var result = Record.Exception(() => new TfsPullRequestSettings(new Uri("http://example.com"), " "));

                // Then
                result.IsArgumentOutOfRangeException("sourceBranch");
            }

            [Fact]
            public void Should_Throw_If_RepositoryUrl_For_PullRequestId_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() => new TfsPullRequestSettings((Uri)null, 0));

                // Then
                result.IsArgumentNullException("repositoryUrl");
            }
        }
    }
}
