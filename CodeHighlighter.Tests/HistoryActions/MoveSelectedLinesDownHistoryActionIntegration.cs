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
        _context.InputModel.SetText("000\r\n111\r\n222");
        _action = new MoveSelectedLinesDownHistoryAction(_context);
    }

    [Test]
    public void NoMove()
    {
        _context.InputModel.MoveCursorTo(new(2, 0));
        Assert.False(_action.Do());
    }

    [Test]
    public void NoSelection()
    {
        _context.InputModel.MoveCursorTo(new(0, 0));
        MakeInactiveSelection();

        _action.Do();
        Assert.AreEqual("111\r\n000\r\n222", _text.ToString());

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("000\r\n111\r\n222", _text.ToString());
        AssertCursorPosition(new(0, 0));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("111\r\n000\r\n222", _text.ToString());
    }

    [Test]
    public void WithSelection()
    {
        _context.InputModel.MoveCursorTo(new(0, 3));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(1, 1));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("222\r\n000\r\n111", _text.ToString());

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("000\r\n111\r\n222", _text.ToString());
        AssertCursorPosition(new(1, 1));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("222\r\n000\r\n111", _text.ToString());
        Assert.AreEqual(new CursorPosition(1, 3), _context.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(2, 1), _context.TextSelection.EndPosition);
    }
}
