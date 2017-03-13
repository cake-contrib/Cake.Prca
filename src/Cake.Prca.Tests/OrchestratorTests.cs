namespace Cake.Prca.Tests
{
    using System.Collections.Generic;
    using Core.IO;
    using Prca.Issues;
    using Prca.PullRequests;
    using Shouldly;
    using Xunit;

    public class OrchestratorTests
    {
        public sealed class TheOrchestrator
        {
            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.Log = null;

                // When
                var result = Record.Exception(() => fixture.RunOrchestrator());

                // Then
                result.IsArgumentNullException("log");
            }

            [Fact]
            public void Should_Throw_If_Code_Analysis_Provider_Is_Null()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProvider = null;

                // When
                var result = Record.Exception(() => fixture.RunOrchestrator());

                // Then
                result.IsArgumentNullException("codeAnalysisProvider");
            }

            [Fact]
            public void Should_Throw_If_Pull_Request_System_Is_Null()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.PullRequestSystem = null;

                // When
                var result = Record.Exception(() => fixture.RunOrchestrator());

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
                var result = Record.Exception(() => fixture.RunOrchestrator());

                // Then
                result.IsArgumentNullException("settings");
            }
        }

        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Read_Correct_Number_Of_Code_Analysis_Issues()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProvider =
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                10,
                                "Foo",
                                0,
                                "Foo"
                            ),
                            new CodeAnalysisIssue(
                                @"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                12,
                                "Bar",
                                0,
                                "Bar"
                            )
                        });

                // When
                fixture.RunOrchestrator();

                // Then
                fixture.Log.Entries.ShouldContain(x => x.Message == "Processing 2 new issues");
            }

            [Fact]
            public void Should_Ignore_Issues_If_File_Is_Not_Modified()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProvider =
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                10,
                                "Foo",
                                0,
                                "Foo"
                            ),
                            new CodeAnalysisIssue(
                                @"\src\Cake.Prca.Tests\NotModified.cs",
                                12,
                                "Bar",
                                0,
                                "Bar"
                            )
                        });
                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>(),
                        new List<FilePath>
                        {
                            new FilePath(@"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs")
                        });

                // When
                fixture.RunOrchestrator();

                // Then
                fixture.Log.Entries.ShouldContain(x => x.Message == "1 issue(s) were filtered because they do not belong to files that were changed in this pull request");
                fixture.Log.Entries.ShouldContain(x => x.Message.StartsWith("Posting 1 issue(s):"));
            }

            [Fact]
            public void Should_Ignore_Issues_Already_Present()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProvider =
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                10,
                                "Foo",
                                0,
                                "Foo"
                            ),
                            new CodeAnalysisIssue(
                                @"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                12,
                                "Bar",
                                0,
                                "Bar"
                            )
                        });
                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>
                        {
                            new PrcaDiscussionThread(
                                1,
                                PrcaDiscussionStatus.Active,
                                new FilePath(@"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs"),
                                new List<IPrcaDiscussionComment>
                                {
                                    new PrcaDiscussionComment()
                                    {
                                        Content = "Foo",
                                        IsDeleted = false
                                    }
                                }
                                )
                            {
                                CommentSource = fixture.Settings.CommentSource,
                            }
                        },
                        new List<FilePath>
                        {
                            new FilePath(@"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs")
                        });

                // When
                fixture.RunOrchestrator();

                // Then
                fixture.Log.Entries.ShouldContain(x => x.Message == "1 issue(s) were filtered because they were already present");
                fixture.Log.Entries.ShouldContain(x => x.Message.StartsWith("Posting 1 issue(s):"));
            }

            [Fact]
            public void Should_Only_Ignore_Issues_With_Same_Comment_Source()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProvider =
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                10,
                                "Foo",
                                0,
                                "Foo"
                            )
                        });
                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>
                        {
                            new PrcaDiscussionThread(
                                1,
                                PrcaDiscussionStatus.Active,
                                new FilePath(@"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs"),
                                new List<IPrcaDiscussionComment>
                                {
                                    new PrcaDiscussionComment()
                                    {
                                        Content = "Foo",
                                        IsDeleted = false
                                    }
                                })
                            {
                                CommentSource = "DifferentCommentSource",
                            }
                        },
                        new List<FilePath>
                        {
                            new FilePath(@"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs")
                        });

                // When
                fixture.RunOrchestrator();

                // Then
                fixture.Log.Entries.ShouldContain(x => x.Message == "0 issue(s) were filtered because they were already present");
                fixture.Log.Entries.ShouldContain(x => x.Message.StartsWith("Posting 1 issue(s):"));
            }

            [Fact]
            public void Should_Limit_Messages_To_Maximum()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProvider =
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                10,
                                "Foo",
                                0,
                                "Foo"
                            ),
                            new CodeAnalysisIssue(
                                @"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                12,
                                "Bar",
                                0,
                                "Bar"
                            )
                        });
                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>(),
                        new List<FilePath>
                        {
                            new FilePath(@"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs")
                        });
                fixture.Settings.MaxIssuesToPost = 1;

                // When
                fixture.RunOrchestrator();

                // Then
                fixture.Log.Entries.ShouldContain(x => x.Message == "1 issue(s) were filtered to match the maximum 1 comments limit");
                fixture.Log.Entries.ShouldContain(x => x.Message.StartsWith("Posting 1 issue(s):"));
            }

            [Fact]
            public void Should_Log_Message_If_All_Issues_Are_Filtered()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProvider =
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                10,
                                "Foo",
                                0,
                                "Foo"
                            ),
                            new CodeAnalysisIssue(
                                @"\src\Cake.Prca.Tests\NotModified.cs",
                                12,
                                "Bar",
                                0,
                                "Bar"
                            )
                        });
                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>(),
                        new List<FilePath>());

                // When
                fixture.RunOrchestrator();

                // Then
                fixture.Log.Entries.ShouldContain(x => x.Message == "All issues were filtered. Nothing new to post.");
            }

            [Fact]
            public void Should_Resolve_Closed_Issues()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProvider =
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                10,
                                "Foo",
                                0,
                                "Foo"
                            )
                        });
                var threadToResolve =
                    new PrcaDiscussionThread(
                        1,
                        PrcaDiscussionStatus.Active,
                        new FilePath(@"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs"),
                        new List<IPrcaDiscussionComment>
                        {
                            new PrcaDiscussionComment()
                            {
                                Content = "Bar",
                                IsDeleted = false
                            }
                        })
                    {
                        CommentSource = null
                    };
                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>
                        {
                            threadToResolve
                        },
                        new List<FilePath>
                        {
                            new FilePath(@"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs")
                        });

                // When
                fixture.RunOrchestrator();

                // Then
                fixture.PullRequestSystem.ThreadsMarkedAsFixed.ShouldContain(threadToResolve);
                fixture.Log.Entries.ShouldContain(x => x.Message == "Found 1 existing thread(s) that do not match any new issue and can be resolved.");
            }

            [Fact]
            public void Should_Post_Issue()
            {
                // Given
                var fixture = new PrcaFixture();
                var issueToPost =
                    new CodeAnalysisIssue(
                        @"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                        10,
                        "Foo",
                        0,
                        "Foo"
                    );

                fixture.CodeAnalysisProvider =
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            issueToPost
                        });
                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>(),
                        new List<FilePath>
                        {
                            new FilePath(@"\src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs")
                        });

                // When
                fixture.RunOrchestrator();

                // Then
                fixture.PullRequestSystem.PostedIssues.ShouldContain(issueToPost);
                fixture.Log.Entries.ShouldContain(
                    x => x.Message ==
                        string.Format(
                            "Posting 1 issue(s):\n  Rule: {0} Line: {1} File: {2}",
                            issueToPost.Rule,
                            issueToPost.Line,
                            issueToPost.AffectedFileRelativePath));
            }
        }
    }
}