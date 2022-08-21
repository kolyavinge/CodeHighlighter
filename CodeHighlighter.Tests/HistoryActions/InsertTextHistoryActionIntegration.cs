using CodeHighlighter.HistoryActions;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.HistoryActions;

internal class InsertTextHistoryActionIntegrationIntegration : BaseHistoryActionIntegration
{
    private string _insertedText;
    private InsertTextHistoryAction _action;

    [SetUp]
    public void Setup()
    {
        _insertedText = "123";
        MakeContext();
        _action = new InsertTextHistoryAction(_context, _insertedText);
    }

    [Test]
    public void NoInsertion()
    {
        _context.InputModel.SetText("text\r\nfor 1");
        _action = new InsertTextHistoryAction(_context, "");
        Assert.False(_action.Do());
        InvalidateVisualCallNever();
    }

    [Test]
    public void NoSelection()
    {
        _context.InputModel.SetText("text\r\nfor 1");
        _context.InputModel.MoveCursorTo(new(0, 4));
        MakeInactiveSelection();

        _action.Do();
        Assert.AreEqual("text123\r\nfor 1", _text.ToString());
        AssertCursorPosition(new(0, 7));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\nfor 1", _text.ToString());
        AssertCursorPosition(new(0, 4));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("text123\r\nfor 1", _text.ToString());
        AssertCursorPosition(new(0, 7));

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
        Assert.AreEqual("text\r\n123 1", _text.ToString());
        AssertCursorPosition(new(1, 3));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\nfor 1", _text.ToString());
        AssertCursorPosition(new(1, 3));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("text\r\n123 1", _text.ToString());
        AssertCursorPosition(new(1, 3));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void NoSelection_VirtualCursor()
    {
        _context.InputModel.SetText("    text\r\n");
        _context.InputModel.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        MakeInactiveSelection();

        _action.Do();
        Assert.AreEqual("    text\r\n    123", _text.ToString());
        AssertCursorPosition(new(1, 7));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    text\r\n", _text.ToString());
        AssertCursorPosition(new(1, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    text\r\n    123", _text.ToString());
        AssertCursorPosition(new(1, 7));

        InvalidateVisualCallThreeTimes();
    }
}
