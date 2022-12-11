﻿using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class CodeTextBoxModelInsertionIntegration
{
    private CodeTextBoxModel _model;

    [SetUp]
    public void Setup()
    {
        _model = new CodeTextBoxModel(new SqlCodeProvider());
        _model.SetText("");
    }

    [Test]
    public void InsertText_Empty_OneLine()
    {
        _model.MoveCursorTo(new(0, 0));
        _model.InsertText("XXX");
        Assert.AreEqual("XXX", _model.Text.ToString());
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(3, _model.TextCursor.ColumnIndex);
        Assert.AreEqual(1, _model.Tokens.LinesCount);
        Assert.AreEqual(1, _model.Tokens.GetTokens(0).Count);
    }

    [Test]
    public void InsertText_Empty_MultyLine()
    {
        _model.MoveCursorTo(new(0, 0));
        _model.InsertText("XXX\nYYY\nZZZ");
        Assert.AreEqual("XXX\r\nYYY\r\nZZZ", _model.Text.ToString());
        Assert.AreEqual(2, _model.TextCursor.LineIndex);
        Assert.AreEqual(3, _model.TextCursor.ColumnIndex);
        Assert.AreEqual(3, _model.Tokens.LinesCount);
        Assert.AreEqual(1, _model.Tokens.GetTokens(0).Count);
        Assert.AreEqual(1, _model.Tokens.GetTokens(1).Count);
        Assert.AreEqual(1, _model.Tokens.GetTokens(2).Count);
    }

    [Test]
    public void InsertText_OneLine()
    {
        _model.SetText("0123456789");
        _model.MoveCursorTo(new(0, 5));
        _model.InsertText("XXX");
        Assert.AreEqual("01234XXX56789", _model.Text.ToString());
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(8, _model.TextCursor.ColumnIndex);
        Assert.AreEqual(1, _model.Tokens.LinesCount);
        Assert.AreEqual(1, _model.Tokens.GetTokens(0).Count);
    }

    [Test]
    public void InsertText_OneLine_SecondTextLine()
    {
        _model.SetText("0123456789\r\n");
        _model.MoveCursorTo(new(1, 0));
        _model.InsertText("XXX");
        Assert.AreEqual("0123456789\r\nXXX", _model.Text.ToString());
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(3, _model.TextCursor.ColumnIndex);
        Assert.AreEqual(2, _model.Tokens.LinesCount);
        Assert.AreEqual(1, _model.Tokens.GetTokens(1).Count);
    }

    [Test]
    public void InsertText_MultyLines()
    {
        _model.SetText("0123456789");
        _model.MoveCursorTo(new(0, 5));
        _model.InsertText("XXX\nYYY\nZZZ");
        Assert.AreEqual("01234XXX\r\nYYY\r\nZZZ56789", _model.Text.ToString());
        Assert.AreEqual(2, _model.TextCursor.LineIndex);
        Assert.AreEqual(3, _model.TextCursor.ColumnIndex);
        Assert.AreEqual(3, _model.Tokens.LinesCount);
        Assert.AreEqual(1, _model.Tokens.GetTokens(0).Count);
        Assert.AreEqual(1, _model.Tokens.GetTokens(1).Count);
        Assert.AreEqual(1, _model.Tokens.GetTokens(2).Count);
    }

    [Test]
    public void InsertText_MultyLinesWithSelection()
    {
        _model.SetText("0123456789");
        _model.MoveCursorTo(new(0, 5));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(0, 7));
        _model.CompleteSelection();
        _model.InsertText("XXX\nYYY\nZZZ");
        Assert.AreEqual("01234XXX\r\nYYY\r\nZZZ789", _model.Text.ToString());
        Assert.AreEqual(2, _model.TextCursor.LineIndex);
        Assert.AreEqual(3, _model.TextCursor.ColumnIndex);
        Assert.AreEqual(3, _model.Tokens.LinesCount);
        Assert.AreEqual(1, _model.Tokens.GetTokens(0).Count);
        Assert.AreEqual(1, _model.Tokens.GetTokens(1).Count);
        Assert.AreEqual(1, _model.Tokens.GetTokens(2).Count);
    }

    [Test]
    public void InsertText_MultyLinesWithAllSelection()
    {
        _model.SetText("0123456789");
        _model.SelectAll();
        _model.InsertText("XXX\nYYY\nZZZ");
        Assert.AreEqual("XXX\r\nYYY\r\nZZZ", _model.Text.ToString());
        Assert.AreEqual(2, _model.TextCursor.LineIndex);
        Assert.AreEqual(3, _model.TextCursor.ColumnIndex);
        Assert.AreEqual(3, _model.Tokens.LinesCount);
        Assert.AreEqual(1, _model.Tokens.GetTokens(0).Count);
        Assert.AreEqual(1, _model.Tokens.GetTokens(1).Count);
        Assert.AreEqual(1, _model.Tokens.GetTokens(2).Count);
    }
}