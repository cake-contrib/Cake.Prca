namespace Cake.Prca.Tests.Issues
{
    using Prca.Issues;
    using Shouldly;
    using Xunit;

    public sealed class TheCodeAnalysisIssueCtor
    {
        [Fact]
        public void Should_Throw_If_File_Path_Is_Null()
        {
            // Given / When
            var result = Record.Exception(() => new CodeAnalysisIssue(null, 100, "Foo", 1, "Bar"));

            // Then
            result.IsArgumentNullException("filePath");
        }

        [Fact]
        public void Should_Throw_If_File_Path_Is_Empty()
        {
            // Given / When
            var result = Record.Exception(() => new CodeAnalysisIssue(string.Empty, 100, "Foo", 1, "Bar"));

            // Then
            result.IsArgumentOutOfRangeException("filePath");
        }

        [Fact]
        public void Should_Throw_If_File_Path_Is_WhiteSpace()
        {
            // Given / When
            var result = Record.Exception(() => new CodeAnalysisIssue(" ", 100, "Foo", 1, "Bar"));

            // Then
            result.IsArgumentOutOfRangeException("filePath");
        }

        [Fact]
        public void Should_Throw_If_File_Path_Is_Absolute()
        {
            // Given / When
            var result = Record.Exception(() => new CodeAnalysisIssue(@"c:\src\foo.cs", 100, "Foo", 1, "Bar"));

            // Then
            result.IsArgumentOutOfRangeException("filePath");
        }

        [Fact]
        public void Should_Throw_If_Line_Is_Negative()
        {
            // Given / When
            var result = Record.Exception(() => new CodeAnalysisIssue(@"\src\foo.cs", -1, "Foo", 1, "Bar"));

            // Then
            result.IsArgumentOutOfRangeException("line");
        }

        [Fact]
        public void Should_Throw_If_Line_Is_Zero()
        {
            // Given / When
            var result = Record.Exception(() => new CodeAnalysisIssue(@"\src\foo.cs", 0, "Foo", 1, "Bar"));

            // Then
            result.IsArgumentOutOfRangeException("line");
        }

        [Fact]
        public void Should_Throw_If_Message_Is_Null()
        {
            // Given / When
            var result = Record.Exception(() => new CodeAnalysisIssue(@"\src\foo.cs", 100, null, 1, "Bar"));

            // Then
            result.IsArgumentNullException("message");
        }

        [Fact]
        public void Should_Throw_If_Message_Is_Empty()
        {
            // Given / When
            var result = Record.Exception(() => new CodeAnalysisIssue(@"\src\foo.cs", 100, string.Empty, 1, "Bar"));

            // Then
            result.IsArgumentOutOfRangeException("message");
        }

        [Fact]
        public void Should_Throw_If_Message_Is_WhiteSpace()
        {
            // Given / When
            var result = Record.Exception(() => new CodeAnalysisIssue(@"\src\foo.cs", 100, " ", 1, "Bar"));

            // Then
            result.IsArgumentOutOfRangeException("message");
        }

        [Theory]
        [InlineData(@"\foo", @"/foo")]
        [InlineData(@"\foo\", @"/foo")]
        [InlineData(@"/foo", @"/foo")]
        [InlineData(@"/foo/", @"/foo")]
        public void Should_Set_File_Path(string filePath, string expectedFilePath)
        {
            // Given / When
            var issue = new CodeAnalysisIssue(filePath, 100, "Foo", 1, "Bar");

            // Then
            issue.AffectedFileRelativePath.ToString().ShouldBe(expectedFilePath);
        }
    }
}
