using CodeHighlighter.Model;
using CodeHighlighter.Tests.InputActions;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class InputModelIntegration : BaseInputActionIntegration
{
    [SetUp]
    public void Setup()
    {
        Init();
    }

    [Test]
    public void SetText_ResetCursor_1()
    {
        _model.SetText("123");
        MoveCursorTextEnd();

        _model.SetText("");

        Assert.AreEqual(new CursorPosition(0, 0), _model.TextCursor.Position);
    }

    [Test]
    public void SetText_ResetCursor_2()
    {
        _model.SetText("123");
        MoveCursorTextEnd();

        _model.SetText("1");

        Assert.AreEqual(new CursorPosition(0, 0), _model.TextCursor.Position);
    }

    [Test]
    public void SetSelectedTextCase()
    {
        SetText("AAA");
        SelectAll();
        var result = _model.SetSelectedTextCase(TextCase.Lower);

        Assert.AreEqual(new CursorPosition(0, 3), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 3), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 3), result.SelectionEnd);
        Assert.AreEqual("AAA", result.DeletedSelectedText);
        Assert.AreEqual("aaa", result.ChangedText);
        Assert.True(result.HasChanged);
        Assert.AreEqual("aaa", _model.Text.ToString());
    }

    [Test]
    public void SetSelectedTextCase_NoChanges()
    {
        SetText("AAA");
        MoveCursorStartLine();
        var result = _model.SetSelectedTextCase(TextCase.Lower);

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("", result.ChangedText);
        Assert.False(result.HasChanged);
        Assert.AreEqual("AAA", _model.Text.ToString());
    }
}
