using CodeHighlighter.Core;
using CodeHighlighter.InputActions;
using NUnit.Framework;

namespace CodeHighlighter.Tests.InputActions;

internal class AppendNewLineInputActionIntegration : BaseInputActionIntegration
{
    private AppendNewLineInputAction _action;

    [SetUp]
    public void Setup()
    {
        Init();
        _action = new AppendNewLineInputAction();
    }

    [Test]
    public void AppendNewLine_NoSelection()
    {
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("\r\n", _text.ToString());
    }

    [Test]
    public void AppendNewLine_WithSelection()
    {
        SetText("0123456789");
        MoveCursorTo(new(0, 5));
        ActivateSelection();
        MoveCursorTo(new(0, 7));
        CompleteSelection();
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(0, 7), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 5), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionEnd);
        Assert.AreEqual("56", result.DeletedSelectedText);
        Assert.AreEqual("01234\r\n789", _text.ToString());
    }

    [Test]
    public void AppendNewLine_VirtualCursor()
    {
        SetText("    012\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n\r\n", _text.ToString());
    }

    [Test]
    public void AppendNewLine_WithSelection_1_VirtualCursor()
    {
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 0));
        CompleteSelection();
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(2, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 0), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n\r\n", _text.ToString());
    }

    [Test]
    public void AppendNewLine_WithSelection_2_VirtualCursor()
    {
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(0, 7));
        ActivateSelection();
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n\r\n", _text.ToString());
    }

    [Test]
    public void AppendNewLine_WithSelection_3_VirtualCursor()
    {
        SetText("    012\r\n\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = _action.Do(_context);

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n\r\n\r\n", _text.ToString());
    }
}
