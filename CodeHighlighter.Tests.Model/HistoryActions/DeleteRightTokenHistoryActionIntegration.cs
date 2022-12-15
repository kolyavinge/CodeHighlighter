using CodeHighlighter.HistoryActions;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.HistoryActions;

internal class DeleteRightTokenHistoryActionIntegration : BaseHistoryActionIntegration
{
    private DeleteRightTokenHistoryAction _action;

    [SetUp]
    public void Setup()
    {
        MakeContext();
        _action = new DeleteRightTokenHistoryAction(_inputActionsFactory, _context);
    }

    [Test]
    public void NoDelete()
    {
        SetText("text\r\n123");
        MoveCursorTo(new(1, 3));
        Assert.False(_action.Do());
        InvalidateVisualCallNever();
    }

    [Test]
    public void NoSelection()
    {
        SetText("text\r\n123");
        MoveCursorTo(new(0, 0));

        _action.Do();
        Assert.AreEqual("\r\n123", _text.ToString());
        AssertCursorPosition(new(0, 0));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\n123", _text.ToString());
        AssertCursorPosition(new(0, 0));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("\r\n123", _text.ToString());
        AssertCursorPosition(new(0, 0));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void NoSelection_DeleteReturn()
    {
        SetText("text\r\n123");
        MoveCursorTo(new(0, 4));

        _action.Do();
        Assert.AreEqual("text123", _text.ToString());
        AssertCursorPosition(new(0, 4));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\n123", _text.ToString());
        AssertCursorPosition(new(0, 4));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("text123", _text.ToString());
        AssertCursorPosition(new(0, 4));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection()
    {
        SetText("text\r\n123");
        MoveCursorTo(new(0, 3));
        ActivateSelection();
        MoveCursorTo(new(1, 1));
        CompleteSelection();

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
        SetText("    text\r\n\r\n123");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));

        _action.Do();
        Assert.AreEqual("    text\r\n    123", _text.ToString());
        AssertCursorPosition(new(1, 4));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    text\r\n\r\n123", _text.ToString());
        AssertCursorPosition(new(1, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    text\r\n    123", _text.ToString());
        AssertCursorPosition(new(1, 4));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection_1_VirtualCursor()
    {
        SetText("    123\r\n\r\n456");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 3));
        CompleteSelection();

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
        SetText("    123\r\n\r\n456");
        MoveCursorTo(new(0, 7));
        ActivateSelection();
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        CompleteSelection();

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
        SetText("    123\r\n\r\n\r\n456");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();

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
