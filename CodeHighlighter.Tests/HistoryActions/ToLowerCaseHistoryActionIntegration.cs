using CodeHighlighter.HistoryActions;
using NUnit.Framework;

namespace CodeHighlighter.Tests.HistoryActions;

internal class ToLowerCaseHistoryActionIntegration : BaseHistoryActionIntegration
{
    private ToLowerCaseHistoryAction _action;

    [SetUp]
    public void Setup()
    {
        MakeContext();
        _context.InputModel.SetText("TExT\r\nA");
        _action = new ToLowerCaseHistoryAction(_context);
    }

    [Test]
    public void NoChanges()
    {
        _context.InputModel.MoveCursorStartLine();
        Assert.False(_action.Do());
        InvalidateVisualCallNever();
    }

    [Test]
    public void WithSelection()
    {
        _context.InputModel.MoveCursorTo(new(0, 2));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(1, 1));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("TExt\r\na", _text.ToString());

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("TExT\r\nA", _text.ToString());
        AssertCursorPosition(new(1, 1));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("TExt\r\na", _text.ToString());

        InvalidateVisualCallThreeTimes();
    }
}
