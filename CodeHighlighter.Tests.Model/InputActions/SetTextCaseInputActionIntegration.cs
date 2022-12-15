using CodeHighlighter.InputActions;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.InputActions;

internal class SetTextCaseInputActionIntegration : BaseInputActionIntegration
{
    private SetTextCaseInputAction _action;

    [SetUp]
    public void Setup()
    {
        Init();
        _action = new SetTextCaseInputAction();
    }

    [Test]
    public void SetSelectedTextCase()
    {
        SetText("AAA");
        SelectAll();
        var result = _action.Do(_context, TextCase.Lower);

        Assert.AreEqual(new CursorPosition(0, 3), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 3), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 3), result.SelectionEnd);
        Assert.AreEqual("AAA", result.DeletedSelectedText);
        Assert.AreEqual("aaa", result.ChangedText);
        Assert.True(result.HasChanged);
        Assert.AreEqual("aaa", _text.ToString());
    }

    [Test]
    public void SetSelectedTextCase_NoChanges()
    {
        SetText("AAA");
        MoveCursorStartLine();
        var result = _action.Do(_context, TextCase.Lower);

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("", result.ChangedText);
        Assert.False(result.HasChanged);
        Assert.AreEqual("AAA", _text.ToString());
    }
}
