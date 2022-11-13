using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class InputModelIntegration
{
    private InputModel _model;

    [SetUp]
    public void Setup()
    {
        _model = InputModel.MakeDefault();
        _model.SetCodeProvider(new SqlCodeProvider());
        _model.SetText("");
    }

    [Test]
    public void SetText_ResetCursor_1()
    {
        _model.SetText("123");
        _model.MoveCursorTextEnd();

        _model.SetText("");

        Assert.AreEqual(new CursorPosition(0, 0), _model.TextCursor.Position);
    }

    [Test]
    public void SetText_ResetCursor_2()
    {
        _model.SetText("123");
        _model.MoveCursorTextEnd();

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
        _model.SetText("000");
        _model.MoveCursorStartLine();
        _model.ActivateSelection();
        _model.MoveCursorEndLine();
        _model.CompleteSelection();
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
        _model.SetText("000");
        _model.MoveCursorEndLine();
        _model.ActivateSelection();
        _model.MoveCursorStartLine();
        _model.CompleteSelection();
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
        _model.SetText("    012\r\n");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
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
        _model.SetText("    012");
        _model.MoveCursorEndLine();
        _model.AppendNewLine();
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
        _model.SetText("    012\r\n\r\n");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 0));
        _model.CompleteSelection();
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
        _model.SetText("    012\r\n\r\n");
        _model.MoveCursorTo(new(0, 7));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.CompleteSelection();
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
        _model.SetText("    012\r\n\r\n\r\n");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        _model.CompleteSelection();
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
        _model.SetText("0");
        _model.MoveCursorTo(new(0, 1));
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
        _model.SetText("0123456789\r\n9876543210");
        _model.MoveCursorTo(new(0, 3));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 7));
        _model.CompleteSelection();
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
        _model.SetText("0123456789\r\n9876543210");
        _model.MoveCursorTo(new(0, 10));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 0));
        _model.CompleteSelection();
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
        _model.SetText("    123\r\n");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
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
        _model.SetText("    123\r\n\r\n456");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 3));
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
        _model.SetText("    123\r\n\r\n456");
        _model.MoveCursorTo(new(0, 7));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
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
        _model.SetText("    123\r\n\r\n\r\n456");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
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
        _model.SetText("0");
        _model.MoveCursorTo(new(0, 0));
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
        _model.SetText("0123456789\r\n9876543210");
        _model.MoveCursorTo(new(0, 3));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 7));
        _model.CompleteSelection();
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
        _model.SetText("0123456789\r\n9876543210");
        _model.MoveCursorTo(new(0, 10));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 0));
        _model.CompleteSelection();
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
        _model.SetText("    123\r\n\r\n456");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
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
        _model.SetText("    123\r\n\r\n456");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 3));
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
        _model.SetText("    123\r\n\r\n456");
        _model.MoveCursorTo(new(0, 7));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
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
        _model.SetText("    123\r\n\r\n\r\n456");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
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
        _model.SetText("0123456789");
        _model.MoveCursorTo(new(0, 5));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(0, 7));
        _model.CompleteSelection();
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
        _model.SetText("    125\r\n");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));

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
        _model.SetText("    125");
        _model.MoveCursorEndLine();
        _model.AppendNewLine();

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
        _model.SetText("    125\r\n");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));

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
        _model.SetText("    125");
        _model.MoveCursorEndLine();
        _model.AppendNewLine();

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
        _model.SetText("    012\r\n\r\n");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 0));
        _model.CompleteSelection();
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
        _model.SetText("    012\r\n\r\n");
        _model.MoveCursorTo(new(0, 7));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.CompleteSelection();
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
        _model.SetText("    012\r\n\r\n\r\n");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        _model.CompleteSelection();
        var result = _model.InsertText("9");

        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 5), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 4, CursorPositionKind.Virtual), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), result.SelectionEnd);
        Assert.AreEqual("\r\n", result.DeletedSelectedText);
        Assert.AreEqual("    012\r\n    9\r\n", _model.Text.ToString());
    }

    [Test]
    public void AppendNewLine()
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
        _model.SetText("0123456789");
        _model.MoveCursorTo(new(0, 5));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(0, 7));
        _model.CompleteSelection();
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
        _model.SetText("    012\r\n");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
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
        _model.SetText("    012\r\n\r\n");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 0));
        _model.CompleteSelection();
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
        _model.SetText("    012\r\n\r\n");
        _model.MoveCursorTo(new(0, 7));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.CompleteSelection();
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
        _model.SetText("    012\r\n\r\n\r\n");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        _model.CompleteSelection();
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
        _model.SetText("SELECT FROM");
        _model.MoveCursorEndLine();
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
        _model.SetText("SELECT FROM");
        _model.MoveCursorStartLine();
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
        _model.SetText("SELECT FROM");
        _model.MoveCursorStartLine();
        _model.ActivateSelection();
        _model.MoveCursorEndLine();
        _model.CompleteSelection();
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
        _model.SetText("    SELECT FROM\r\n");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
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
        _model.SetText("    123\r\n\r\n456");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 3));
        _model.CompleteSelection();
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
        _model.SetText("    123\r\n\r\n456");
        _model.MoveCursorTo(new(0, 7));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.CompleteSelection();
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
        _model.SetText("    123\r\n\r\n\r\n456");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        _model.CompleteSelection();
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
        _model.SetText("SELECT FROM");
        _model.MoveCursorTo(new(0, 7));
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
        _model.SetText("SELECT FROM");
        _model.MoveCursorEndLine();
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
        _model.SetText("SELECT FROM");
        _model.MoveCursorStartLine();
        _model.ActivateSelection();
        _model.MoveCursorEndLine();
        _model.CompleteSelection();
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
        _model.SetText("    SELECT FROM\r\n\r\nSELECT FROM");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
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
        _model.SetText("    123\r\n\r\n456");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 3));
        _model.CompleteSelection();
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
        _model.SetText("    123\r\n\r\n456");
        _model.MoveCursorTo(new(0, 7));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.CompleteSelection();
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
        _model.SetText("    123\r\n\r\n\r\n456");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        _model.CompleteSelection();
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
        _model.SetText("000\r\n111\r\n222\r\n");
        _model.MoveCursorTo(new(1, 1));
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
        _model.SetText("000");
        _model.MoveCursorTo(new(0, 0));
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
        _model.SetText("000\r\n111\r\n222\r\n");
        _model.MoveCursorTo(new(1, 1));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 1));
        _model.CompleteSelection();
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
        _model.SetText("    012\r\n\r\n");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 0));
        _model.CompleteSelection();
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
        _model.SetText("\r\n    012\r\n\r\n");
        _model.MoveCursorTo(new(1, 7));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        _model.CompleteSelection();
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
        _model.SetText("    012\r\n\r\n\r\n");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        _model.CompleteSelection();
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
        _model.SetText("000\r\n111\r\n222\r\n");
        _model.MoveCursorTo(new(1, 1));
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
        _model.SetText("000");
        _model.MoveCursorTo(new(0, 0));
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
        _model.SetText("000\r\n111\r\n222\r\n");
        _model.MoveCursorTo(new(0, 1));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 1));
        _model.CompleteSelection();
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
        _model.SetText("    012\r\n\r\n\r\n");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 0));
        _model.CompleteSelection();
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
        _model.SetText("\r\n    012\r\n\r\n\r\n");
        _model.MoveCursorTo(new(1, 7));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        _model.CompleteSelection();
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
        _model.SetText("    012\r\n\r\n\r\n\r\n");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        _model.CompleteSelection();
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
        _model.SetText("000\r\n111\r\n222\r\n");
        _model.MoveCursorTo(new(1, 1));
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
        _model.SetText("000");
        _model.AppendNewLine();
        _model.AppendNewLine();
        _model.MoveCursorTo(new(1, 0));
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
        _model.SetText("000\r\n111\r\n222\r\n");
        _model.MoveCursorTo(new(0, 1));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 1));
        _model.CompleteSelection();
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
        _model.SetText("000\r\n111\r\n222");
        _model.MoveCursorTo(new(2, 1));
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
        _model.SetText("    012\r\n\r\n");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 0));
        _model.CompleteSelection();
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
        _model.SetText("    012\r\n\r\n");
        _model.MoveCursorTo(new(0, 7));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.CompleteSelection();
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
        _model.SetText("    012\r\n\r\n\r\n");
        _model.MoveCursorTo(new(1, 4, CursorPositionKind.Virtual));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 4, CursorPositionKind.Virtual));
        _model.CompleteSelection();
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
        _model.SetText("AAA");
        _model.SelectAll();
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
        _model.SetText("AAA");
        _model.MoveCursorStartLine();
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
