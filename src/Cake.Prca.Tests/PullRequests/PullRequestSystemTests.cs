namespace Cake.Prca.Tests.PullRequests
{
    using System.Collections.Generic;
    using Prca.Issues;
    using Prca.PullRequests;
    using Shouldly;
    using Testing;
    using Xunit;

    public sealed class PullRequestSystemTests
    {
        public sealed class ThePullRequestSystemCtor
        {
            [Fact]
            public void Should_Throw_If_File_Log_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() => new FakePullRequestSystem(null));

                // Then
                result.IsArgumentNullException("log");
            }

            [Fact]
            public void Should_Set_Log()
            {
                // Given
                var log = new FakeLog();

                // When
                var prSystem = new FakePullRequestSystem(log);

                // Then
                prSystem.Log.ShouldBe(log);
            }
        }

        public sealed class TheFetchActiveDiscussionThreadsMethod
        {
            [Fact]
            public void Should_Throw_If_PrcaSettings_Is_Null()
            {
                // Given
                var provider = new FakePullRequestSystem(new FakeLog());

                // When
                var result = Record.Exception(() => provider.FetchActiveDiscussionThreads("Foo"));

                // Then
                result.IsInvalidOperationException("Initialize needs to be called first.");
            }
        }

        public sealed class TheGetModifiedFilesInPullRequestMethod
        {
            [Fact]
            public void Should_Throw_If_PrcaSettings_Is_Null()
            {
                // Given
                var provider = new FakePullRequestSystem(new FakeLog());

                // When
                var result = Record.Exception(() => provider.GetModifiedFilesInPullRequest());

                // Then
                result.IsInvalidOperationException("Initialize needs to be called first.");
            }
        }

        public sealed class TheMarkThreadsAsFixedMethod
        {
            [Fact]
            public void Should_Throw_If_PrcaSettings_Is_Null()
            {
                // Given
                var provider = new FakePullRequestSystem(new FakeLog());

                // When
                var result = Record.Exception(() => provider.MarkThreadsAsFixed(new List<IPrcaDiscussionThread>()));

                // Then
                result.IsInvalidOperationException("Initialize needs to be called first.");
            }
        }

        public sealed class ThePostDiscussionThreadsMethod
        {
            [Fact]
            public void Should_Throw_If_PrcaSettings_Is_Null()
            {
                // Given
                var provider = new FakePullRequestSystem(new FakeLog());

                // When
                var result = Record.Exception(() => provider.PostDiscussionThreads(new List<ICodeAnalysisIssue>(), "Foo"));

                // Then
                result.IsInvalidOperationException("Initialize needs to be called first.");
            }
        }
    }
}
