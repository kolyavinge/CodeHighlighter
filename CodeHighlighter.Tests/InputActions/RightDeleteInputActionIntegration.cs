using CodeHighlighter.InputActions;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.InputActions;

internal class RightDeleteInputActionIntegration : BaseInputActionIntegration
{
    [SetUp]
    public void Setup()
    {
        Init();
    }

    [Test]
    public void RightDelete()
    {
        SetText("0");
        MoveCursorTo(new(0, 0));
        var result = RightDeleteInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual('0', result.CharCharDeleteResult.DeletedChar);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("", _model.Text.ToString());
    }

    [Test]
    public void RightDelete_WithSelection()
    {
        SetText("0123456789\r\n9876543210");
        MoveCursorTo(new(0, 3));
        ActivateSelection();
        MoveCursorTo(new(1, 7));
        CompleteSelection();
        var result = RightDeleteInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(1, 7), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 3), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 3), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 7), result.SelectionEnd);
        Assert.AreEqual("3456789\r\n9876543", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("012210", _model.Text.ToString());
    }

    [Test]
    public void RightDelete_WithSelection_DeleteEmptyLine()
    {
        SetText("0123456789\r\n9876543210");
        MoveCursorTo(new(0, 10));
        ActivateSelection();
        MoveCursorTo(new(1, 0));
        CompleteSelection();
        var result = RightDeleteInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(1, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 10), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 10), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 0), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("01234567899876543210", _model.Text.ToString());
    }

    [Test]
    public void RightDelete_VirtualCursor()
    {
        SetText("    123\r\n\r\n456");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        var result = RightDeleteInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n    456", _model.Text.ToString());
    }

    [Test]
    public void RightDelete_WithSelection_1_VirtualCursor()
    {
        SetText("    123\r\n\r\n456");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 3));
        var result = RightDeleteInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(2, 3), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 3), result.SelectionEnd);
        Assert.AreEqual("\r\n456", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n    ", _model.Text.ToString());
    }

    [Test]
    public void RightDelete_WithSelection_2_VirtualCursor()
    {
        SetText("    123\r\n\r\n456");
        MoveCursorTo(new(0, 7));
        ActivateSelection();
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        var result = RightDeleteInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n456", _model.Text.ToString());
    }

    [Test]
    public void RightDelete_WithSelection_3_VirtualCursor()
    {
        SetText("    123\r\n\r\n\r\n456");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        var result = RightDeleteInputAction.Instance.Do(_context);

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n    \r\n456", _model.Text.ToString());
    }
}
