using CodeHighlighter.Core;
using CodeHighlighter.InputActions;
using NUnit.Framework;

namespace CodeHighlighter.Tests.InputActions;

internal class MoveSelectedLinesDownInputActionIntegration : BaseInputActionIntegration
{
    private MoveSelectedLinesDownInputAction _action;

    [SetUp]
    public void Setup()
    {
        Init();
        _action = new MoveSelectedLinesDownInputAction();
    }

    [Test]
    public void MoveSelectedLinesDown()
    {
        SetText("000\r\n111\r\n222\r\n");
        MoveCursorTo(new(1, 1));
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(1, 1), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 1), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 1), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.True(result.HasMoved);
        Assert.AreEqual("000\r\n222\r\n111\r\n", _text.ToString());
    }

    [Test]
    public void MoveSelectedLinesDown_NoMoving()
    {
        SetText("000");
        MoveCursorTo(new(0, 0));
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.False(result.HasMoved);
        Assert.AreEqual("000", _text.ToString());
    }

    [Test]
    public void MoveSelectedLinesDown_WithSelection()
    {
        SetText("000\r\n111\r\n222\r\n");
        MoveCursorTo(new(0, 1));
        ActivateSelection();
        MoveCursorTo(new(1, 1));
        CompleteSelection();
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(1, 1), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 1), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 1), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.True(result.HasMoved);
        Assert.AreEqual("222\r\n000\r\n111\r\n", _text.ToString());
    }

    [Test]
    public void MoveSelectedLinesDown_WithSelection_1_VirtualCursor()
    {
        SetText("    012\r\n\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 0));
        CompleteSelection();
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(2, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(3, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n\r\n\r\n", _text.ToString());
    }

    [Test]
    public void MoveSelectedLinesDown_WithSelection_2_VirtualCursor()
    {
        SetText("\r\n    012\r\n\r\n\r\n");
        MoveCursorTo(new(1, 7));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(3, 4, CursorPositionKind.Virtual), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("\r\n\r\n    012\r\n\r\n", _text.ToString());
    }

    [Test]
    public void MoveSelectedLinesDown_WithSelection_3_VirtualCursor()
    {
        SetText("    012\r\n\r\n\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(3, 4, CursorPositionKind.Virtual), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n\r\n\r\n\r\n", _text.ToString());
    }
}
