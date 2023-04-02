using CodeHighlighter.Core;
using CodeHighlighter.HistoryActions;
using NUnit.Framework;

namespace CodeHighlighter.Tests.HistoryActions;

internal class DeleteSelectedLinesHistoryActionIntegration : BaseHistoryActionIntegration
{
    private DeleteSelectedLinesHistoryAction _action;

    [SetUp]
    public void Setup()
    {
        MakeContext();
        _action = new DeleteSelectedLinesHistoryAction(_inputActionsFactory, _context);
    }

    [Test]
    public void NoDelete()
    {
        SetText("");
        MoveCursorTo(new(0, 0));
        Assert.False(_action.Do());
        InvalidateVisualCallNever();
    }

    [Test]
    public void NoSelection()
    {
        SetText("text\r\nfor 1\r\n123");
        MoveCursorTo(new(0, 3));

        _action.Do();
        Assert.AreEqual("for 1\r\n123", _text.ToString());
        AssertCursorPosition(new(0, 0));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\nfor 1\r\n123", _text.ToString());
        AssertCursorPosition(new(0, 3));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("for 1\r\n123", _text.ToString());
        AssertCursorPosition(new(0, 0));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void NoSelection_DeleteLastLine()
    {
        SetText("text\r\nfor 1\r\n123");
        MoveCursorTo(new(2, 0));

        _action.Do();
        Assert.AreEqual("text\r\nfor 1\r\n", _text.ToString());
        AssertCursorPosition(new(2, 0));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\nfor 1\r\n123", _text.ToString());
        AssertCursorPosition(new(2, 0));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("text\r\nfor 1\r\n", _text.ToString());
        AssertCursorPosition(new(2, 0));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection()
    {
        SetText("text\r\nfor 1\r\n123");
        MoveCursorTo(new(1, 0));
        ActivateSelection();
        MoveCursorTo(new(2, 3));
        CompleteSelection();

        _action.Do();
        Assert.AreEqual("text\r\n", _text.ToString());
        AssertCursorPosition(new(1, 0));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\nfor 1\r\n123", _text.ToString());
        AssertCursorPosition(new(2, 3));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("text\r\n", _text.ToString());
        AssertCursorPosition(new(1, 0));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection_Middle()
    {
        SetText("text\r\nfor 1\r\n123\r\n456\r\n789");
        MoveCursorTo(new(1, 0));
        ActivateSelection();
        MoveCursorTo(new(2, 3));
        CompleteSelection();

        _action.Do();
        Assert.AreEqual("text\r\n456\r\n789", _text.ToString());
        AssertCursorPosition(new(1, 0));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\nfor 1\r\n123\r\n456\r\n789", _text.ToString());
        AssertCursorPosition(new(2, 3));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("text\r\n456\r\n789", _text.ToString());
        AssertCursorPosition(new(1, 0));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void NoSelection_VirtualCursor()
    {
        SetText("    text\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));

        _action.Do();
        Assert.AreEqual("    text\r\n", _text.ToString());
        AssertCursorPosition(new(1, 0));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    text\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(1, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    text\r\n", _text.ToString());
        AssertCursorPosition(new(1, 0));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void NoSelection_DeleteLastLine_VirtualCursor()
    {
        SetText("    text\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));

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
    public void DeleteSelectedLines_WithSelection_1_VirtualCursor()
    {
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 0));
        CompleteSelection();

        _action.Do();
        Assert.AreEqual("    012\r\n", _text.ToString());
        AssertCursorPosition(new(1, 0));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    012\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 0));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    012\r\n", _text.ToString());
        AssertCursorPosition(new(1, 0));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void DeleteSelectedLines_WithSelection_2_VirtualCursor()
    {
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(0, 7));
        ActivateSelection();
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        CompleteSelection();

        _action.Do();
        Assert.AreEqual("", _text.ToString());
        AssertCursorPosition(new(0, 0));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    012\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(1, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("", _text.ToString());
        AssertCursorPosition(new(0, 0));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void DeleteSelectedLines_WithSelection_3_VirtualCursor()
    {
        SetText("    012\r\n\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();

        _action.Do();
        Assert.AreEqual("    012\r\n", _text.ToString());
        AssertCursorPosition(new(1, 0));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    012\r\n\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    012\r\n", _text.ToString());
        AssertCursorPosition(new(1, 0));

        InvalidateVisualCallThreeTimes();
    }
}
