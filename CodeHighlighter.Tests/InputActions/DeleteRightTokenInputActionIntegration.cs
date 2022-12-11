using CodeHighlighter.InputActions;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.InputActions;

internal class DeleteRightTokenInputActionIntegration : BaseInputActionIntegration
{
    [SetUp]
    public void Setup()
    {
        Init();
    }

    [Test]
    public void DeleteRightToken()
    {
        SetText("SELECT FROM");
        MoveCursorTo(new(0, 7));
        var result = DeleteRightTokenInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(0, 7), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 11), result.SelectionEnd);
        Assert.AreEqual("FROM", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("SELECT ", _text.ToString());
    }

    [Test]
    public void DeleteRightToken_NoDelection()
    {
        SetText("SELECT FROM");
        MoveCursorEndLine();
        var result = DeleteRightTokenInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(0, 11), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 11), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 11), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 11), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.False(result.HasDeleted);
        Assert.AreEqual("SELECT FROM", _text.ToString());
    }

    [Test]
    public void DeleteRightToken_WithSelection()
    {
        SetText("SELECT FROM");
        MoveCursorStartLine();
        ActivateSelection();
        MoveCursorEndLine();
        CompleteSelection();
        var result = DeleteRightTokenInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(0, 11), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 11), result.SelectionEnd);
        Assert.AreEqual("SELECT FROM", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("", _text.ToString());
    }

    [Test]
    public void DeleteRightToken_VirtualCursor()
    {
        SetText("    SELECT FROM\r\n\r\nSELECT FROM");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        var result = DeleteRightTokenInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 0), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    SELECT FROM\r\n    SELECT FROM", _text.ToString());
    }

    [Test]
    public void DeleteRightToken_WithSelection_1_VirtualCursor()
    {
        SetText("    123\r\n\r\n456");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 3));
        CompleteSelection();
        var result = DeleteRightTokenInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(2, 3), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 3), result.SelectionEnd);
        Assert.AreEqual("\r\n456", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n    ", _text.ToString());
    }

    [Test]
    public void DeleteRightToken_WithSelection_2_VirtualCursor()
    {
        SetText("    123\r\n\r\n456");
        MoveCursorTo(new(0, 7));
        ActivateSelection();
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = DeleteRightTokenInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n456", _text.ToString());
    }

    [Test]
    public void DeleteRightToken_WithSelection_3_VirtualCursor()
    {
        SetText("    123\r\n\r\n\r\n456");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = DeleteRightTokenInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n    \r\n456", _text.ToString());
    }
}
