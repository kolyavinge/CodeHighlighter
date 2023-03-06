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
    public void AppendNewLine_NoSelection_1()
    {
        _result = _logic.AppendNewLine(new(2, 0), default, default);
        AssertAddedLines(2, 1);
        AssertDeletedLines(0, 0);
    }

    [Test]
    public void AppendNewLine_NoSelection_2()
    {
        _result = _logic.AppendNewLine(new(2, 3), default, default);
        AssertAddedLines(3, 1);
        AssertDeletedLines(0, 0);
    }

    [Test]
    public void AppendNewLine_WithSelection_1()
    {
        _result = _logic.AppendNewLine(new(2, 3), new(2, 0), new(4, 0));
        AssertAddedLines(3, 1);
        AssertDeletedLines(2, 2);
    }

    [Test]
    public void AppendNewLine_WithSelection_2()
    {
        _result = _logic.AppendNewLine(new(2, 3), new(2, 3), new(4, 0));
        AssertAddedLines(3, 1);
        AssertDeletedLines(3, 2);
    }

    [Test]
    public void AppendNewLine_OneLineSelection()
    {
        _result = _logic.AppendNewLine(new(2, 3), new(2, 2), new(2, 5));
        AssertAddedLines(3, 1);
        AssertDeletedLines(0, 0);
    }

    [Test]
    public void InsertText_NoSelection_1()
    {
        _result = _logic.InsertText(new(2, 0), new(4, 0), default, default);
        AssertAddedLines(2, 3);
        AssertDeletedLines(0, 0);
    }

    [Test]
    public void InsertText_NoSelection_2()
    {
        _result = _logic.InsertText(new(2, 3), new(4, 5), default, default);
        AssertAddedLines(3, 2);
        AssertDeletedLines(0, 0);
    }

    [Test]
    public void InsertText_WithSelection_1()
    {
        _result = _logic.InsertText(new(2, 3), new(4, 5), new(2, 0), new(4, 0));
        AssertAddedLines(3, 2);
        AssertDeletedLines(2, 2);
    }

    [Test]
    public void InsertText_WithSelection_2()
    {
        _result = _logic.InsertText(new(2, 3), new(4, 5), new(2, 3), new(4, 0));
        AssertAddedLines(3, 2);
        AssertDeletedLines(3, 2);
    }

    [Test]
    public void InsertText_OneLineSelection()
    {
        _result = _logic.InsertText(new(2, 2), new(4, 5), new(2, 2), new(2, 5));
        AssertAddedLines(3, 2);
        AssertDeletedLines(0, 0);
    }

    [Test]
    public void LeftDelete_NoSelection_1()
    {
        _result = _logic.LeftDelete(new(2, 0), default, default);
        AssertAddedLines(0, 0);
        AssertDeletedLines(2, 1);
    }

    [Test]
    public void LeftDelete_NoSelection_2()
    {
        _result = _logic.LeftDelete(new(2, 2), default, default);
        AssertAddedLines(0, 0);
        AssertDeletedLines(3, 1);
    }

    [Test]
    public void LeftDelete_WithSelection_1()
    {
        _result = _logic.LeftDelete(new(2, 0), new(2, 0), new(4, 0));
        AssertAddedLines(0, 0);
        AssertDeletedLines(2, 2);
    }

    [Test]
    public void LeftDelete_WithSelection_2()
    {
        _result = _logic.LeftDelete(new(2, 0), new(2, 3), new(4, 0));
        AssertAddedLines(0, 0);
        AssertDeletedLines(3, 2);
    }

    [Test]
    public void LeftDelete_OneLineSelection()
    {
        _result = _logic.LeftDelete(new(2, 0), new(2, 0), new(2, 5));
        AssertAddedLines(0, 0);
        AssertDeletedLines(0, 0);
    }

    [Test]
    public void RightDelete_NoSelection_1()
    {
        _result = _logic.RightDelete(new(2, 0), default, default);
        AssertAddedLines(0, 0);
        AssertDeletedLines(2, 1);
    }

    [Test]
    public void RightDelete_NoSelection_2()
    {
        _result = _logic.RightDelete(new(2, 3), default, default);
        AssertAddedLines(0, 0);
        AssertDeletedLines(3, 1);
    }

    [Test]
    public void RightDelete_WithSelection_1()
    {
        _result = _logic.RightDelete(new(2, 0), new(2, 0), new(4, 0));
        AssertAddedLines(0, 0);
        AssertDeletedLines(2, 2);
    }

    [Test]
    public void RightDelete_WithSelection_2()
    {
        _result = _logic.RightDelete(new(2, 0), new(2, 3), new(4, 0));
        AssertAddedLines(0, 0);
        AssertDeletedLines(3, 2);
    }

    [Test]
    public void RightDelete_OneLineSelection()
    {
        _result = _logic.RightDelete(new(2, 0), new(2, 0), new(2, 5));
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
