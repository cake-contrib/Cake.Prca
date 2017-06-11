namespace Cake.Prca.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Prca.Issues;
    using Shouldly;
    using Xunit;

    public sealed class IssueReaderTests
    {
        public sealed class TheCtor
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
                var result = Record.Exception(() => fixture.ReadIssues(PrcaCommentFormat.Undefined));

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
                var result = Record.Exception(() => fixture.ReadIssues(PrcaCommentFormat.Undefined));

                // Then
                result.IsArgumentNullException("issueProviders");
            }

            [Fact]
            public void Should_Throw_If_Code_Analysis_Provider_List_Is_Empty()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProviders.Clear();

                // When
                var result = Record.Exception(() => fixture.ReadIssues(PrcaCommentFormat.Undefined));

                // Then
                result.IsArgumentException("issueProviders");
            }

            [Fact]
            public void Should_Throw_If_Code_Analysis_Provider_Is_Null()
            {
                // Given
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(null);

                // When
                var result = Record.Exception(() => fixture.ReadIssues(PrcaCommentFormat.Undefined));

                // Then
                result.IsArgumentOutOfRangeException("issueProviders");
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
                var result = Record.Exception(() => fixture.ReadIssues(PrcaCommentFormat.Undefined));

                // Then
                result.IsArgumentNullException("settings");
            }
        }

        public sealed class TheReadIssuesMethod
        {
            [Fact]
            public void Should_Initialize_Code_Analysis_Provider()
            {
                // Given
                var fixture = new PrcaFixture();

                // When
                fixture.ReadIssues(PrcaCommentFormat.Undefined);

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
                fixture.ReadIssues(PrcaCommentFormat.Undefined);

                // Then
                fixture.CodeAnalysisProviders.ShouldAllBe(x => x.PrcaSettings == fixture.Settings);
            }

            [Fact]
            public void Should_Read_Correct_Number_Of_Code_Analysis_Issues()
            {
                // Given
                var issue1 =
                    new CodeAnalysisIssue(
                        @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                        10,
                        "Foo",
                        0,
                        "Foo",
                        "Foo");
                var issue2 =
                    new CodeAnalysisIssue(
                        @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                        12,
                        "Bar",
                        0,
                        "Bar",
                        "Bar");
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            issue1,
                            issue2
                        }));

                // When
                var issues = fixture.ReadIssues(PrcaCommentFormat.Undefined).ToList();

                // Then
                issues.Count.ShouldBe(2);
                issues.ShouldContain(issue1);
                issues.ShouldContain(issue2);
            }

            [Fact]
            public void Should_Read_Correct_Number_Of_Code_Analysis_Issues_Not_Related_To_A_File()
            {
                // Given
                var issue1 =
                    new CodeAnalysisIssue(
                        null,
                        null,
                        "Foo",
                        0,
                        "Foo",
                        "Foo");
                var issue2 =
                    new CodeAnalysisIssue(
                        null,
                        null,
                        "Bar",
                        0,
                        "Bar",
                        "Bar");
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            issue1,
                            issue2
                        }));

                // When
                var issues = fixture.ReadIssues(PrcaCommentFormat.Undefined).ToList();

                // Then
                issues.Count.ShouldBe(2);
                issues.ShouldContain(issue1);
                issues.ShouldContain(issue2);
            }

            [Fact]
            public void Should_Read_Correct_Number_Of_Code_Analysis_Issues_From_Multiple_Providers()
            {
                // Given
                var issue1 =
                    new CodeAnalysisIssue(
                        @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                        10,
                        "Foo",
                        0,
                        "Foo",
                        "Foo");
                var issue2 =
                    new CodeAnalysisIssue(
                        @"src\Cake.Prca.Tests\FakeCodeAnalysisProvider.cs",
                        12,
                        "Bar",
                        0,
                        "Bar",
                        "Bar");
                var issue3 =
                    new CodeAnalysisIssue(
                        @"src\Cake.Prca.Tests\Foo.cs",
                        5,
                        "Foo",
                        0,
                        "Foo",
                        "Foo");
                var issue4 =
                    new CodeAnalysisIssue(
                        @"src\Cake.Prca.Tests\Bar.cs",
                        7,
                        "Bar",
                        0,
                        "Bar",
                        "Bar");
                var fixture = new PrcaFixture();
                fixture.CodeAnalysisProviders.Clear();
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            issue1,
                            issue2
                        }));
                fixture.CodeAnalysisProviders.Add(
                    new FakeCodeAnalysisProvider(
                        fixture.Log,
                        new List<ICodeAnalysisIssue>
                        {
                            issue3,
                            issue4
                        }));

                // When
                var issues = fixture.ReadIssues(PrcaCommentFormat.Undefined).ToList();

                // Then
                issues.Count.ShouldBe(4);
                issues.ShouldContain(issue1);
                issues.ShouldContain(issue2);
                issues.ShouldContain(issue3);
                issues.ShouldContain(issue4);
            }
        }
    }
}