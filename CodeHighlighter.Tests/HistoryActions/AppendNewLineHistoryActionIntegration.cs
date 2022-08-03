using CodeHighlighter.HistoryActions;
using NUnit.Framework;

namespace CodeHighlighter.Tests.HistoryActions;

internal class AppendNewLineHistoryActionIntegration : BaseHistoryActionIntegration
{
    private AppendNewLineHistoryAction _action;

    [SetUp]
    public void Setup()
    {
        MakeContext();
        _context.InputModel.SetText(@"text");
        _action = new AppendNewLineHistoryAction(_context);
    }

    [Test]
    public void NoSelection()
    {
        _context.InputModel.MoveCursorTo(new(0, 1));
        MakeInactiveSelection();

        _action.Do();
        Assert.AreEqual("t\r\next", _text.ToString());

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text", _text.ToString());
        AssertCursorPosition(new(0, 1));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("t\r\next", _text.ToString());
    }

    [Test]
    public void WithSelection()
    {
        _context.InputModel.MoveCursorTo(new(1, 1));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(1, 2));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("t\r\nxt", _text.ToString());

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text", _text.ToString());
        AssertCursorPosition(new(0, 2));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("t\r\nxt", _text.ToString());
    }
}
