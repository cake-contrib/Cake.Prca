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

        public sealed class TheInitializeMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var prSystem = new FakePullRequestSystem(new FakeLog());

                // When
                var result = Record.Exception(() => prSystem.Initialize(null));

                // Then
                result.IsArgumentNullException("settings");
            }

            [Fact]
            public void Should_Set_Settings()
            {
                // Given
                var prSystem = new FakePullRequestSystem(new FakeLog());
                var settings = new ReportIssuesToPullRequestSettings(@"c:\foo");

                // When
                prSystem.Initialize(settings);

                // Then
                prSystem.PrcaSettings.ShouldBe(settings);
            }

            [Fact]
            public void Should_Return_True()
            {
                // Given
                var prSystem = new FakePullRequestSystem(new FakeLog());
                var settings = new ReportIssuesToPullRequestSettings(@"c:\foo");

                // When
                var result = prSystem.Initialize(settings);

                // Then
                result.ShouldBe(true);
            }
        }

        public sealed class TheGetPreferredCommentFormatMethod
        {
            [Fact]
            public void Should_Return_PlainText()
            {
                // Given
                var prSystem = new FakePullRequestSystem(new FakeLog());

                // When
                var result = prSystem.GetPreferredCommentFormat();

                // Then
                result.ShouldBe(PrcaCommentFormat.PlainText);
            }
        }

        public sealed class TheFetchActiveDiscussionThreadsMethod
        {
            [Fact]
            public void Should_Throw_If_PrcaSettings_Is_Null()
            {
                // Given
                var prSystem = new FakePullRequestSystem(new FakeLog());

                // When
                var result = Record.Exception(() => prSystem.FetchActiveDiscussionThreads("Foo"));

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
                var prSystem = new FakePullRequestSystem(new FakeLog());

                // When
                var result = Record.Exception(() => prSystem.GetModifiedFilesInPullRequest());

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
                var prSystem = new FakePullRequestSystem(new FakeLog());

                // When
                var result = Record.Exception(() => prSystem.MarkThreadsAsFixed(new List<IPrcaDiscussionThread>()));

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
                var prSystem = new FakePullRequestSystem(new FakeLog());

                // When
                var result = Record.Exception(() => prSystem.PostDiscussionThreads(new List<ICodeAnalysisIssue>(), "Foo"));

                // Then
                result.IsInvalidOperationException("Initialize needs to be called first.");
            }
        }
    }
}
