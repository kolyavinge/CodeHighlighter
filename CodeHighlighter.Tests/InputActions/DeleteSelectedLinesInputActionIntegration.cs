using CodeHighlighter.InputActions;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.InputActions;

internal class DeleteSelectedLinesInputActionIntegration : BaseInputActionIntegration
{
    [SetUp]
    public void Setup()
    {
        Init();
    }

    [Test]
    public void DeleteSelectedLines()
    {
        SetText("000\r\n111\r\n222\r\n");
        MoveCursorTo(new(1, 1));
        var result = DeleteSelectedLinesInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(1, 1), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 1), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 1), result.SelectionEnd);
        Assert.AreEqual("111\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("000\r\n222\r\n", _model.Text.ToString());
    }

    [Test]
    public void DeleteSelectedLines_NoDeletion()
    {
        var result = DeleteSelectedLinesInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.False(result.HasDeleted);
        Assert.AreEqual("", _model.Text.ToString());
    }

    [Test]
    public void DeleteSelectedLines_EmptyLine()
    {
        SetText("000");
        AppendNewLine();
        AppendNewLine();
        MoveCursorTo(new(1, 0));
        var result = DeleteSelectedLinesInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(1, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 0), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("\r\n000", _model.Text.ToString());
    }

    [Test]
    public void DeleteSelectedLines_WithSelection()
    {
        SetText("000\r\n111\r\n222\r\n");
        MoveCursorTo(new(0, 1));
        ActivateSelection();
        MoveCursorTo(new(1, 1));
        CompleteSelection();
        var result = DeleteSelectedLinesInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(1, 1), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 1), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 1), result.SelectionEnd);
        Assert.AreEqual("000\r\n111\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("222\r\n", _model.Text.ToString());
    }

    [Test]
    public void DeleteSelectedLines_LastLine()
    {
        SetText("000\r\n111\r\n222");
        MoveCursorTo(new(2, 1));
        var result = DeleteSelectedLinesInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(2, 1), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 1), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 1), result.SelectionEnd);
        Assert.AreEqual("222", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("000\r\n111\r\n", _model.Text.ToString());
    }

    [Test]
    public void DeleteSelectedLines_WithSelection_1_VirtualCursor()
    {
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 0));
        CompleteSelection();
        var result = DeleteSelectedLinesInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(2, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 0), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n", _model.Text.ToString());
    }

    [Test]
    public void DeleteSelectedLines_WithSelection_2_VirtualCursor()
    {
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(0, 7));
        ActivateSelection();
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = DeleteSelectedLinesInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("    012\r\n\r\n", result.DeletedSelectedText);
        Assert.AreEqual("", _model.Text.ToString());
    }

    [Test]
    public void DeleteSelectedLines_WithSelection_3_VirtualCursor()
    {
        SetText("    012\r\n\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = DeleteSelectedLinesInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n", _model.Text.ToString());
    }
}
