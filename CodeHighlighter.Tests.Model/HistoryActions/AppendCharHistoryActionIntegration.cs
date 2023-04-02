using CodeHighlighter.Core;
using CodeHighlighter.HistoryActions;
using NUnit.Framework;

namespace CodeHighlighter.Tests.HistoryActions;

internal class AppendCharHistoryActionIntegration : BaseHistoryActionIntegration
{
    private char _appendedChar;
    private AppendCharHistoryAction _action;

    [SetUp]
    public void Setup()
    {
        _appendedChar = 'A';
        MakeContext();
        _action = new AppendCharHistoryAction(_inputActionsFactory, _context);
        _action.SetParams(_appendedChar);
    }

    [Test]
    public void NoSelection()
    {
        SetText("text\r\nfor 1");
        MoveCursorTo(new(0, 4));

        _action.Do();
        Assert.AreEqual("textA\r\nfor 1", _text.ToString());
        AssertCursorPosition(new(0, 5));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\nfor 1", _text.ToString());
        AssertCursorPosition(new(0, 4));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("textA\r\nfor 1", _text.ToString());
        AssertCursorPosition(new(0, 5));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection()
    {
        SetText("text\r\nfor 1");
        MoveCursorTo(new(1, 0));
        ActivateSelection();
        MoveCursorTo(new(1, 3));
        CompleteSelection();

        _action.Do();
        Assert.AreEqual("text\r\nA 1", _text.ToString());
        AssertCursorPosition(new(1, 1));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\nfor 1", _text.ToString());
        AssertCursorPosition(new(1, 3));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("text\r\nA 1", _text.ToString());
        AssertCursorPosition(new(1, 1));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void NoSelection_VirtualCursor()
    {
        SetText("    text\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));

        _action.Do();
        Assert.AreEqual("    text\r\n    A", _text.ToString());
        AssertCursorPosition(new(1, 5));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    text\r\n", _text.ToString());
        AssertCursorPosition(new(1, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    text\r\n    A", _text.ToString());
        AssertCursorPosition(new(1, 5));

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
        Assert.AreEqual("    012\r\n    A", _text.ToString());
        AssertCursorPosition(new(1, 5));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    012\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 0));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    012\r\n    A", _text.ToString());
        AssertCursorPosition(new(1, 5));

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
        Assert.AreEqual("    012A\r\n", _text.ToString());
        AssertCursorPosition(new(0, 8));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    012\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(1, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    012A\r\n", _text.ToString());
        AssertCursorPosition(new(0, 8));

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
        Assert.AreEqual("    012\r\n    A\r\n", _text.ToString());
        AssertCursorPosition(new(1, 5));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    012\r\n\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    012\r\n    A\r\n", _text.ToString());
        AssertCursorPosition(new(1, 5));

        InvalidateVisualCallThreeTimes();
    }
}
