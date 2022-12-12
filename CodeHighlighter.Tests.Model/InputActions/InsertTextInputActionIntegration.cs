using CodeHighlighter.InputActions;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.InputActions;

internal class InsertTextInputActionIntegration : BaseInputActionIntegration
{
    [SetUp]
    public void Setup()
    {
        Init();
    }

    [Test]
    public void InsertText()
    {
        var result = InsertTextInputAction.Instance.Do(_context, "XXX\nYYY\nZZZ");

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 3), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual(new CursorPosition(0, 0), result.InsertStartPosition);
        Assert.AreEqual(new CursorPosition(2, 3), result.InsertEndPosition);
        Assert.AreEqual("XXX\nYYY\nZZZ", result.InsertedText);
        Assert.True(result.HasInserted);
        Assert.AreEqual("XXX\r\nYYY\r\nZZZ", _text.ToString());
    }

    [Test]
    public void InsertText_WithSelection()
    {
        SetText("0123456789");
        MoveCursorTo(new(0, 5));
        ActivateSelection();
        MoveCursorTo(new(0, 7));
        CompleteSelection();
        var result = InsertTextInputAction.Instance.Do(_context, "XXX\nYYY\nZZZ");

        Assert.AreEqual(new CursorPosition(0, 7), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 3), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 5), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionEnd);
        Assert.AreEqual("56", result.DeletedSelectedText);
        Assert.AreEqual(new CursorPosition(0, 5), result.InsertStartPosition);
        Assert.AreEqual(new CursorPosition(2, 3), result.InsertEndPosition);
        Assert.AreEqual("XXX\nYYY\nZZZ", result.InsertedText);
        Assert.True(result.HasInserted);
        Assert.AreEqual("01234XXX\r\nYYY\r\nZZZ789", _text.ToString());
    }

    [Test]
    public void InsertText_Empty()
    {
        var result = InsertTextInputAction.Instance.Do(_context, "");

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual(new CursorPosition(0, 0), result.InsertStartPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.InsertEndPosition);
        Assert.AreEqual("", result.InsertedText);
        Assert.False(result.HasInserted);
        Assert.AreEqual("", _text.ToString());
    }

    [Test]
    public void InsertText_VirtualCursor()
    {
        SetText("    125\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));

        var result = InsertTextInputAction.Instance.Do(_context, "333");
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 7), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    125\r\n    333", _text.ToString());
    }

    [Test]
    public void InsertText_VirtualCursor_2()
    {
        SetText("    125");
        MoveCursorEndLine();
        AppendNewLine();

        var result = InsertTextInputAction.Instance.Do(_context, "333");
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 7), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    125\r\n    333", _text.ToString());
    }

    [Test]
    public void InsertTwoLines_VirtualCursor()
    {
        SetText("    125\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));

        var result = InsertTextInputAction.Instance.Do(_context, "3\n4");
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    125\r\n    3\r\n4", _text.ToString());
    }

    [Test]
    public void InsertTwoLines_VirtualCursor_2()
    {
        SetText("    125");
        MoveCursorEndLine();
        AppendNewLine();

        var result = InsertTextInputAction.Instance.Do(_context, "3\n4");
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    125\r\n    3\r\n4", _text.ToString());
    }

    [Test]
    public void InsertText_WithSelection_1_VirtualCursor()
    {
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 0));
        CompleteSelection();
        var result = InsertTextInputAction.Instance.Do(_context, "9");

        Assert.AreEqual(new CursorPosition(2, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 5), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 0), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n    9", _text.ToString());
    }

    [Test]
    public void InsertText_WithSelection_2_VirtualCursor()
    {
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(0, 7));
        ActivateSelection();
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = InsertTextInputAction.Instance.Do(_context, "9");

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 8), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    0129\r\n", _text.ToString());
    }

    [Test]
    public void InsertText_WithSelection_3_VirtualCursor()
    {
        SetText("    012\r\n\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = InsertTextInputAction.Instance.Do(_context, "9");

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 5), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n    9\r\n", _text.ToString());
    }
}
