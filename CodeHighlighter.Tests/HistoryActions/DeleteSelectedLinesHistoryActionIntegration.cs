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
        _context.InputModel.SetText("text\r\nfor 1\r\n123");
        _action = new DeleteSelectedLinesHistoryAction(_context);
    }

    [Test]
    public void NoDelete()
    {
        _context.InputModel.SetText("");
        Assert.False(_action.Do());
    }

    [Test]
    public void NoSelection()
    {
        _context.InputModel.MoveCursorTo(new(0, 3));
        MakeInactiveSelection();

        _action.Do();
        Assert.AreEqual("for 1\r\n123", _text.ToString());

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\nfor 1\r\n123", _text.ToString());
        AssertCursorPosition(new(0, 3));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("for 1\r\n123", _text.ToString());
    }

    [Test]
    public void NoSelection_DeleteLastLine()
    {
        _context.InputModel.MoveCursorTo(new(2, 0));
        MakeInactiveSelection();

        _action.Do();
        Assert.AreEqual("text\r\nfor 1\r\n", _text.ToString());

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\nfor 1\r\n123", _text.ToString());
        AssertCursorPosition(new(2, 0));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("text\r\nfor 1\r\n", _text.ToString());
    }

    [Test]
    public void WithSelection()
    {
        _context.InputModel.MoveCursorTo(new(1, 0));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(2, 3));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("text\r\n", _text.ToString());

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\nfor 1\r\n123", _text.ToString());
        AssertCursorPosition(new(2, 3));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("text\r\n", _text.ToString());
    }

    [Test]
    public void WithSelection_Middle()
    {
        _context.InputModel.SetText("text\r\nfor 1\r\n123\r\n456\r\n789");
        _context.InputModel.MoveCursorTo(new(1, 0));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(2, 3));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("text\r\n456\r\n789", _text.ToString());

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\nfor 1\r\n123\r\n456\r\n789", _text.ToString());
        AssertCursorPosition(new(2, 3));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("text\r\n456\r\n789", _text.ToString());
    }
}
