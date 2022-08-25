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

    [Test]
    public void WithSelection_1_VirtualCursor()
    {
        _context.InputModel.SetText("    012\r\n\r\n");
        _context.InputModel.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(2, 0));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("    012\r\n    123", _text.ToString());
        AssertCursorPosition(new(1, 7));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    012\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 0));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    012\r\n    123", _text.ToString());
        AssertCursorPosition(new(1, 7));

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
        Assert.AreEqual("    012123\r\n", _text.ToString());
        AssertCursorPosition(new(0, 10));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    012\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(1, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    012123\r\n", _text.ToString());
        AssertCursorPosition(new(0, 10));

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
        Assert.AreEqual("    012\r\n    123\r\n", _text.ToString());
        AssertCursorPosition(new(1, 7));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    012\r\n\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    012\r\n    123\r\n", _text.ToString());
        AssertCursorPosition(new(1, 7));

        InvalidateVisualCallThreeTimes();
    }
}
