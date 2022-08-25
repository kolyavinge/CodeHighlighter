using CodeHighlighter.HistoryActions;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.HistoryActions;

internal class AppendNewLineHistoryActionIntegration : BaseHistoryActionIntegration
{
    private AppendNewLineHistoryAction _action;

    [SetUp]
    public void Setup()
    {
        MakeContext();
        _action = new AppendNewLineHistoryAction(_context);
    }

    [Test]
    public void NoSelection()
    {
        _context.InputModel.SetText("text");
        _context.InputModel.MoveCursorTo(new(0, 1));

        _action.Do();
        Assert.AreEqual("t\r\next", _text.ToString());
        AssertCursorPosition(new(1, 0));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text", _text.ToString());
        AssertCursorPosition(new(0, 1));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("t\r\next", _text.ToString());
        AssertCursorPosition(new(1, 0));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection()
    {
        _context.InputModel.SetText("text");
        _context.InputModel.MoveCursorTo(new(1, 1));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(1, 2));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("t\r\nxt", _text.ToString());
        AssertCursorPosition(new(1, 0));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text", _text.ToString());
        AssertCursorPosition(new(0, 2));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("t\r\nxt", _text.ToString());
        AssertCursorPosition(new(1, 0));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection_ManyLines()
    {
        _context.InputModel.SetText("text\r\n123\r\n456");
        _context.InputModel.MoveCursorTo(new(1, 0));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(2, 3));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("text\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 0));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\n123\r\n456", _text.ToString());
        AssertCursorPosition(new(2, 3));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("text\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 0));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void NoSelection_VirtualCursor()
    {
        _context.InputModel.SetText("    text");
        _context.InputModel.MoveCursorTo(new(0, 8));

        _action.Do();
        Assert.AreEqual("    text\r\n", _text.ToString());
        AssertCursorPosition(new(1, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    text", _text.ToString());
        AssertCursorPosition(new(0, 8));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    text\r\n", _text.ToString());
        AssertCursorPosition(new(1, 4, CursorPositionKind.Virtual));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void NoSelection_VirtualCursor_2()
    {
        _context.InputModel.SetText("    text\r\n");
        _context.InputModel.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));

        _action.Do();
        Assert.AreEqual("    text\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    text\r\n", _text.ToString());
        AssertCursorPosition(new(1, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    text\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 4, CursorPositionKind.Virtual));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection_1_VirtualCursor()
    {
        _context.InputModel.SetText("    012\r\n\r\n");
        _context.InputModel.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(2, 0));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("    012\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    012\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 0));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    012\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 4, CursorPositionKind.Virtual));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection_2_VirtualCursor()
    {
        _context.InputModel.SetText("    012\r\n\r\n");
        _context.InputModel.MoveCursorTo(new(0, 7));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("    012\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(1, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    012\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(1, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    012\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(1, 4, CursorPositionKind.Virtual));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection_3_VirtualCursor()
    {
        _context.InputModel.SetText("    012\r\n\r\n\r\n");
        _context.InputModel.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("    012\r\n\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    012\r\n\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    012\r\n\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 4, CursorPositionKind.Virtual));

        InvalidateVisualCallThreeTimes();
    }
}
