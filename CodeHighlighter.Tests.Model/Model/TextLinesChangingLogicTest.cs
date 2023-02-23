using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class TextLinesChangingLogicTest
{
    private TextLinesChangingLogic _logic;
    private LinesChangeResult _result;

    [SetUp]
    public void Setup()
    {
        _logic = new TextLinesChangingLogic();
    }

    [Test]
    public void AppendNewLine_NoSelection()
    {
        _result = _logic.AppendNewLine(2, 0, 0);
        AssertAddedLines(3, 1);
        AssertDeletedLines(0, 0);
    }

    [Test]
    public void AppendNewLine_WithSelection()
    {
        _result = _logic.AppendNewLine(2, 2, 4);
        AssertAddedLines(3, 1);
        AssertDeletedLines(3, 2);
    }

    [Test]
    public void AppendNewLine_OneLineSelection()
    {
        _result = _logic.AppendNewLine(2, 2, 2);
        AssertAddedLines(3, 1);
        AssertDeletedLines(0, 0);
    }

    [Test]
    public void InsertText_NoSelection()
    {
        _result = _logic.InsertText(2, 4, 0, 0);
        AssertAddedLines(3, 2);
        AssertDeletedLines(0, 0);
    }

    [Test]
    public void InsertText_WithSelection()
    {
        _result = _logic.InsertText(2, 4, 2, 3);
        AssertAddedLines(3, 2);
        AssertDeletedLines(3, 1);
    }

    [Test]
    public void InsertText_OneLineSelection()
    {
        _result = _logic.InsertText(2, 4, 2, 2);
        AssertAddedLines(3, 2);
        AssertDeletedLines(0, 0);
    }

    [Test]
    public void LeftDelete_NoSelection()
    {
        _result = _logic.LeftDelete(2, false, 0, 0);
        AssertAddedLines(0, 0);
        AssertDeletedLines(2, 1);
    }

    [Test]
    public void LeftDelete_WithSelection()
    {
        _result = _logic.LeftDelete(2, true, 2, 4);
        AssertAddedLines(0, 0);
        AssertDeletedLines(3, 2);
    }

    [Test]
    public void LeftDelete_OneLineSelection()
    {
        _result = _logic.LeftDelete(2, true, 2, 2);
        AssertAddedLines(0, 0);
        AssertDeletedLines(0, 0);
    }

    [Test]
    public void RightDelete_NoSelection()
    {
        _result = _logic.RightDelete(2, false, 0, 0);
        AssertAddedLines(0, 0);
        AssertDeletedLines(3, 1);
    }

    [Test]
    public void RightDelete_WithSelection()
    {
        _result = _logic.RightDelete(2, true, 2, 4);
        AssertAddedLines(0, 0);
        AssertDeletedLines(3, 2);
    }

    [Test]
    public void RightDelete_OneLineSelection()
    {
        _result = _logic.RightDelete(2, true, 2, 2);
        AssertAddedLines(0, 0);
        AssertDeletedLines(0, 0);
    }

    private void AssertAddedLines(int startLineIndex, int linesCount)
    {
        Assert.That(_result.AddedLines, Is.EqualTo(new LinesChange(startLineIndex, linesCount)));
    }

    private void AssertDeletedLines(int startLineIndex, int linesCount)
    {
        Assert.That(_result.DeletedLines, Is.EqualTo(new LinesChange(startLineIndex, linesCount)));
    }
}
