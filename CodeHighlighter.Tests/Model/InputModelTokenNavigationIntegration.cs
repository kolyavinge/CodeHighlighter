using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class InputModelTokenNavigationIntegration
{
    private InputModel _model;

    [SetUp]
    public void Setup()
    {
        _model = InputModel.MakeDefault();
        _model.SetCodeProvider(new SqlCodeProvider());
        _model.SetText("SELECT * FROM Table1\r\nJOIN Table2 ON t1 = t2");
    }

    [Test]
    public void MoveToNextToken()
    {
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(7, _model.TextCursor.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(9, _model.TextCursor.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(14, _model.TextCursor.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(20, _model.TextCursor.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(5, _model.TextCursor.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(12, _model.TextCursor.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(15, _model.TextCursor.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(18, _model.TextCursor.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(20, _model.TextCursor.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(22, _model.TextCursor.ColumnIndex);

        _model.MoveToNextToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(22, _model.TextCursor.ColumnIndex);
    }

    [Test]
    public void MoveToNextToken_Selection()
    {
        _model.ActivateSelection();
        _model.MoveToNextToken();
        _model.MoveToNextToken();
        _model.MoveToNextToken();
        _model.MoveToNextToken();
        Assert.AreEqual(0, _model.TextSelection.StartCursorLineIndex);
        Assert.AreEqual(0, _model.TextSelection.StartCursorColumnIndex);
        Assert.AreEqual(0, _model.TextSelection.EndCursorLineIndex);
        Assert.AreEqual(20, _model.TextSelection.EndCursorColumnIndex);

        _model.MoveToNextToken();
        _model.MoveToNextToken();
        _model.MoveToNextToken();
        _model.MoveToNextToken();
        _model.MoveToNextToken();
        _model.MoveToNextToken();
        _model.MoveToNextToken();
        _model.CompleteSelection();
        Assert.AreEqual(0, _model.TextSelection.StartCursorLineIndex);
        Assert.AreEqual(0, _model.TextSelection.StartCursorColumnIndex);
        Assert.AreEqual(1, _model.TextSelection.EndCursorLineIndex);
        Assert.AreEqual(22, _model.TextSelection.EndCursorColumnIndex);
    }

    [Test]
    public void MoveToPrevToken()
    {
        _model.MoveCursorTo(new(1, 22));

        _model.MoveToPrevToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(20, _model.TextCursor.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(18, _model.TextCursor.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(15, _model.TextCursor.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(12, _model.TextCursor.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(5, _model.TextCursor.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(20, _model.TextCursor.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(14, _model.TextCursor.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(9, _model.TextCursor.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(7, _model.TextCursor.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);

        _model.MoveToPrevToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
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
        Assert.AreEqual(1, _model.TextSelection.StartCursorLineIndex);
        Assert.AreEqual(22, _model.TextSelection.StartCursorColumnIndex);
        Assert.AreEqual(1, _model.TextSelection.EndCursorLineIndex);
        Assert.AreEqual(0, _model.TextSelection.EndCursorColumnIndex);

        _model.MoveToPrevToken();
        _model.MoveToPrevToken();
        _model.MoveToPrevToken();
        _model.MoveToPrevToken();
        _model.MoveToPrevToken();
        _model.CompleteSelection();
        Assert.AreEqual(1, _model.TextSelection.StartCursorLineIndex);
        Assert.AreEqual(22, _model.TextSelection.StartCursorColumnIndex);
        Assert.AreEqual(0, _model.TextSelection.EndCursorLineIndex);
        Assert.AreEqual(0, _model.TextSelection.EndCursorColumnIndex);
    }

    [Test]
    public void DeleteLeftToken()
    {
        _model.MoveCursorTextEnd();

        _model.DeleteLeftToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(20, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("SELECT * FROM Table1\r\nJOIN Table2 ON t1 = ", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(18, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("SELECT * FROM Table1\r\nJOIN Table2 ON t1 ", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(15, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("SELECT * FROM Table1\r\nJOIN Table2 ON ", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(12, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("SELECT * FROM Table1\r\nJOIN Table2 ", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(5, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("SELECT * FROM Table1\r\nJOIN ", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(1, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("SELECT * FROM Table1\r\n", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(20, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("SELECT * FROM Table1", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(14, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("SELECT * FROM ", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(9, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("SELECT * ", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(7, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("SELECT ", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("", _model.Text.ToString());

        _model.DeleteLeftToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("", _model.Text.ToString());
    }

    [Test]
    public void DeleteRightToken()
    {
        _model.MoveCursorTextBegin();

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("* FROM Table1\r\nJOIN Table2 ON t1 = t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("FROM Table1\r\nJOIN Table2 ON t1 = t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("Table1\r\nJOIN Table2 ON t1 = t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("\r\nJOIN Table2 ON t1 = t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("JOIN Table2 ON t1 = t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("Table2 ON t1 = t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("ON t1 = t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("t1 = t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("= t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("t2", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("", _model.Text.ToString());

        _model.DeleteRightToken();
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
        Assert.AreEqual("", _model.Text.ToString());
    }
}
