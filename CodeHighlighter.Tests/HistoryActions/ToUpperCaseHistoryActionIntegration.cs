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
        SetText("teXt\r\na");
        _action = new ToUpperCaseHistoryAction(_context);
    }

    [Test]
    public void NoChanges()
    {
        MoveCursorStartLine();
        Assert.False(_action.Do());
        InvalidateVisualCallNever();
    }

    [Test]
    public void WithSelection()
    {
        MoveCursorTo(new(0, 2));
        ActivateSelection();
        MoveCursorTo(new(1, 1));
        CompleteSelection();

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
