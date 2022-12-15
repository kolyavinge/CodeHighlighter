using CodeHighlighter.InputActions;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.InputActions;

internal class LeftDeleteInputActionIntegration : BaseInputActionIntegration
{
    private LeftDeleteInputAction _action;

    [SetUp]
    public void Setup()
    {
        Init();
        _action = new LeftDeleteInputAction();
    }

    [Test]
    public void LeftDelete()
    {
        SetText("0");
        MoveCursorTo(new(0, 1));
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(0, 1), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 1), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 1), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual('0', result.CharCharDeleteResult.DeletedChar);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("", _text.ToString());
    }

    [Test]
    public void LeftDelete_WithSelection()
    {
        SetText("0123456789\r\n9876543210");
        MoveCursorTo(new(0, 3));
        ActivateSelection();
        MoveCursorTo(new(1, 7));
        CompleteSelection();
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(1, 7), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 3), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 3), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 7), result.SelectionEnd);
        Assert.AreEqual("3456789\r\n9876543", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("012210", _text.ToString());
    }

    [Test]
    public void LeftDelete_WithSelection_DeleteEmptyLine()
    {
        SetText("0123456789\r\n9876543210");
        MoveCursorTo(new(0, 10));
        ActivateSelection();
        MoveCursorTo(new(1, 0));
        CompleteSelection();
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(1, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 10), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 10), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 0), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("01234567899876543210", _text.ToString());
    }

    [Test]
    public void LeftDelete_VirtualCursor()
    {
        SetText("    123\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.False(result.HasDeleted);
        Assert.AreEqual("    123\r\n", _text.ToString());
    }

    [Test]
    public void LeftDelete_WithSelection_1_VirtualCursor()
    {
        SetText("    123\r\n\r\n456");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 3));
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(2, 3), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 3), result.SelectionEnd);
        Assert.AreEqual("\r\n456", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n    ", _text.ToString());
    }

    [Test]
    public void LeftDelete_WithSelection_2_VirtualCursor()
    {
        SetText("    123\r\n\r\n456");
        MoveCursorTo(new(0, 7));
        ActivateSelection();
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n456", _text.ToString());
    }

    [Test]
    public void LeftDelete_WithSelection_3_VirtualCursor()
    {
        SetText("    123\r\n\r\n\r\n456");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n    \r\n456", _text.ToString());
    }
}
