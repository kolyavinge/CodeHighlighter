using CodeHighlighter.HistoryActions;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.HistoryActions;

internal class DeleteLeftTokenHistoryActionIntegration : BaseHistoryActionIntegration
{
    private DeleteLeftTokenHistoryAction _action;

    [SetUp]
    public void Setup()
    {
        MakeContext();
        _action = new DeleteLeftTokenHistoryAction(_context);
    }

    [Test]
    public void NoDelete()
    {
        _context.InputModel.SetText("text\r\n123");
        _context.InputModel.MoveCursorTo(new(0, 0));
        Assert.False(_action.Do());
        InvalidateVisualCallNever();
    }

    [Test]
    public void NoSelection()
    {
        _context.InputModel.SetText("text\r\n123");
        _context.InputModel.MoveCursorTo(new(0, 3));

        _action.Do();
        Assert.AreEqual("t\r\n123", _text.ToString());
        AssertCursorPosition(new(0, 0));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\n123", _text.ToString());
        AssertCursorPosition(new(0, 3));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("t\r\n123", _text.ToString());
        AssertCursorPosition(new(0, 0));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void NoSelection_DeleteReturn()
    {
        _context.InputModel.SetText("text\r\n123");
        _context.InputModel.MoveCursorTo(new(1, 0));

        _action.Do();
        Assert.AreEqual("text123", _text.ToString());
        AssertCursorPosition(new(0, 4));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\n123", _text.ToString());
        AssertCursorPosition(new(1, 0));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("text123", _text.ToString());
        AssertCursorPosition(new(0, 4));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection()
    {
        _context.InputModel.SetText("text\r\n123");
        _context.InputModel.MoveCursorTo(new(0, 3));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(1, 1));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("tex23", _text.ToString());
        AssertCursorPosition(new(0, 3));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\n123", _text.ToString());
        AssertCursorPosition(new(1, 1));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("tex23", _text.ToString());
        AssertCursorPosition(new(0, 3));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void NoSelection_VirtualCursor()
    {
        _context.InputModel.SetText("    text\r\n");
        _context.InputModel.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));

        _action.Do();
        Assert.AreEqual("    text\r\n", _text.ToString());
        AssertCursorPosition(new(1, 0));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    text\r\n", _text.ToString());
        AssertCursorPosition(new(1, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    text\r\n", _text.ToString());
        AssertCursorPosition(new(1, 0));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection_1_VirtualCursor()
    {
        _context.InputModel.SetText("    123\r\n\r\n456");
        _context.InputModel.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(2, 3));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("    123\r\n    ", _text.ToString());
        AssertCursorPosition(new(1, 4));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    123\r\n\r\n456", _text.ToString());
        AssertCursorPosition(new(2, 3));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    123\r\n    ", _text.ToString());
        AssertCursorPosition(new(1, 4));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection_2_VirtualCursor()
    {
        _context.InputModel.SetText("    123\r\n\r\n456");
        _context.InputModel.MoveCursorTo(new(0, 7));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("    123\r\n456", _text.ToString());
        AssertCursorPosition(new(0, 7));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    123\r\n\r\n456", _text.ToString());
        AssertCursorPosition(new(1, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    123\r\n456", _text.ToString());
        AssertCursorPosition(new(0, 7));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection_3_VirtualCursor()
    {
        _context.InputModel.SetText("    123\r\n\r\n\r\n456");
        _context.InputModel.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("    123\r\n    \r\n456", _text.ToString());
        AssertCursorPosition(new(1, 4));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    123\r\n\r\n\r\n456", _text.ToString());
        AssertCursorPosition(new(2, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    123\r\n    \r\n456", _text.ToString());
        AssertCursorPosition(new(1, 4));

        InvalidateVisualCallThreeTimes();
    }
}
