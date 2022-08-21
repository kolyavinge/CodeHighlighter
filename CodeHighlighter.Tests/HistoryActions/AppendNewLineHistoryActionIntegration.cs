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
        MakeInactiveSelection();

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
    public void NoSelection_VirtualCursor()
    {
        _context.InputModel.SetText("    text");
        _context.InputModel.MoveCursorTo(new(0, 8));
        MakeInactiveSelection();

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
        MakeInactiveSelection();

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
}
