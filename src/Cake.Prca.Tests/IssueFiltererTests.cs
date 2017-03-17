namespace Cake.Prca.Tests
{
    using System.Collections.Generic;
    using Core.IO;
    using Prca.Issues;
    using Prca.PullRequests;
    using Xunit;

    public class IssueFiltererTests
    {
        public sealed class TheIssueFilterer
        {
            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.Log = null;

                // When
                var result = Record.Exception(() => fixture.FilterIssues(null, null));

                // Then
                result.IsArgumentNullException("log");
            }

            [Fact]
            public void Should_Throw_If_Pull_Request_System_Is_Null()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.PullRequestSystem = null;

                // When
                var result = Record.Exception(() => fixture.FilterIssues(null, null));

                // Then
                result.IsArgumentNullException("pullRequestSystem");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.FilterIssues(null, null));

                // Then
                result.IsArgumentNullException("settings");
            }
        }

        public sealed class TheFilterIssuesMethod
        {
            [Fact]
            public void Should_Throw_If_Issues_Are_Null()
            {
                // Given
                var fixture = new PrcaFixture();

                // When
                var result = Record.Exception(() => fixture.FilterIssues(null, new Dictionary<ICodeAnalysisIssue, IEnumerable<IPrcaDiscussionComment>>()));

                // Then
                result.IsArgumentNullException("issues");
            }

            [Fact]
            public void Should_Throw_If_Issue_Comments_Are_Null()
            {
                // Given
                var fixture = new PrcaFixture();

                // When
                var result = Record.Exception(() => fixture.FilterIssues(new List<ICodeAnalysisIssue>(), null));

                // Then
                result.IsArgumentNullException("issueComments");
            }

            [Fact]
            public void Should_Throw_If_Modified_Files_Contain_Absolute_Path()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>(),
                        new List<FilePath>
                        {
                            new FilePath(@"c:\FakeCodeAnalysisProvider.cs")
                        });

                // When
                var result = Record.Exception(() => 
                    fixture.FilterIssues(
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                10,
                                "Foo",
                                0,
                                "Foo"
                            )
                        },
                        new Dictionary<ICodeAnalysisIssue, IEnumerable<IPrcaDiscussionComment>>()));

                // Then
                result.IsPrcaException(@"Absolute file paths are not suported for modified files. Path: c:/FakeCodeAnalysisProvider.cs");
            }
        }
    }
}
