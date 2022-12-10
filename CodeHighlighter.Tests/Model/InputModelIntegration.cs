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
    public void AppendChar()
    {
        var result = _model.AppendChar('0');

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual('0', result.AppendedChar);
        Assert.AreEqual("0", _model.Text.ToString());
    }

    [Test]
    public void AppendChar_WithSelection_1()
    {
        SetText("000");
        MoveCursorStartLine();
        ActivateSelection();
        MoveCursorEndLine();
        CompleteSelection();
        var result = _model.AppendChar('0');

        Assert.AreEqual(new CursorPosition(0, 3), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 3), result.SelectionEnd);
        Assert.AreEqual("000", result.DeletedSelectedText);
        Assert.AreEqual('0', result.AppendedChar);
        Assert.AreEqual("0", _model.Text.ToString());
    }

    [Test]
    public void AppendChar_WithSelection_2()
    {
        SetText("000");
        MoveCursorEndLine();
        ActivateSelection();
        MoveCursorStartLine();
        CompleteSelection();
        var result = _model.AppendChar('0');

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 3), result.SelectionEnd);
        Assert.AreEqual("000", result.DeletedSelectedText);
        Assert.AreEqual('0', result.AppendedChar);
        Assert.AreEqual("0", _model.Text.ToString());
    }

    [Test]
    public void AppendChar_VirtualCursor()
    {
        SetText("    012\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        var result = _model.AppendChar('9');

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 5), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n    9", _model.Text.ToString());
    }

    [Test]
    public void AppendChar_VirtualCursor_2()
    {
        SetText("    012");
        MoveCursorEndLine();
        AppendNewLine();
        var result = _model.AppendChar('9');

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 5), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n    9", _model.Text.ToString());
    }

    [Test]
    public void AppendChar_WithSelection_1_VirtualCursor()
    {
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 0));
        CompleteSelection();
        var result = _model.AppendChar('9');

        Assert.AreEqual(new CursorPosition(2, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 5), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 0), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n    9", _model.Text.ToString());
    }

    [Test]
    public void AppendChar_WithSelection_2_VirtualCursor()
    {
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(0, 7));
        ActivateSelection();
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = _model.AppendChar('9');

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 8), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    0129\r\n", _model.Text.ToString());
    }

    [Test]
    public void AppendChar_WithSelection_3_VirtualCursor()
    {
        SetText("    012\r\n\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = _model.AppendChar('9');

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 5), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n    9\r\n", _model.Text.ToString());
    }

    [Test]
    public void LeftDelete()
    {
        SetText("0");
        MoveCursorTo(new(0, 1));
        var result = _model.LeftDelete();

        Assert.AreEqual(new CursorPosition(0, 1), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 1), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 1), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual('0', result.CharCharDeleteResult.DeletedChar);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("", _model.Text.ToString());
    }

    [Test]
    public void LeftDelete_WithSelection()
    {
        SetText("0123456789\r\n9876543210");
        MoveCursorTo(new(0, 3));
        ActivateSelection();
        MoveCursorTo(new(1, 7));
        CompleteSelection();
        var result = _model.LeftDelete();

        Assert.AreEqual(new CursorPosition(1, 7), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 3), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 3), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 7), result.SelectionEnd);
        Assert.AreEqual("3456789\r\n9876543", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("012210", _model.Text.ToString());
    }

    [Test]
    public void LeftDelete_WithSelection_DeleteEmptyLine()
    {
        SetText("0123456789\r\n9876543210");
        MoveCursorTo(new(0, 10));
        ActivateSelection();
        MoveCursorTo(new(1, 0));
        CompleteSelection();
        var result = _model.LeftDelete();

        Assert.AreEqual(new CursorPosition(1, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 10), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 10), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 0), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("01234567899876543210", _model.Text.ToString());
    }

    [Test]
    public void LeftDelete_VirtualCursor()
    {
        SetText("    123\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        var result = _model.LeftDelete();

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.False(result.HasDeleted);
        Assert.AreEqual("    123\r\n", _model.Text.ToString());
    }

    [Test]
    public void LeftDelete_WithSelection_1_VirtualCursor()
    {
        SetText("    123\r\n\r\n456");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 3));
        var result = _model.LeftDelete();

        Assert.AreEqual(new CursorPosition(2, 3), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 3), result.SelectionEnd);
        Assert.AreEqual("\r\n456", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n    ", _model.Text.ToString());
    }

    [Test]
    public void LeftDelete_WithSelection_2_VirtualCursor()
    {
        SetText("    123\r\n\r\n456");
        MoveCursorTo(new(0, 7));
        ActivateSelection();
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        var result = _model.LeftDelete();

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n456", _model.Text.ToString());
    }

    [Test]
    public void LeftDelete_WithSelection_3_VirtualCursor()
    {
        SetText("    123\r\n\r\n\r\n456");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        var result = _model.LeftDelete();

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n    \r\n456", _model.Text.ToString());
    }

    [Test]
    public void RightDelete()
    {
        SetText("0");
        MoveCursorTo(new(0, 0));
        var result = _model.RightDelete();

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
        var result = _model.RightDelete();

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
        var result = _model.RightDelete();

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
        var result = _model.RightDelete();

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
        var result = _model.RightDelete();

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
        var result = _model.RightDelete();

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
        var result = _model.RightDelete();

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n    \r\n456", _model.Text.ToString());
    }

    [Test]
    public void InsertText()
    {
        var result = _model.InsertText("XXX\nYYY\nZZZ");

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 3), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual(new CursorPosition(0, 0), result.InsertStartPosition);
        Assert.AreEqual(new CursorPosition(2, 3), result.InsertEndPosition);
        Assert.AreEqual("XXX\nYYY\nZZZ", result.InsertedText);
        Assert.True(result.HasInserted);
        Assert.AreEqual("XXX\r\nYYY\r\nZZZ", _model.Text.ToString());
    }

    [Test]
    public void InsertText_WithSelection()
    {
        SetText("0123456789");
        MoveCursorTo(new(0, 5));
        ActivateSelection();
        MoveCursorTo(new(0, 7));
        CompleteSelection();
        var result = _model.InsertText("XXX\nYYY\nZZZ");

        Assert.AreEqual(new CursorPosition(0, 7), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 3), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 5), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionEnd);
        Assert.AreEqual("56", result.DeletedSelectedText);
        Assert.AreEqual(new CursorPosition(0, 5), result.InsertStartPosition);
        Assert.AreEqual(new CursorPosition(2, 3), result.InsertEndPosition);
        Assert.AreEqual("XXX\nYYY\nZZZ", result.InsertedText);
        Assert.True(result.HasInserted);
        Assert.AreEqual("01234XXX\r\nYYY\r\nZZZ789", _model.Text.ToString());
    }

    [Test]
    public void InsertText_Empty()
    {
        var result = _model.InsertText("");

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual(new CursorPosition(0, 0), result.InsertStartPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.InsertEndPosition);
        Assert.AreEqual("", result.InsertedText);
        Assert.False(result.HasInserted);
        Assert.AreEqual("", _model.Text.ToString());
    }

    [Test]
    public void InsertText_VirtualCursor()
    {
        SetText("    125\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));

        var result = _model.InsertText("333");
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 7), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    125\r\n    333", _model.Text.ToString());
    }

    [Test]
    public void InsertText_VirtualCursor_2()
    {
        SetText("    125");
        MoveCursorEndLine();
        AppendNewLine();

        var result = _model.InsertText("333");
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 7), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    125\r\n    333", _model.Text.ToString());
    }

    [Test]
    public void InsertTwoLines_VirtualCursor()
    {
        SetText("    125\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));

        var result = _model.InsertText("3\n4");
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    125\r\n    3\r\n4", _model.Text.ToString());
    }

    [Test]
    public void InsertTwoLines_VirtualCursor_2()
    {
        SetText("    125");
        MoveCursorEndLine();
        AppendNewLine();

        var result = _model.InsertText("3\n4");
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    125\r\n    3\r\n4", _model.Text.ToString());
    }

    [Test]
    public void InsertText_WithSelection_1_VirtualCursor()
    {
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 0));
        CompleteSelection();
        var result = _model.InsertText("9");

        Assert.AreEqual(new CursorPosition(2, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 5), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 0), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n    9", _model.Text.ToString());
    }

    [Test]
    public void InsertText_WithSelection_2_VirtualCursor()
    {
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(0, 7));
        ActivateSelection();
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = _model.InsertText("9");

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 8), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    0129\r\n", _model.Text.ToString());
    }

    [Test]
    public void InsertText_WithSelection_3_VirtualCursor()
    {
        SetText("    012\r\n\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = _model.InsertText("9");

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 5), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n    9\r\n", _model.Text.ToString());
    }

    [Test]
    public void AppendNewLine_NoSelection()
    {
        var result = _model.AppendNewLine();

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("\r\n", _model.Text.ToString());
    }

    [Test]
    public void AppendNewLine_WithSelection()
    {
        SetText("0123456789");
        MoveCursorTo(new(0, 5));
        ActivateSelection();
        MoveCursorTo(new(0, 7));
        CompleteSelection();
        var result = _model.AppendNewLine();

        Assert.AreEqual(new CursorPosition(0, 7), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 5), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionEnd);
        Assert.AreEqual("56", result.DeletedSelectedText);
        Assert.AreEqual("01234\r\n789", _model.Text.ToString());
    }

    [Test]
    public void AppendNewLine_VirtualCursor()
    {
        SetText("    012\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        var result = _model.AppendNewLine();

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n\r\n", _model.Text.ToString());
    }

    [Test]
    public void AppendNewLine_WithSelection_1_VirtualCursor()
    {
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 0));
        CompleteSelection();
        var result = _model.AppendNewLine();

        Assert.AreEqual(new CursorPosition(2, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 0), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n\r\n", _model.Text.ToString());
    }

    [Test]
    public void AppendNewLine_WithSelection_2_VirtualCursor()
    {
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(0, 7));
        ActivateSelection();
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = _model.AppendNewLine();

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n\r\n", _model.Text.ToString());
    }

    [Test]
    public void AppendNewLine_WithSelection_3_VirtualCursor()
    {
        SetText("    012\r\n\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = _model.AppendNewLine();

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n\r\n\r\n", _model.Text.ToString());
    }

    [Test]
    public void DeleteLeftToken()
    {
        SetText("SELECT FROM");
        MoveCursorEndLine();
        var result = _model.DeleteLeftToken();

        Assert.AreEqual(new CursorPosition(0, 11), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 11), result.SelectionEnd);
        Assert.AreEqual("FROM", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("SELECT ", _model.Text.ToString());
    }

    [Test]
    public void DeleteLeftToken_NoDelection()
    {
        SetText("SELECT FROM");
        MoveCursorStartLine();
        var result = _model.DeleteLeftToken();

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.False(result.HasDeleted);
        Assert.AreEqual("SELECT FROM", _model.Text.ToString());
    }

    [Test]
    public void DeleteLeftToken_WithSelection()
    {
        SetText("SELECT FROM");
        MoveCursorStartLine();
        ActivateSelection();
        MoveCursorEndLine();
        CompleteSelection();
        var result = _model.DeleteLeftToken();

        Assert.AreEqual(new CursorPosition(0, 11), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 11), result.SelectionEnd);
        Assert.AreEqual("SELECT FROM", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("", _model.Text.ToString());
    }

    [Test]
    public void DeleteLeftToken_VirtualCursor()
    {
        SetText("    SELECT FROM\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        var result = _model.DeleteLeftToken();

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.False(result.HasDeleted);
        Assert.AreEqual("    SELECT FROM\r\n", _model.Text.ToString());
    }

    [Test]
    public void DeleteLeftToken_1_VirtualCursor()
    {
        SetText("    123\r\n\r\n456");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 3));
        CompleteSelection();
        var result = _model.DeleteLeftToken();

        Assert.AreEqual(new CursorPosition(2, 3), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 3), result.SelectionEnd);
        Assert.AreEqual("\r\n456", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n    ", _model.Text.ToString());
    }

    [Test]
    public void DeleteLeftToken_2_VirtualCursor()
    {
        SetText("    123\r\n\r\n456");
        MoveCursorTo(new(0, 7));
        ActivateSelection();
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = _model.DeleteLeftToken();

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n456", _model.Text.ToString());
    }

    [Test]
    public void DeleteLeftToken_3_VirtualCursor()
    {
        SetText("    123\r\n\r\n\r\n456");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = _model.DeleteLeftToken();

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n    \r\n456", _model.Text.ToString());
    }

    [Test]
    public void DeleteRightToken()
    {
        SetText("SELECT FROM");
        MoveCursorTo(new(0, 7));
        var result = _model.DeleteRightToken();

        Assert.AreEqual(new CursorPosition(0, 7), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 11), result.SelectionEnd);
        Assert.AreEqual("FROM", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("SELECT ", _model.Text.ToString());
    }

    [Test]
    public void DeleteRightToken_NoDelection()
    {
        SetText("SELECT FROM");
        MoveCursorEndLine();
        var result = _model.DeleteRightToken();

        Assert.AreEqual(new CursorPosition(0, 11), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 11), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 11), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 11), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.False(result.HasDeleted);
        Assert.AreEqual("SELECT FROM", _model.Text.ToString());
    }

    [Test]
    public void DeleteRightToken_WithSelection()
    {
        SetText("SELECT FROM");
        MoveCursorStartLine();
        ActivateSelection();
        MoveCursorEndLine();
        CompleteSelection();
        var result = _model.DeleteRightToken();

        Assert.AreEqual(new CursorPosition(0, 11), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 11), result.SelectionEnd);
        Assert.AreEqual("SELECT FROM", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("", _model.Text.ToString());
    }

    [Test]
    public void DeleteRightToken_VirtualCursor()
    {
        SetText("    SELECT FROM\r\n\r\nSELECT FROM");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        var result = _model.DeleteRightToken();

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 0), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    SELECT FROM\r\n    SELECT FROM", _model.Text.ToString());
    }

    [Test]
    public void DeleteRightToken_WithSelection_1_VirtualCursor()
    {
        SetText("    123\r\n\r\n456");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 3));
        CompleteSelection();
        var result = _model.DeleteRightToken();

        Assert.AreEqual(new CursorPosition(2, 3), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 3), result.SelectionEnd);
        Assert.AreEqual("\r\n456", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n    ", _model.Text.ToString());
    }

    [Test]
    public void DeleteRightToken_WithSelection_2_VirtualCursor()
    {
        SetText("    123\r\n\r\n456");
        MoveCursorTo(new(0, 7));
        ActivateSelection();
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = _model.DeleteRightToken();

        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n456", _model.Text.ToString());
    }

    [Test]
    public void DeleteRightToken_WithSelection_3_VirtualCursor()
    {
        SetText("    123\r\n\r\n\r\n456");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = _model.DeleteRightToken();

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
        Assert.AreEqual("    123\r\n    \r\n456", _model.Text.ToString());
    }

    [Test]
    public void MoveSelectedLinesUp()
    {
        SetText("000\r\n111\r\n222\r\n");
        MoveCursorTo(new(1, 1));
        var result = _model.MoveSelectedLinesUp();

        Assert.AreEqual(new CursorPosition(1, 1), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 1), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 1), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.True(result.HasMoved);
        Assert.AreEqual("111\r\n000\r\n222\r\n", _model.Text.ToString());
    }

    [Test]
    public void MoveSelectedLinesUp_NoMoving()
    {
        SetText("000");
        MoveCursorTo(new(0, 0));
        var result = _model.MoveSelectedLinesUp();

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.False(result.HasMoved);
        Assert.AreEqual("000", _model.Text.ToString());
    }

    [Test]
    public void MoveSelectedLinesUp_WithSelection()
    {
        SetText("000\r\n111\r\n222\r\n");
        MoveCursorTo(new(1, 1));
        ActivateSelection();
        MoveCursorTo(new(2, 1));
        CompleteSelection();
        var result = _model.MoveSelectedLinesUp();

        Assert.AreEqual(new CursorPosition(2, 1), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 1), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 1), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.True(result.HasMoved);
        Assert.AreEqual("111\r\n222\r\n000\r\n", _model.Text.ToString());
    }

    [Test]
    public void MoveSelectedLinesUp_WithSelection_1_VirtualCursor()
    {
        SetText("    012\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 0));
        CompleteSelection();
        var result = _model.MoveSelectedLinesUp();

        Assert.AreEqual(new CursorPosition(2, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("\r\n\r\n    012", _model.Text.ToString());
    }

    [Test]
    public void MoveSelectedLinesUp_WithSelection_2_VirtualCursor()
    {
        SetText("\r\n    012\r\n\r\n");
        MoveCursorTo(new(1, 7));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = _model.MoveSelectedLinesUp();

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n\r\n\r\n", _model.Text.ToString());
    }

    [Test]
    public void MoveSelectedLinesUp_WithSelection_3_VirtualCursor()
    {
        SetText("    012\r\n\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = _model.MoveSelectedLinesUp();

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("\r\n\r\n    012\r\n", _model.Text.ToString());
    }

    [Test]
    public void MoveSelectedLinesDown()
    {
        SetText("000\r\n111\r\n222\r\n");
        MoveCursorTo(new(1, 1));
        var result = _model.MoveSelectedLinesDown();

        Assert.AreEqual(new CursorPosition(1, 1), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 1), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 1), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.True(result.HasMoved);
        Assert.AreEqual("000\r\n222\r\n111\r\n", _model.Text.ToString());
    }

    [Test]
    public void MoveSelectedLinesDown_NoMoving()
    {
        SetText("000");
        MoveCursorTo(new(0, 0));
        var result = _model.MoveSelectedLinesDown();

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.False(result.HasMoved);
        Assert.AreEqual("000", _model.Text.ToString());
    }

    [Test]
    public void MoveSelectedLinesDown_WithSelection()
    {
        SetText("000\r\n111\r\n222\r\n");
        MoveCursorTo(new(0, 1));
        ActivateSelection();
        MoveCursorTo(new(1, 1));
        CompleteSelection();
        var result = _model.MoveSelectedLinesDown();

        Assert.AreEqual(new CursorPosition(1, 1), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 1), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 1), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.True(result.HasMoved);
        Assert.AreEqual("222\r\n000\r\n111\r\n", _model.Text.ToString());
    }

    [Test]
    public void MoveSelectedLinesDown_WithSelection_1_VirtualCursor()
    {
        SetText("    012\r\n\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 0));
        CompleteSelection();
        var result = _model.MoveSelectedLinesDown();

        Assert.AreEqual(new CursorPosition(2, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(3, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n\r\n\r\n", _model.Text.ToString());
    }

    [Test]
    public void MoveSelectedLinesDown_WithSelection_2_VirtualCursor()
    {
        SetText("\r\n    012\r\n\r\n\r\n");
        MoveCursorTo(new(1, 7));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = _model.MoveSelectedLinesDown();

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(3, 4, CursorPositionKind.Virtual), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("\r\n\r\n    012\r\n\r\n", _model.Text.ToString());
    }

    [Test]
    public void MoveSelectedLinesDown_WithSelection_3_VirtualCursor()
    {
        SetText("    012\r\n\r\n\r\n\r\n");
        MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        ActivateSelection();
        MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        CompleteSelection();
        var result = _model.MoveSelectedLinesDown();

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(3, 4, CursorPositionKind.Virtual), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n\r\n\r\n\r\n", _model.Text.ToString());
    }

    [Test]
    public void DeleteSelectedLines()
    {
        SetText("000\r\n111\r\n222\r\n");
        MoveCursorTo(new(1, 1));
        var result = _model.DeleteSelectedLines();

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
        var result = _model.DeleteSelectedLines();

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
        var result = _model.DeleteSelectedLines();

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
        var result = _model.DeleteSelectedLines();

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
        var result = _model.DeleteSelectedLines();

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
        var result = _model.DeleteSelectedLines();

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
        var result = _model.DeleteSelectedLines();

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
        var result = _model.DeleteSelectedLines();

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n", _model.Text.ToString());
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
