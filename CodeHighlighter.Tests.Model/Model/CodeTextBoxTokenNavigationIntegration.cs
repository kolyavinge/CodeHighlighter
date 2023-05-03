using CodeHighlighter.Core;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class CodeTextBoxTokenNavigationIntegration : BaseCodeTextBoxIntegration
{
    private CodeTextBoxModel _model;

    [SetUp]
    public void Setup()
    {
        _model = MakeModel();
        _model.Text = "SELECT * FROM Table1\r\nJOIN Table2 ON t1 = t2";
    }

    [Test]
    public void MoveToNextToken()
    {
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(7, _model.CursorPosition.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(9, _model.CursorPosition.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(14, _model.CursorPosition.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(20, _model.CursorPosition.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(5, _model.CursorPosition.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(12, _model.CursorPosition.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(15, _model.CursorPosition.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(18, _model.CursorPosition.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(20, _model.CursorPosition.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(22, _model.CursorPosition.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(22, _model.CursorPosition.ColumnIndex);
    }

    [Test]
    public void MoveToNextToken_Selection()
    {
        _model.ActivateSelection();
        _model.MoveToNextToken();
        _model.MoveToNextToken();
        _model.MoveToNextToken();
        _model.MoveToNextToken();
        Assert.AreEqual(new CursorPosition(0, 0), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(0, 20), _model.TextSelection.EndPosition);

        _model.MoveToNextToken();
        _model.MoveToNextToken();
        _model.MoveToNextToken();
        _model.MoveToNextToken();
        _model.MoveToNextToken();
        _model.MoveToNextToken();
        _model.MoveToNextToken();
        _model.CompleteSelection();
        Assert.AreEqual(new CursorPosition(0, 0), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(1, 22), _model.TextSelection.EndPosition);
    }

    [Test]
    public void MoveToPrevToken()
    {
        _model.MoveCursorTo(new(1, 22));

        _model.MoveToPrevToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(20, _model.CursorPosition.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(18, _model.CursorPosition.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(15, _model.CursorPosition.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(12, _model.CursorPosition.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(5, _model.CursorPosition.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(20, _model.CursorPosition.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(14, _model.CursorPosition.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(9, _model.CursorPosition.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(7, _model.CursorPosition.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
    }

    [Test]
    public void MoveToPrevToken_Selection()
    {
        _model.MoveCursorTo(new(1, 22));

        _model.ActivateSelection();
        _model.MoveToPrevToken();
        _model.MoveToPrevToken();
        _model.MoveToPrevToken();
        _model.MoveToPrevToken();
        _model.MoveToPrevToken();
        _model.MoveToPrevToken();
        Assert.AreEqual(new CursorPosition(1, 22), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(1, 0), _model.TextSelection.EndPosition);

        _model.MoveToPrevToken();
        _model.MoveToPrevToken();
        _model.MoveToPrevToken();
        _model.MoveToPrevToken();
        _model.MoveToPrevToken();
        _model.CompleteSelection();
        Assert.AreEqual(new CursorPosition(1, 22), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(0, 0), _model.TextSelection.EndPosition);
    }

    [Test]
    public void DeleteLeftToken()
    {
        _model.MoveCursorTextEnd();

        _model.DeleteLeftToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(20, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("SELECT * FROM Table1\r\nJOIN Table2 ON t1 = ", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(18, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("SELECT * FROM Table1\r\nJOIN Table2 ON t1 ", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(15, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("SELECT * FROM Table1\r\nJOIN Table2 ON ", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(12, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("SELECT * FROM Table1\r\nJOIN Table2 ", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(5, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("SELECT * FROM Table1\r\nJOIN ", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(1, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("SELECT * FROM Table1\r\n", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(20, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("SELECT * FROM Table1", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(14, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("SELECT * FROM ", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(9, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("SELECT * ", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(7, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("SELECT ", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("", _model.Text.ToString());
    }

    [Test]
    public void DeleteRightToken()
    {
        _model.MoveCursorTextBegin();

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("* FROM Table1\r\nJOIN Table2 ON t1 = t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("FROM Table1\r\nJOIN Table2 ON t1 = t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("Table1\r\nJOIN Table2 ON t1 = t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("\r\nJOIN Table2 ON t1 = t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("JOIN Table2 ON t1 = t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("Table2 ON t1 = t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("ON t1 = t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("t1 = t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("= t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.CursorPosition.LineIndex);
        Assert.AreEqual(0, _model.CursorPosition.ColumnIndex);
        Assert.AreEqual("", _model.Text.ToString());
    }
}
