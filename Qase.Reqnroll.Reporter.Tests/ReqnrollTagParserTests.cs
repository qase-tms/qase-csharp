using Qase.Csharp.Commons.Models.Domain;
using Qase.Reqnroll.Reporter;

namespace Qase.Reqnroll.Reporter.Tests;

public class ReqnrollTagParserTests
{
    #region QaseId Tests

    [Fact]
    public void ParseTags_SingleQaseId_SetsTestopsIds()
    {
        var result = new TestResult();
        var tags = new[] { "QaseId:123" };

        ReqnrollTagParser.ApplyTags(result, tags);

        result.TestopsIds.Should().BeEquivalentTo(new List<long> { 123 });
    }

    [Fact]
    public void ParseTags_MultipleQaseIds_SetsAllIds()
    {
        var result = new TestResult();
        var tags = new[] { "QaseId:123,456,789" };

        ReqnrollTagParser.ApplyTags(result, tags);

        result.TestopsIds.Should().BeEquivalentTo(new List<long> { 123, 456, 789 });
    }

    [Fact]
    public void ParseTags_QaseIdCaseInsensitive_SetsTestopsIds()
    {
        var result = new TestResult();
        var tags = new[] { "qaseid:42" };

        ReqnrollTagParser.ApplyTags(result, tags);

        result.TestopsIds.Should().BeEquivalentTo(new List<long> { 42 });
    }

    [Fact]
    public void ParseTags_InvalidQaseId_SkipsGracefully()
    {
        var result = new TestResult();
        var tags = new[] { "QaseId:abc" };

        ReqnrollTagParser.ApplyTags(result, tags);

        result.TestopsIds.Should().BeNull();
    }

    #endregion

    #region QaseTitle Tests

    [Fact]
    public void ParseTags_QaseTitle_SetsTitle()
    {
        var result = new TestResult();
        var tags = new[] { "QaseTitle:Custom_Test_Title" };

        ReqnrollTagParser.ApplyTags(result, tags);

        result.Title.Should().Be("Custom Test Title");
    }

    [Fact]
    public void ParseTags_QaseTitleCaseInsensitive_SetsTitle()
    {
        var result = new TestResult();
        var tags = new[] { "qasetitle:My_Title" };

        ReqnrollTagParser.ApplyTags(result, tags);

        result.Title.Should().Be("My Title");
    }

    #endregion

    #region QaseFields Tests

    [Fact]
    public void ParseTags_QaseFields_AddsToFields()
    {
        var result = new TestResult();
        var tags = new[] { "QaseFields:severity:critical" };

        ReqnrollTagParser.ApplyTags(result, tags);

        result.Fields.Should().ContainKey("severity");
        result.Fields["severity"].Should().Be("critical");
    }

    [Fact]
    public void ParseTags_MultipleQaseFields_AddsAll()
    {
        var result = new TestResult();
        var tags = new[] { "QaseFields:severity:critical", "QaseFields:priority:high" };

        ReqnrollTagParser.ApplyTags(result, tags);

        result.Fields.Should().HaveCount(2);
        result.Fields["severity"].Should().Be("critical");
        result.Fields["priority"].Should().Be("high");
    }

    #endregion

    #region QaseSuite Tests

    [Fact]
    public void ParseTags_QaseSuite_SetsSuiteHierarchy()
    {
        var result = new TestResult();
        var tags = new[] { @"QaseSuite:Auth\Login" };

        ReqnrollTagParser.ApplyTags(result, tags);

        result.Relations!.Suite.Data.Should().HaveCount(2);
        result.Relations.Suite.Data[0].Title.Should().Be("Auth");
        result.Relations.Suite.Data[1].Title.Should().Be("Login");
    }

    [Fact]
    public void ParseTags_QaseSuiteSingleLevel_SetsSingleSuite()
    {
        var result = new TestResult();
        var tags = new[] { "QaseSuite:Smoke" };

        ReqnrollTagParser.ApplyTags(result, tags);

        result.Relations!.Suite.Data.Should().HaveCount(1);
        result.Relations.Suite.Data[0].Title.Should().Be("Smoke");
    }

    #endregion

    #region QaseIgnore Tests

    [Fact]
    public void ParseTags_QaseIgnore_SetsIgnore()
    {
        var result = new TestResult();
        var tags = new[] { "QaseIgnore" };

        ReqnrollTagParser.ApplyTags(result, tags);

        result.Ignore.Should().BeTrue();
    }

    #endregion

    #region Mixed Tags Tests

    [Fact]
    public void ParseTags_MixedTags_AppliesAll()
    {
        var result = new TestResult();
        var tags = new[] { "QaseId:101", "QaseTitle:Login_Test", "QaseFields:severity:high", "QaseSuite:Auth", "sometag" };

        ReqnrollTagParser.ApplyTags(result, tags);

        result.TestopsIds.Should().BeEquivalentTo(new List<long> { 101 });
        result.Title.Should().Be("Login Test");
        result.Fields["severity"].Should().Be("high");
        result.Relations!.Suite.Data[0].Title.Should().Be("Auth");
    }

    [Fact]
    public void ParseTags_NoQaseTags_LeavesDefaultValues()
    {
        var result = new TestResult();
        var tags = new[] { "smoke", "regression", "p1" };

        ReqnrollTagParser.ApplyTags(result, tags);

        result.TestopsIds.Should().BeNull();
        result.Ignore.Should().BeFalse();
    }

    [Fact]
    public void ParseTags_EmptyTags_DoesNothing()
    {
        var result = new TestResult();

        ReqnrollTagParser.ApplyTags(result, Array.Empty<string>());

        result.TestopsIds.Should().BeNull();
        result.Ignore.Should().BeFalse();
    }

    [Fact]
    public void ParseTags_NullTags_DoesNothing()
    {
        var result = new TestResult();

        ReqnrollTagParser.ApplyTags(result, null);

        result.TestopsIds.Should().BeNull();
    }

    #endregion

    #region HasQaseId Tests

    [Fact]
    public void HasQaseId_WithQaseIdTag_ReturnsTrue()
    {
        var tags = new[] { "QaseId:123" };

        ReqnrollTagParser.HasQaseId(tags).Should().BeTrue();
    }

    [Fact]
    public void HasQaseId_WithoutQaseIdTag_ReturnsFalse()
    {
        var tags = new[] { "smoke", "regression" };

        ReqnrollTagParser.HasQaseId(tags).Should().BeFalse();
    }

    #endregion
}
