﻿using System.Linq;
using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class CodeTextBoxCursorIntegration : BaseCodeTextBoxIntegration
{
    private Mock<ICodeTextBoxView> _codeTextBox;
    private Mock<IViewportContext> _viewportContext;
    private CodeTextBoxModel _model;

    [SetUp]
    public void Setup()
    {
        _codeTextBox = new Mock<ICodeTextBoxView>();
        _viewportContext = _codeTextBox.As<IViewportContext>();
        _model = MakeModel();
        _model.AttachCodeTextBox(_codeTextBox.Object);
        _codeTextBox.Raise(x => x.FontSettingsChanged += null, new FontSettingsChangedEventArgs(10, 10));
        _model.Text = "";
    }

    [Test]
    public void CursorMove()
    {
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("0123456789");

        _model.MoveCursorTo(new(0, 5));
        _model.MoveCursorLeft();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(4, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorRight();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(5, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorDown();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(5, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorUp();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(5, _model.CursorPosition.ColumnIndex);
    }

    [Test]
    public void CursorLimitsLeft()
    {
        AppendString("0123456789");

        _model.MoveCursorTextBegin();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorTextBegin();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorStartLine();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorLeft();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
    }

    [Test]
    public void CursorLimitsRight()
    {
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("0123456789");

        _model.MoveCursorTextEnd();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(10, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorTextEnd();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(10, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorEndLine();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(10, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorRight();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(10, _model.CursorPosition.ColumnIndex);
    }

    [Test]
    public void CursorLimitsUp()
    {
        AppendString("0123456789");

        _model.MoveCursorTo(new(0, 5));
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(5, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorUp();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(5, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorPageUp();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(5, _model.CursorPosition.ColumnIndex);
    }

    [Test]
    public void CursorLimitsDown()
    {
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("0123456789");

        _model.MoveCursorTo(new(1, 5));
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(5, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorDown();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(5, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorPageDown();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(5, _model.CursorPosition.ColumnIndex);
    }

    [Test]
    public void CursorPageUp()
    {
        _viewportContext.SetupGet(x => x.ActualHeight).Returns(_model.TextMeasures.LineHeight * 3);
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("0123456789");
        _model.MoveCursorTo(new(4, 5));

        _model.MoveCursorPageUp();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(5, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorPageUp();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(5, _model.CursorPosition.ColumnIndex);
    }

    [Test]
    public void CursorPageDown()
    {
        _viewportContext.SetupGet(x => x.ActualHeight).Returns(_model.TextMeasures.LineHeight * 3);
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("0123456789");
        _model.MoveCursorTo(new(0, 5));

        _model.MoveCursorPageDown();
        Assert.AreEqual(3, _model.CursorPosition.LineIndex);
        Assert.AreEqual(5, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorPageDown();
        Assert.AreEqual(4, _model.CursorPosition.LineIndex);
        Assert.AreEqual(5, _model.CursorPosition.ColumnIndex);
    }

    [Test]
    public void CursorPositionAndTextEditing()
    {
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);

        AppendString("DECLARE @x INT");
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(14, _model.CursorPosition.ColumnIndex);

        _model.AppendNewLine();
        AppendString("DECLARE @y FLOAT");
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(16, _model.CursorPosition.ColumnIndex);

        Assert.AreEqual("DECLARE @x INT\r\nDECLARE @y FLOAT", _model.Text.ToString());

        _model.MoveCursorTextBegin();
        _model.MoveCursorRight();
        _model.MoveCursorRight();
        _model.AppendNewLine();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("DE\r\nCLARE @x INT\r\nDECLARE @y FLOAT", _model.Text.ToString());

        _model.LeftDelete();
        Assert.AreEqual("DECLARE @x INT\r\nDECLARE @y FLOAT", _model.Text.ToString());

        _model.MoveCursorTextBegin();
        _model.MoveCursorRight();
        _model.MoveCursorRight();
        _model.AppendNewLine();
        _model.MoveCursorTextBegin();
        _model.MoveCursorEndLine();
        _model.RightDelete();
        Assert.AreEqual("DECLARE @x INT\r\nDECLARE @y FLOAT", _model.Text.ToString());
    }

    [Test]
    public void MoveCursorLeftFromFirstLineStartPosition()
    {
        AppendString("0000000000");

        _model.MoveCursorStartLine();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorLeft();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
    }

    [Test]
    public void MoveCursorRightFromLastLineEndPosition()
    {
        AppendString("0000000000");

        _model.MoveCursorEndLine();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(10, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorRight();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(10, _model.CursorPosition.ColumnIndex);
    }

    [Test]
    public void MoveCursorFromOneLineToAnother()
    {
        AppendString("0000000000");
        _model.AppendNewLine();
        AppendString("0000000000");
        _model.MoveCursorTextBegin();
        _model.MoveCursorEndLine();

        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(10, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorRight();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);

        _model.MoveCursorLeft();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(10, _model.CursorPosition.ColumnIndex);
    }

    private void AppendString(string str)
    {
        str.ToList().ForEach(ch => _model.AppendChar(ch));
    }
}