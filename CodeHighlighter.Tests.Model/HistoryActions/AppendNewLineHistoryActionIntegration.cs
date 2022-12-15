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
        _action = new AppendNewLineHistoryAction(_inputActionsFactory, _context);
    }

    [Test]
    public void NoSelection()
    {
        SetText("text");
        MoveCursorTo(new(0, 1));

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
        SetText("text");
        MoveCursorTo(new(1, 1));
        ActivateSelection();
        MoveCursorTo(new(1, 2));
        CompleteSelection();

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
        SetText("text\r\n123\r\n456");
        MoveCursorTo(new(1, 0));
        ActivateSelection();
        MoveCursorTo(new(2, 3));
        CompleteSelection();

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
        SetText("    text");
        MoveCursorTo(new(0, 8));

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
        SetText("    text\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));

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
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 0));
        CompleteSelection();

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
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(0, 7));
        ActivateSelection();
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        CompleteSelection();

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
        SetText("    012\r\n\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();

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
