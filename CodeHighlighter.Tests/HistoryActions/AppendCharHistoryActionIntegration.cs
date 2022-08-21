using CodeHighlighter.HistoryActions;
using CodeHighlighter.Model;
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
        _action = new AppendCharHistoryAction(_context, _appendedChar);
    }

    [Test]
    public void NoSelection()
    {
        _context.InputModel.SetText("text\r\nfor 1");
        _context.InputModel.MoveCursorTo(new(0, 4));
        MakeInactiveSelection();

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
        _context.InputModel.SetText("text\r\nfor 1");
        _context.InputModel.MoveCursorTo(new(1, 0));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(1, 3));
        _context.InputModel.CompleteSelection();

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
        _context.InputModel.SetText("    text\r\n");
        _context.InputModel.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        MakeInactiveSelection();

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
}
