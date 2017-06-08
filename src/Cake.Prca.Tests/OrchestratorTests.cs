namespace Cake.Prca.Tests
{
    using System.Collections.Generic;
    using System.Linq;
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
                var fixture = new PrcaFixture
                {
                    Log = null
                };

                // When
                var result = Record.Exception(() => fixture.RunOrchestrator());

                // Then
                result.IsArgumentNullException("log");
            }

            [Fact]
            public void Should_Throw_If_Code_Analysis_Provider_List_Is_Null()
            {
                // Given
                var fixture = new PrcaFixture
                {
                    CodeAnalysisProviders = null
                };

                // When
                var result = Record.Exception(() => fixture.RunOrchestrator());

                // Then
                result.IsArgumentNullException("codeAnalysisProviders");
            }

            [Fact]
            public void Should_Throw_If_Code_Analysis_Provider_List_Is_Empty()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProviders.Clear();

                // When
                var result = Record.Exception(() => fixture.RunOrchestrator());

                // Then
                result.IsArgumentException("codeAnalysisProviders");
            }

            [Fact]
            public void Should_Throw_If_Code_Analysis_Provider_Is_Null()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(null);

                // When
                var result = Record.Exception(() => fixture.RunOrchestrator());

                // Then
                result.IsArgumentOutOfRangeException("codeAnalysisProviders");
            }

            [Fact]
            public void Should_Throw_If_Pull_Request_System_Is_Null()
            {
                // Given
                var fixture = new PrcaFixture
                {
                    PullRequestSystem = null
                };

                // When
                var result = Record.Exception(() => fixture.RunOrchestrator());

                // Then
                result.IsArgumentNullException("pullRequestSystem");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new PrcaFixture
                {
                    Settings = null
                };

                // When
                var result = Record.Exception(() => fixture.RunOrchestrator());

                // Then
                result.IsArgumentNullException("settings");
            }
        }

        public sealed class TheRunMethod
        {
            [Theory]
            [InlineData(PrcaCommentFormat.Undefined)]
            [InlineData(PrcaCommentFormat.Html)]
            [InlineData(PrcaCommentFormat.Markdown)]
            [InlineData(PrcaCommentFormat.PlainText)]
            public void Should_Use_The_Correct_Comment_Format(PrcaCommentFormat format)
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>(),
                        new List<FilePath>())
                    {
                        CommentFormat = format
                    };

                // When
                fixture.RunOrchestrator();

                // Then
                fixture.CodeAnalysisProviders.ShouldAllBe(x => x.Format == format);
            }

            [Fact]
            public void Should_Initialize_Code_Analysis_Provider()
            {
                // Given
                var fixture = new PrcaFixture();

                // When
                fixture.RunOrchestrator();

                // Then
                fixture.CodeAnalysisProviders.ShouldAllBe(x => x.PrcaSettings == fixture.Settings);
            }

            [Fact]
            public void Should_Initialize_All_Code_Analysis_Provider()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                10,
                                "Foo",
                                0,
                                "Foo",
                                "Foo"),
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                12,
                                "Bar",
                                0,
                                "Bar",
                                "Bar")
                        }));
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\Foo.cs",
                                5,
                                "Foo",
                                0,
                                "Foo",
                                "Foo"),
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\Bar.cs",
                                7,
                                "Bar",
                                0,
                                "Bar",
                                "Bar")
                        }));

                // When
                fixture.RunOrchestrator();

                // Then
                fixture.CodeAnalysisProviders.ShouldAllBe(x => x.PrcaSettings == fixture.Settings);
            }

            [Fact]
            public void Should_Initialize_Pull_Request_System()
            {
                // Given
                var fixture = new PrcaFixture();

                // When
                fixture.RunOrchestrator();

                // Then
                fixture.PullRequestSystem.PrcaSettings.ShouldBe(fixture.Settings);
            }

            [Fact]
            public void Should_Read_Correct_Number_Of_Code_Analysis_Issues()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                10,
                                "Foo",
                                0,
                                "Foo",
                                "Foo"),
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                12,
                                "Bar",
                                0,
                                "Bar",
                                "Bar")
                        }));

                // When
                fixture.RunOrchestrator();

                // Then
                fixture.Log.Entries.ShouldContain(x => x.Message == "Processing 2 new issues");
            }

            [Fact]
            public void Should_Read_Correct_Number_Of_Code_Analysis_Issues_Not_Related_To_A_File()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                null,
                                null,
                                "Foo",
                                0,
                                "Foo",
                                "Foo"),
                            new CodeAnalysisIssue(
                                null,
                                null,
                                "Bar",
                                0,
                                "Bar",
                                "Bar")
                        }));

                // When
                fixture.RunOrchestrator();

                // Then
                fixture.Log.Entries.ShouldContain(x => x.Message == "Processing 2 new issues");
            }

            [Fact]
            public void Should_Read_Correct_Number_Of_Code_Analysis_Issues_From_Multiple_Providers()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                10,
                                "Foo",
                                0,
                                "Foo",
                                "Foo"),
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                12,
                                "Bar",
                                0,
                                "Bar",
                                "Bar")
                        }));
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\Foo.cs",
                                5,
                                "Foo",
                                0,
                                "Foo",
                                "Foo"),
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\Bar.cs",
                                7,
                                "Bar",
                                0,
                                "Bar",
                                "Bar")
                        }));

                // When
                fixture.RunOrchestrator();

                // Then
                fixture.Log.Entries.ShouldContain(x => x.Message == "Processing 4 new issues");
            }

            [Fact]
            public void Should_Ignore_Issues_If_File_Is_Not_Modified()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                10,
                                "Foo",
                                0,
                                "Foo",
                                "Foo"),
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\NotModified.cs",
                                12,
                                "Bar",
                                0,
                                "Bar",
                                "Bar")
                        }));

                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>(),
                        new List<FilePath>
                        {
                            new FilePath(@"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs")
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
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                10,
                                "Foo",
                                0,
                                "Foo",
                                "Foo"),
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                12,
                                "Bar",
                                0,
                                "Bar",
                                "Bar")
                        }));

                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>
                        {
                            new PrcaDiscussionThread(
                                1,
                                PrcaDiscussionStatus.Active,
                                new FilePath(@"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs"),
                                new List<IPrcaDiscussionComment>
                                {
                                    new PrcaDiscussionComment()
                                    {
                                        Content = "Foo",
                                        IsDeleted = false
                                    }
                                })
                            {
                                CommentSource = fixture.Settings.CommentSource,
                            }
                        },
                        new List<FilePath>
                        {
                            new FilePath(@"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs")
                        });

                // When
                fixture.RunOrchestrator();

                // Then
                fixture.Log.Entries.ShouldContain(x => x.Message == "1 issue(s) were filtered because they were already present");
                fixture.Log.Entries.ShouldContain(x => x.Message.StartsWith("Posting 1 issue(s):"));
            }

            [Fact]
            public void Should_Ignore_Issues_Already_Present_Not_Related_To_A_File()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                null,
                                null,
                                "Foo",
                                0,
                                "Foo",
                                "Foo"),
                            new CodeAnalysisIssue(
                                null,
                                null,
                                "Bar",
                                0,
                                "Bar",
                                "Bar")
                        }));

                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>
                        {
                            new PrcaDiscussionThread(
                                1,
                                PrcaDiscussionStatus.Active,
                                null,
                                new List<IPrcaDiscussionComment>
                                {
                                    new PrcaDiscussionComment()
                                    {
                                        Content = "Foo",
                                        IsDeleted = false
                                    }
                                })
                            {
                                CommentSource = fixture.Settings.CommentSource,
                            }
                        },
                        new List<FilePath>
                        {
                            new FilePath(@"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs")
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
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                10,
                                "Foo",
                                0,
                                "Foo",
                                "Foo")
                        }));

                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>
                        {
                            new PrcaDiscussionThread(
                                1,
                                PrcaDiscussionStatus.Active,
                                new FilePath(@"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs"),
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
                            new FilePath(@"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs")
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
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                10,
                                "Foo",
                                0,
                                "Foo",
                                "Foo"),
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                12,
                                "Bar",
                                0,
                                "Bar",
                                "Bar")
                        }));

                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>(),
                        new List<FilePath>
                        {
                            new FilePath(@"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs")
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
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                10,
                                "Foo",
                                0,
                                "Foo",
                                "Foo"),
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\NotModified.cs",
                                12,
                                "Bar",
                                0,
                                "Bar",
                                "Bar")
                        }));

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
                var threadToResolve =
                    new PrcaDiscussionThread(
                        1,
                        PrcaDiscussionStatus.Active,
                        new FilePath(@"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs"),
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

                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            new CodeAnalysisIssue(
                                @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                                10,
                                "Foo",
                                0,
                                "Foo",
                                "Foo")
                        }));

                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>
                        {
                            threadToResolve
                        },
                        new List<FilePath>
                        {
                            new FilePath(@"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs")
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
                var issueToPost =
                    new CodeAnalysisIssue(
                        @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                        10,
                        "Foo",
                        0,
                        "Foo",
                        "Foo");

                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            issueToPost
                        }));

                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>(),
                        new List<FilePath>
                        {
                            new FilePath(@"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs")
                        });

                // When
                fixture.RunOrchestrator();

                // Then
                fixture.PullRequestSystem.PostedIssues.ShouldContain(issueToPost);
                fixture.Log.Entries.ShouldContain(
                    x =>
                        x.Message ==
                            $"Posting 1 issue(s):\n  Rule: {issueToPost.Rule} Line: {issueToPost.Line} File: {issueToPost.AffectedFileRelativePath}");
            }

            [Fact]
            public void Should_Post_Issue_Not_Related_To_A_File()
            {
                // Given
                var issueToPost =
                    new CodeAnalysisIssue(
                        null,
                        null,
                        "Foo",
                        0,
                        "Foo",
                        "Foo");

                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            issueToPost
                        }));

                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>(),
                        new List<FilePath>
                        {
                            new FilePath(@"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs")
                        });

                // When
                fixture.RunOrchestrator();

                // Then
                fixture.PullRequestSystem.PostedIssues.ShouldContain(issueToPost);
                fixture.Log.Entries.ShouldContain(
                    x =>
                        x.Message ==
                            $"Posting 1 issue(s):\n  Rule: {issueToPost.Rule} Line: {issueToPost.Line} File: {issueToPost.AffectedFileRelativePath}");
            }

            [Fact]
            public void Should_Return_Correct_Values()
            {
                // Given
                var reportedIssue =
                    new CodeAnalysisIssue(
                        @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                        10,
                        "Foo",
                        0,
                        "Foo",
                        "Foo");
                var postedIssue =
                    new CodeAnalysisIssue(
                        @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                        10,
                        "Foo",
                        0,
                        "Foo",
                        "Foo");
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            postedIssue, reportedIssue
                        }));

                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>(),
                        new List<FilePath>
                        {
                            new FilePath(@"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs")
                        });

                fixture.Settings.MaxIssuesToPost = 1;

                // When
                var result = fixture.RunOrchestrator();

                // Then
                result.ReportedIssues.Count().ShouldBe(2);
                result.ReportedIssues.ShouldContain(reportedIssue);
                result.ReportedIssues.ShouldContain(postedIssue);
                result.PostedIssues.Count().ShouldBe(1);
                result.PostedIssues.ShouldContain(postedIssue);
            }

            [Fact]
            public void Should_Return_Reported_Issues_If_PullRequestSystem_Could_Not_Be_Initialized()
            {
                // Given
                var firstIssue =
                    new CodeAnalysisIssue(
                        @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                        10,
                        "Foo",
                        0,
                        "Foo",
                        "Foo");
                var secondIssue =
                    new CodeAnalysisIssue(
                        @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                        10,
                        "Foo",
                        0,
                        "Foo",
                        "Foo");
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            firstIssue, secondIssue
                        }));

                fixture.PullRequestSystem =
                    new FakePullRequestSystem(
                        fixture.Log,
                        new List<IPrcaDiscussionThread>(),
                        new List<FilePath>
                        {
                            new FilePath(@"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs")
                        })
                    {
                        ShouldFailOnInitialization = true
                    };

                fixture.Settings.MaxIssuesToPost = 1;

                // When
                var result = fixture.RunOrchestrator();

                // Then
                result.ReportedIssues.Count().ShouldBe(2);
                result.ReportedIssues.ShouldContain(firstIssue);
                result.ReportedIssues.ShouldContain(secondIssue);
                result.PostedIssues.Count().ShouldBe(0);
            }
        }
    }
}