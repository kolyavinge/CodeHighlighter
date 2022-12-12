using CodeHighlighter.InputActions;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.InputActions;

internal class AppendCharInputActionIntegration : BaseInputActionIntegration
{
    [SetUp]
    public void Setup()
    {
        Init();
    }

    [Test]
    public void AppendChar()
    {
        var result = AppendCharInputAction.Instance.Do(_context, '0');

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual('0', result.AppendedChar);
        Assert.AreEqual("0", _text.ToString());
    }

    [Test]
    public void AppendChar_WithSelection_1()
    {
        SetText("000");
        MoveCursorStartLine();
        ActivateSelection();
        MoveCursorEndLine();
        CompleteSelection();
        var result = AppendCharInputAction.Instance.Do(_context, '0');

        Assert.AreEqual(new CursorPosition(0, 3), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 3), result.SelectionEnd);
        Assert.AreEqual("000", result.DeletedSelectedText);
        Assert.AreEqual('0', result.AppendedChar);
        Assert.AreEqual("0", _text.ToString());
    }

    [Test]
    public void AppendChar_WithSelection_2()
    {
        SetText("000");
        MoveCursorEndLine();
        ActivateSelection();
        MoveCursorStartLine();
        CompleteSelection();
        var result = AppendCharInputAction.Instance.Do(_context, '0');

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 3), result.SelectionEnd);
        Assert.AreEqual("000", result.DeletedSelectedText);
        Assert.AreEqual('0', result.AppendedChar);
        Assert.AreEqual("0", _text.ToString());
    }

    [Test]
    public void AppendChar_VirtualCursor()
    {
        SetText("    012\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        var result = AppendCharInputAction.Instance.Do(_context, '9');

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 5), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n    9", _text.ToString());
    }

    [Test]
    public void AppendChar_VirtualCursor_2()
    {
        SetText("    012");
        MoveCursorEndLine();
        AppendNewLine();
        var result = AppendCharInputAction.Instance.Do(_context, '9');

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 5), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n    9", _text.ToString());
    }

    [Test]
    public void AppendChar_WithSelection_1_VirtualCursor()
    {
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 0));
        CompleteSelection();
        var result = AppendCharInputAction.Instance.Do(_context, '9');

        Assert.AreEqual(new CursorPosition(2, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 5), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 0), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n    9", _text.ToString());
    }

    [Test]
    public void AppendChar_WithSelection_2_VirtualCursor()
    {
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(0, 7));
        ActivateSelection();
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = AppendCharInputAction.Instance.Do(_context, '9');

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 8), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    0129\r\n", _text.ToString());
    }

    [Test]
    public void AppendChar_WithSelection_3_VirtualCursor()
    {
        SetText("    012\r\n\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = AppendCharInputAction.Instance.Do(_context, '9');

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 5), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n    9\r\n", _text.ToString());
    }
}
