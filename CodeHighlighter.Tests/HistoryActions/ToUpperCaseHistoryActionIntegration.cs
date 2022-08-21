using CodeHighlighter.HistoryActions;
using NUnit.Framework;

namespace CodeHighlighter.Tests.HistoryActions;

internal class ToUpperCaseHistoryActionIntegration : BaseHistoryActionIntegration
{
    private ToUpperCaseHistoryAction _action;

    [SetUp]
    public void Setup()
    {
        MakeContext();
        _context.InputModel.SetText("teXt\r\na");
        _action = new ToUpperCaseHistoryAction(_context);
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
        Assert.AreEqual("teXT\r\nA", _text.ToString());
        AssertCursorPosition(new(1, 1));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("teXt\r\na", _text.ToString());
        AssertCursorPosition(new(1, 1));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("teXT\r\nA", _text.ToString());
        AssertCursorPosition(new(1, 0));

        InvalidateVisualCallThreeTimes();
    }
}
