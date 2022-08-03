﻿using System.Linq;
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
    public void AppendChar()
    {
        var result = _model.AppendChar('0');

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual('0', result.AppendedChar);
    }

    [Test]
    public void AppendChar_WithSelection_1()
    {
        AppendString("000");
        _model.MoveCursorStartLine();
        _model.ActivateSelection();
        _model.MoveCursorEndLine();
        var result = _model.AppendChar('0');

        Assert.AreEqual(new CursorPosition(0, 3), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 3), result.SelectionEnd);
        Assert.AreEqual("000", result.DeletedSelectedText);
        Assert.AreEqual('0', result.AppendedChar);
    }

    [Test]
    public void AppendChar_WithSelection_2()
    {
        AppendString("000");
        _model.MoveCursorEndLine();
        _model.ActivateSelection();
        _model.MoveCursorStartLine();
        var result = _model.AppendChar('0');

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 3), result.SelectionEnd);
        Assert.AreEqual("000", result.DeletedSelectedText);
        Assert.AreEqual('0', result.AppendedChar);
    }

    [Test]
    public void LeftDelete()
    {
        AppendString("0");
        _model.MoveCursorTo(new(0, 1));
        var result = _model.LeftDelete();

        Assert.AreEqual(new CursorPosition(0, 1), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 1), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 1), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual('0', result.CharCharDeleteResult.DeletedChar);
        Assert.True(result.HasDeleted);
    }

    [Test]
    public void LeftDelete_WithSelection()
    {
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("9876543210");
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
    }

    [Test]
    public void LeftDelete_WithSelection_DeleteEmptyLine()
    {
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("9876543210");
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
    }

    [Test]
    public void RightDelete()
    {
        AppendString("0");
        _model.MoveCursorTo(new(0, 0));
        var result = _model.RightDelete();

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual('0', result.CharCharDeleteResult.DeletedChar);
        Assert.True(result.HasDeleted);
    }

    [Test]
    public void RightDelete_WithSelection()
    {
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("9876543210");
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
    }

    [Test]
    public void RightDelete_WithSelection_DeleteEmptyLine()
    {
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("9876543210");
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
    }

    [Test]
    public void InsertText_WithSelection()
    {
        AppendString("0123456789");
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
    }

    [Test]
    public void AppendNewLine_WithSelection()
    {
        AppendString("0123456789");
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
    }

    [Test]
    public void DeleteLeftToken()
    {
        AppendString("SELECT FROM");
        _model.MoveCursorEndLine();
        var result = _model.DeleteLeftToken();

        Assert.AreEqual(new CursorPosition(0, 11), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 11), result.SelectionEnd);
        Assert.AreEqual("FROM", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
    }

    [Test]
    public void DeleteLeftToken_NoDelection()
    {
        AppendString("SELECT FROM");
        _model.MoveCursorStartLine();
        var result = _model.DeleteLeftToken();

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.False(result.HasDeleted);
    }

    [Test]
    public void DeleteLeftToken_WithSelection()
    {
        AppendString("SELECT FROM");
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
    }

    [Test]
    public void DeleteRightToken()
    {
        AppendString("SELECT FROM");
        _model.MoveCursorTo(new(0, 7));
        var result = _model.DeleteRightToken();

        Assert.AreEqual(new CursorPosition(0, 7), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 7), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 11), result.SelectionEnd);
        Assert.AreEqual("FROM", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
    }

    [Test]
    public void DeleteRightToken_NoDelection()
    {
        AppendString("SELECT FROM");
        _model.MoveCursorEndLine();
        var result = _model.DeleteRightToken();

        Assert.AreEqual(new CursorPosition(0, 11), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 11), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 11), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 11), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.False(result.HasDeleted);
    }

    [Test]
    public void DeleteRightToken_WithSelection()
    {
        AppendString("SELECT FROM");
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
    }

    [Test]
    public void MoveSelectedLinesUp()
    {
        AppendString("000");
        _model.AppendNewLine();
        AppendString("111");
        _model.AppendNewLine();
        AppendString("222");
        _model.AppendNewLine();
        _model.MoveCursorTo(new(1, 1));
        var result = _model.MoveSelectedLinesUp();

        Assert.AreEqual(new CursorPosition(1, 1), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 1), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 1), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.True(result.HasMoved);
    }

    [Test]
    public void MoveSelectedLinesUp_NoMoving()
    {
        AppendString("000");
        _model.MoveCursorTo(new(0, 0));
        var result = _model.MoveSelectedLinesUp();

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.False(result.HasMoved);
    }

    [Test]
    public void MoveSelectedLinesUp_WithSelection()
    {
        AppendString("000");
        _model.AppendNewLine();
        AppendString("111");
        _model.AppendNewLine();
        AppendString("222");
        _model.AppendNewLine();
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
    }

    [Test]
    public void MoveSelectedLinesDown()
    {
        AppendString("000");
        _model.AppendNewLine();
        AppendString("111");
        _model.AppendNewLine();
        AppendString("222");
        _model.AppendNewLine();
        _model.MoveCursorTo(new(1, 1));
        var result = _model.MoveSelectedLinesDown();

        Assert.AreEqual(new CursorPosition(1, 1), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 1), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 1), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 1), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.True(result.HasMoved);
    }

    [Test]
    public void MoveSelectedLinesDown_NoMoving()
    {
        AppendString("000");
        _model.MoveCursorTo(new(0, 0));
        var result = _model.MoveSelectedLinesDown();

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.False(result.HasMoved);
    }

    [Test]
    public void MoveSelectedLinesDown_WithSelection()
    {
        AppendString("000");
        _model.AppendNewLine();
        AppendString("111");
        _model.AppendNewLine();
        AppendString("222");
        _model.AppendNewLine();
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
    }

    [Test]
    public void DeleteSelectedLines()
    {
        AppendString("000");
        _model.AppendNewLine();
        AppendString("111");
        _model.AppendNewLine();
        AppendString("222");
        _model.AppendNewLine();
        _model.MoveCursorTo(new(1, 1));
        var result = _model.DeleteSelectedLines();

        Assert.AreEqual(new CursorPosition(1, 1), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(1, 1), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(1, 1), result.SelectionEnd);
        Assert.AreEqual("111\r\n", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
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
    }

    [Test]
    public void DeleteSelectedLines_EmptyLine()
    {
        AppendString("000");
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
    }

    [Test]
    public void DeleteSelectedLines_WithSelection()
    {
        AppendString("000");
        _model.AppendNewLine();
        AppendString("111");
        _model.AppendNewLine();
        AppendString("222");
        _model.AppendNewLine();
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
    }

    [Test]
    public void DeleteSelectedLines_LastLine()
    {
        AppendString("000");
        _model.AppendNewLine();
        AppendString("111");
        _model.AppendNewLine();
        AppendString("222");
        _model.MoveCursorTo(new(2, 1));
        var result = _model.DeleteSelectedLines();

        Assert.AreEqual(new CursorPosition(2, 1), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(2, 1), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(2, 1), result.SelectionEnd);
        Assert.AreEqual("222", result.DeletedSelectedText);
        Assert.True(result.HasDeleted);
    }

    [Test]
    public void SetSelectedTextCase()
    {
        AppendString("AAA");
        _model.SelectAll();
        var result = _model.SetSelectedTextCase(TextCase.Lower);

        Assert.AreEqual(new CursorPosition(0, 3), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 3), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 3), result.SelectionEnd);
        Assert.AreEqual("AAA", result.DeletedSelectedText);
        Assert.AreEqual("aaa", result.ChangedText);
        Assert.True(result.HasChanged);
    }

    [Test]
    public void SetSelectedTextCase_NoChanges()
    {
        AppendString("AAA");
        _model.MoveCursorStartLine();
        var result = _model.SetSelectedTextCase(TextCase.Lower);

        Assert.AreEqual(new CursorPosition(0, 0), result.OldCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.NewCursorPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionStart);
        Assert.AreEqual(new CursorPosition(0, 0), result.SelectionEnd);
        Assert.AreEqual("", result.DeletedSelectedText);
        Assert.AreEqual("", result.ChangedText);
        Assert.False(result.HasChanged);
    }

    private void AppendString(string str)
    {
        str.ToList().ForEach(ch => _model.AppendChar(ch));
    }
}
