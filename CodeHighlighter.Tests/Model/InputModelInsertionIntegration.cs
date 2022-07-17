using System.Linq;
using CodeHighlighter.CodeProviders;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

public class InputModelInsertionIntegration
{
    private InputModel _model;

    [SetUp]
    public void Setup()
    {
        _model = new InputModel();
        _model.SetCodeProvider(new SqlCodeProvider());
        _model.SetText("");
    }

    [Test]
    public void InsertText_Empty_OneLine()
    {
        _model.MoveCursorTo(0, 0);
        _model.InsertText("XXX");
        Assert.AreEqual("XXX", _model.Text.ToString());
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(3, _model.TextCursor.ColumnIndex);
    }

    [Test]
    public void InsertText_Empty_MultyLine()
    {
        _model.MoveCursorTo(0, 0);
        _model.InsertText("XXX\nYYY\nZZZ");
        Assert.AreEqual("XXX\r\nYYY\r\nZZZ", _model.Text.ToString());
        Assert.AreEqual(2, _model.TextCursor.LineIndex);
        Assert.AreEqual(3, _model.TextCursor.ColumnIndex);
    }

    [Test]
    public void InsertText_OneLine()
    {
        AppendString("0123456789");
        _model.MoveCursorTo(0, 5);
        _model.InsertText("XXX");
        Assert.AreEqual("01234XXX56789", _model.Text.ToString());
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(8, _model.TextCursor.ColumnIndex);
    }

    [Test]
    public void InsertText_MultyLines()
    {
        AppendString("0123456789");
        _model.MoveCursorTo(0, 5);
        _model.InsertText("XXX\nYYY\nZZZ");
        Assert.AreEqual("01234XXX\r\nYYY\r\nZZZ56789", _model.Text.ToString());
        Assert.AreEqual(2, _model.TextCursor.LineIndex);
        Assert.AreEqual(3, _model.TextCursor.ColumnIndex);
    }

    [Test]
    public void InsertText_MultyLinesWithSelection()
    {
        AppendString("0123456789");
        _model.MoveCursorTo(0, 5);
        _model.ActivateSelection();
        _model.MoveCursorTo(0, 7);
        _model.CompleteSelection();
        _model.InsertText("XXX\nYYY\nZZZ");
        Assert.AreEqual("01234XXX\r\nYYY\r\nZZZ789", _model.Text.ToString());
        Assert.AreEqual(2, _model.TextCursor.LineIndex);
        Assert.AreEqual(3, _model.TextCursor.ColumnIndex);
    }

    [Test]
    public void InsertText_MultyLinesWithAllSelection()
    {
        AppendString("0123456789");
        _model.SelectAll();
        _model.InsertText("XXX\nYYY\nZZZ");
        Assert.AreEqual("XXX\r\nYYY\r\nZZZ", _model.Text.ToString());
        Assert.AreEqual(2, _model.TextCursor.LineIndex);
        Assert.AreEqual(3, _model.TextCursor.ColumnIndex);
    }

    private void AppendString(string str)
    {
        str.ToList().ForEach(_model.AppendChar);
    }
}
