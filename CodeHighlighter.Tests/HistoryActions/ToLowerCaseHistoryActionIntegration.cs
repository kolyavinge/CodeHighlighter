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
        SetText("TExT\r\nA");
        _action = new ToLowerCaseHistoryAction(_context);
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
        Assert.AreEqual("TExt\r\na", _text.ToString());
        AssertCursorPosition(new(1, 1));

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("TExT\r\nA", _text.ToString());
        AssertCursorPosition(new(1, 1));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("TExt\r\na", _text.ToString());
        AssertCursorPosition(new(1, 0));

        InvalidateVisualCallThreeTimes();
    }
}
