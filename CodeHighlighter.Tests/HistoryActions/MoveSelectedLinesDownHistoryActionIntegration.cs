using CodeHighlighter.HistoryActions;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.HistoryActions;

internal class MoveSelectedLinesDownHistoryActionIntegration : BaseHistoryActionIntegration
{
    private MoveSelectedLinesDownHistoryAction _action;

    [SetUp]
    public void Setup()
    {
        MakeContext();
        _action = new MoveSelectedLinesDownHistoryAction(_context);
    }

    [Test]
    public void NoMove()
    {
        _context.InputModel.MoveCursorTo(new(2, 0));
        Assert.False(_action.Do());
        InvalidateVisualCallNever();
    }

    [Test]
    public void NoSelection()
    {
        _context.InputModel.SetText("000\r\n111\r\n222");
        _context.InputModel.MoveCursorTo(new(0, 0));

        _action.Do();
        Assert.AreEqual("111\r\n000\r\n222", _text.ToString());
        AssertCursorPosition(new(1, 0));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("000\r\n111\r\n222", _text.ToString());
        AssertCursorPosition(new(0, 0));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("111\r\n000\r\n222", _text.ToString());
        AssertCursorPosition(new(1, 0));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection()
    {
        _context.InputModel.SetText("000\r\n111\r\n222");
        _context.InputModel.MoveCursorTo(new(0, 3));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(1, 1));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("222\r\n000\r\n111", _text.ToString());
        AssertCursorPosition(new(2, 1));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("000\r\n111\r\n222", _text.ToString());
        AssertCursorPosition(new(1, 1));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("222\r\n000\r\n111", _text.ToString());
        Assert.AreEqual(new CursorPosition(1, 3), _context.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(2, 1), _context.TextSelection.EndPosition);

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void NoSelection_VirtualCursor()
    {
        _context.InputModel.SetText("    000\r\n\r\n111");
        _context.InputModel.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));

        _action.Do();
        Assert.AreEqual("    000\r\n111\r\n", _text.ToString());
        AssertCursorPosition(new(2, 0));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    000\r\n\r\n111", _text.ToString());
        AssertCursorPosition(new(1, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    000\r\n111\r\n", _text.ToString());
        AssertCursorPosition(new(2, 0));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void NoSelection_2_VirtualCursor()
    {
        _context.InputModel.SetText("    000\r\n\r\n");
        _context.InputModel.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));

        _action.Do();
        Assert.AreEqual("    000\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    000\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(1, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    000\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 4, CursorPositionKind.Virtual));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection_1_VirtualCursor()
    {
        _context.InputModel.SetText("    012\r\n\r\n\r\n");
        _context.InputModel.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(2, 0));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("    012\r\n\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(3, 0));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    012\r\n\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 0));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    012\r\n\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(3, 0));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection_2_VirtualCursor()
    {
        _context.InputModel.SetText("\r\n    012\r\n\r\n\r\n");
        _context.InputModel.MoveCursorTo(new(1, 7));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("\r\n\r\n    012\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(3, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("\r\n    012\r\n\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("\r\n\r\n    012\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(3, 4, CursorPositionKind.Virtual));

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection_3_VirtualCursor()
    {
        _context.InputModel.SetText("    012\r\n\r\n\r\n\r\n");
        _context.InputModel.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("    012\r\n\r\n\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(3, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("    012\r\n\r\n\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(2, 4, CursorPositionKind.Virtual));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("    012\r\n\r\n\r\n\r\n", _text.ToString());
        AssertCursorPosition(new(3, 4, CursorPositionKind.Virtual));

        InvalidateVisualCallThreeTimes();
    }
}
