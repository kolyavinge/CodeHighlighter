using System.Linq;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class InputModelSelectionIntegration
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
    public void SelectionFromStartLineToEnd()
    {
        AppendString("0000000000");
        _model.MoveCursorStartLine();
        _model.ActivateSelection();
        _model.MoveCursorEndLine();
        _model.CompleteSelection();
        Assert.AreEqual(new CursorPosition(0, 0), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(0, 10), _model.TextSelection.EndPosition);
        Assert.True(_model.TextSelection.GetSelectedLines(_model.Text).Any());
    }

    [Test]
    public void SelectionFromEndLineToStart()
    {
        AppendString("0000000000");
        _model.MoveCursorEndLine();
        _model.ActivateSelection();
        _model.MoveCursorStartLine();
        _model.CompleteSelection();
        Assert.AreEqual(new CursorPosition(0, 10), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(0, 0), _model.TextSelection.EndPosition);
        Assert.True(_model.TextSelection.GetSelectedLines(_model.Text).Any());
    }

    [Test]
    public void SelectionFromFirstLineToSecond()
    {
        AppendString("0000000000");
        _model.AppendNewLine();
        AppendString("0000000000");
        _model.MoveCursorTo(new(0, 5));
        _model.ActivateSelection();
        _model.MoveCursorDown();
        _model.CompleteSelection();
        Assert.AreEqual(new CursorPosition(0, 5), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(1, 5), _model.TextSelection.EndPosition);
        Assert.True(_model.TextSelection.GetSelectedLines(_model.Text).Any());
    }

    [Test]
    public void SelectionFromSecondLineToFirst()
    {
        AppendString("0000000000");
        _model.AppendNewLine();
        AppendString("0000000000");
        _model.MoveCursorTo(new(1, 5));
        _model.ActivateSelection();
        _model.MoveCursorUp();
        _model.CompleteSelection();
        Assert.AreEqual(new CursorPosition(1, 5), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(0, 5), _model.TextSelection.EndPosition);
        Assert.True(_model.TextSelection.GetSelectedLines(_model.Text).Any());
    }

    [Test]
    public void SelectionByCursor1()
    {
        AppendString("0000000000");
        _model.AppendNewLine();
        AppendString("0000000000");
        _model.MoveCursorTo(new(0, 4));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 9));
        _model.CompleteSelection();
        Assert.AreEqual(new CursorPosition(0, 4), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(1, 9), _model.TextSelection.EndPosition);
        Assert.True(_model.TextSelection.GetSelectedLines(_model.Text).Any());
    }

    [Test]
    public void SelectionByCursor2()
    {
        AppendString("0000000000");
        _model.AppendNewLine();
        AppendString("0000000000");
        _model.MoveCursorTo(new(1, 9));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(0, 4));
        _model.CompleteSelection();
        Assert.AreEqual(new CursorPosition(1, 9), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(0, 4), _model.TextSelection.EndPosition);
        Assert.True(_model.TextSelection.GetSelectedLines(_model.Text).Any());
    }

    [Test]
    public void SelectionAll()
    {
        AppendString("0000000000");
        _model.AppendNewLine();
        AppendString("0000000000");
        _model.SelectAll();
        Assert.AreEqual(new CursorPosition(0, 0), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(1, 10), _model.TextSelection.EndPosition);
        Assert.True(_model.TextSelection.GetSelectedLines(_model.Text).Any());
    }

    [Test]
    public void SelectionResetByClick()
    {
        AppendString("0000000000");
        _model.SelectAll();
        _model.MoveCursorTo(new(0, 5));
        Assert.False(_model.TextSelection.GetSelectedLines(_model.Text).Any());
    }

    [Test]
    public void SelectionResetByMoveCursor()
    {
        AppendString("0000000000");
        _model.SelectAll();
        _model.MoveCursorRight();
        Assert.False(_model.TextSelection.GetSelectedLines(_model.Text).Any());
    }

    [Test]
    public void SelectionAfterInputText()
    {
        AppendString("00000");
        _model.AppendNewLine();
        _model.AppendChar('1');
        _model.AppendChar('2');
        _model.AppendChar('3');
        _model.ActivateSelection();
        _model.MoveCursorStartLine();
        _model.CompleteSelection();
        Assert.AreEqual("123", _model.GetSelectedText());
    }

    [Test]
    public void SelectionAfterNewLine()
    {
        AppendString("00000");
        _model.AppendNewLine();
        _model.ActivateSelection();
        _model.MoveCursorUp();
        _model.CompleteSelection();
        Assert.AreEqual("00000\r\n", _model.GetSelectedText());
    }

    [Test]
    public void SelectionActiveAppendChar_DeleteSelection()
    {
        AppendString("000");
        _model.MoveCursorStartLine();
        _model.ActivateSelection();
        _model.MoveCursorEndLine();
        _model.AppendChar('0');
        Assert.AreEqual("0", _model.Text.ToString());
    }

    [Test]
    public void SelectionActiveNewLine_DeleteSelection()
    {
        AppendString("000");
        _model.MoveCursorStartLine();
        _model.ActivateSelection();
        _model.MoveCursorEndLine();
        _model.AppendNewLine();
        Assert.AreEqual("\r\n", _model.Text.ToString());
    }

    [Test]
    public void DeleteSelection_LeftDelete()
    {
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("9876543210");
        _model.MoveCursorTo(new(0, 3));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 7));
        _model.CompleteSelection();
        _model.LeftDelete();

        Assert.AreEqual("012210", _model.Text.ToString());
        Assert.AreEqual(1, _model.Tokens.LinesCount);
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(3, _model.TextCursor.ColumnIndex);
    }

    [Test]
    public void DeleteSelection_RightDelete()
    {
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("9876543210");
        _model.MoveCursorTo(new(0, 3));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 7));
        _model.CompleteSelection();
        _model.RightDelete();

        Assert.AreEqual("012210", _model.Text.ToString());
        Assert.AreEqual(1, _model.Tokens.LinesCount);
        Assert.AreEqual(0, _model.TextCursor.LineIndex);
        Assert.AreEqual(3, _model.TextCursor.ColumnIndex);
    }

    [Test]
    public void GetSelectedText_NoSelection()
    {
        AppendString("0123456789");
        _model.MoveCursorTo(new(0, 5));
        var result = _model.GetSelectedText();
        Assert.AreEqual("", result);
    }

    [Test]
    public void GetSelectedText_OneLines()
    {
        AppendString("0123456789");
        _model.MoveCursorTo(new(0, 3));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(0, 8));
        _model.CompleteSelection();
        var result = _model.GetSelectedText();
        Assert.AreEqual("34567", result);
    }

    [Test]
    public void GetSelectedText_OneLines_Whole()
    {
        AppendString("0123456789");
        _model.MoveCursorTo(new(0, 0));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(0, 10));
        _model.CompleteSelection();
        var result = _model.GetSelectedText();
        Assert.AreEqual("0123456789", result);
    }

    [Test]
    public void GetSelectedText_MultyLines()
    {
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("0123456789");
        _model.MoveCursorTo(new(0, 3));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 2));
        _model.CompleteSelection();
        var result = _model.GetSelectedText();
        Assert.AreEqual("3456789\r\n01", result);
    }

    [Test]
    public void GetSelectedText_MultyLines_Whole()
    {
        AppendString("0123456789");
        _model.AppendNewLine();
        AppendString("0123456789");
        _model.SelectAll();
        var result = _model.GetSelectedText();
        Assert.AreEqual("0123456789\r\n0123456789", result);
    }

    [Test]
    public void SelectToken()
    {
        AppendString("SELECT Name FROM Table");

        _model.SelectToken(new(0, 0));
        Assert.AreEqual(new CursorPosition(0, 0), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(0, 6), _model.TextSelection.EndPosition);
        Assert.AreEqual(new CursorPosition(0, 6), _model.TextCursor.Position);

        _model.SelectToken(new(0, 8));
        Assert.AreEqual(new CursorPosition(0, 7), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(0, 11), _model.TextSelection.EndPosition);
        Assert.AreEqual(new CursorPosition(0, 11), _model.TextCursor.Position);

        _model.SelectToken(new(0, 12));
        Assert.AreEqual(new CursorPosition(0, 12), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(0, 16), _model.TextSelection.EndPosition);
        Assert.AreEqual(new CursorPosition(0, 16), _model.TextCursor.Position);

        _model.SelectToken(new(0, 20));
        Assert.AreEqual(new CursorPosition(0, 17), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(0, 22), _model.TextSelection.EndPosition);
        Assert.AreEqual(new CursorPosition(0, 22), _model.TextCursor.Position);
    }

    [Test]
    public void DeleteCurrentLine()
    {
        AppendString("123");
        _model.AppendNewLine();
        AppendString("456");
        _model.AppendNewLine();
        AppendString("789");
        _model.MoveCursorTextBegin();

        _model.DeleteSelectedLines();
        Assert.AreEqual("456\r\n789", _model.Text.ToString());

        _model.DeleteSelectedLines();
        Assert.AreEqual("789", _model.Text.ToString());

        _model.DeleteSelectedLines();
        Assert.AreEqual("", _model.Text.ToString());

        _model.DeleteSelectedLines();
        Assert.AreEqual("", _model.Text.ToString());
    }

    [Test]
    public void DeleteLastLine_Clear()
    {
        AppendString("123");
        _model.AppendNewLine();
        AppendString("456");
        _model.AppendNewLine();
        AppendString("789");
        _model.MoveCursorTextEnd();

        _model.DeleteSelectedLines();
        Assert.AreEqual("123\r\n456\r\n", _model.Text.ToString());

        _model.DeleteSelectedLines();
        Assert.AreEqual("123\r\n456\r\n", _model.Text.ToString());
    }

    [Test]
    public void DeleteLines_Cursor()
    {
        AppendString("123");
        _model.AppendNewLine();
        AppendString("45600");
        _model.AppendNewLine();
        AppendString("789");
        _model.MoveCursorTo(new(0, 3));

        _model.DeleteSelectedLines();
        Assert.AreEqual(new CursorPosition(0, 0), _model.TextCursor.Position);

        _model.MoveCursorTo(new(0, 5));
        _model.DeleteSelectedLines();
        Assert.AreEqual(new CursorPosition(0, 0), _model.TextCursor.Position);

        _model.DeleteSelectedLines();
        Assert.AreEqual(new CursorPosition(0, 0), _model.TextCursor.Position);

        _model.DeleteSelectedLines();
        Assert.AreEqual(new CursorPosition(0, 0), _model.TextCursor.Position);
    }

    [Test]
    public void DeleteLines_SelectionFirstTwoLines()
    {
        AppendString("123");
        _model.AppendNewLine();
        AppendString("45600");
        _model.AppendNewLine();
        AppendString("789");
        _model.MoveCursorTo(new(0, 2));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 5));
        _model.CompleteSelection();

        _model.DeleteSelectedLines();
        Assert.AreEqual(new CursorPosition(0, 0), _model.TextCursor.Position);
        Assert.AreEqual("789", _model.Text.ToString());
        Assert.AreEqual(1, _model.Tokens.LinesCount);
        Assert.AreEqual(1, _model.Tokens.GetTokens(0).Count);
    }

    [Test]
    public void DeleteLines_SelectionLastTwoLines_1()
    {
        AppendString("123");
        _model.AppendNewLine();
        AppendString("45600");
        _model.AppendNewLine();
        AppendString("789");
        _model.MoveCursorTo(new(1, 2));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(2, 3));
        _model.CompleteSelection();

        _model.DeleteSelectedLines();
        Assert.AreEqual(new CursorPosition(1, 0), _model.TextCursor.Position);
        Assert.AreEqual("123\r\n", _model.Text.ToString());
        Assert.AreEqual(2, _model.Tokens.LinesCount);
        Assert.AreEqual(1, _model.Tokens.GetTokens(0).Count);
        Assert.AreEqual(0, _model.Tokens.GetTokens(1).Count);
    }

    [Test]
    public void DeleteLines_SelectionLastTwoLines_2()
    {
        AppendString("123");
        _model.AppendNewLine();
        AppendString("45600");
        _model.AppendNewLine();
        AppendString("789");
        _model.MoveCursorTo(new(2, 3));
        _model.ActivateSelection();
        _model.MoveCursorTo(new(1, 2));
        _model.CompleteSelection();

        _model.DeleteSelectedLines();
        Assert.AreEqual(new CursorPosition(1, 0), _model.TextCursor.Position);
        Assert.AreEqual("123\r\n", _model.Text.ToString());
        Assert.AreEqual(2, _model.Tokens.LinesCount);
        Assert.AreEqual(1, _model.Tokens.GetTokens(0).Count);
        Assert.AreEqual(0, _model.Tokens.GetTokens(1).Count);
    }

    [Test]
    public void DeleteLines_All()
    {
        AppendString("123");
        _model.AppendNewLine();
        AppendString("45600");
        _model.AppendNewLine();
        AppendString("789");
        _model.SelectAll();

        _model.DeleteSelectedLines();

        Assert.AreEqual("", _model.Text.ToString());
        Assert.AreEqual(1, _model.Tokens.LinesCount);
        Assert.AreEqual(0, _model.Tokens.GetTokens(0).Count);
    }

    [Test]
    public void MoveSelectedLinesUp_OneLine()
    {
        _model.SetText("123\n456\n789");
        _model.MoveCursorTo(new(1, 2));

        _model.MoveSelectedLinesUp();
        Assert.AreEqual("456\r\n123\r\n789", _model.Text.ToString());
        Assert.AreEqual(new CursorPosition(0, 2), _model.TextCursor.Position);

        _model.MoveSelectedLinesUp();
        Assert.AreEqual("456\r\n123\r\n789", _model.Text.ToString());
        Assert.AreEqual(new CursorPosition(0, 2), _model.TextCursor.Position);
    }

    [Test]
    public void MoveSelectedLinesUp_TwoLines_1()
    {
        _model.SetText("123\n456\n789");

        _model.MoveCursorTo(new(1, 2));
        _model.ActivateSelection();
        _model.MoveCursorDown();
        _model.CompleteSelection();
        _model.MoveSelectedLinesUp();
        Assert.AreEqual("456\r\n789\r\n123", _model.Text.ToString());
        Assert.AreEqual(new CursorPosition(1, 2), _model.TextCursor.Position);
        Assert.AreEqual(true, _model.TextSelection.IsExist);
        Assert.AreEqual(new CursorPosition(0, 2), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(1, 2), _model.TextSelection.EndPosition);

        _model.MoveSelectedLinesUp();
        Assert.AreEqual("456\r\n789\r\n123", _model.Text.ToString());
        Assert.AreEqual(new CursorPosition(1, 2), _model.TextCursor.Position);
        Assert.AreEqual(true, _model.TextSelection.IsExist);
        Assert.AreEqual(new CursorPosition(0, 2), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(1, 2), _model.TextSelection.EndPosition);
    }

    [Test]
    public void MoveSelectedLinesUp_TwoLines_2()
    {
        _model.SetText("123\n456\n789");

        _model.MoveCursorTo(new(2, 2));
        _model.ActivateSelection();
        _model.MoveCursorUp();
        _model.CompleteSelection();
        _model.MoveSelectedLinesUp();
        Assert.AreEqual("456\r\n789\r\n123", _model.Text.ToString());
        Assert.AreEqual(new CursorPosition(0, 2), _model.TextCursor.Position);
        Assert.AreEqual(true, _model.TextSelection.IsExist);
        Assert.AreEqual(new CursorPosition(1, 2), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(0, 2), _model.TextSelection.EndPosition);

        _model.MoveSelectedLinesUp();
        Assert.AreEqual("456\r\n789\r\n123", _model.Text.ToString());
        Assert.AreEqual(new CursorPosition(0, 2), _model.TextCursor.Position);
        Assert.AreEqual(true, _model.TextSelection.IsExist);
        Assert.AreEqual(new CursorPosition(1, 2), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(0, 2), _model.TextSelection.EndPosition);
    }

    [Test]
    public void MoveSelectedLinesDown_OneLine()
    {
        _model.SetText("123\n456\n789");
        _model.MoveCursorTo(new(1, 2));

        _model.MoveSelectedLinesDown();
        Assert.AreEqual("123\r\n789\r\n456", _model.Text.ToString());
        Assert.AreEqual(new CursorPosition(2, 2), _model.TextCursor.Position);

        _model.MoveSelectedLinesDown();
        Assert.AreEqual("123\r\n789\r\n456", _model.Text.ToString());
        Assert.AreEqual(new CursorPosition(2, 2), _model.TextCursor.Position);
    }

    [Test]
    public void MoveSelectedLinesDown_TwoLines_1()
    {
        _model.SetText("123\n456\n789");

        _model.MoveCursorTo(new(0, 2));
        _model.ActivateSelection();
        _model.MoveCursorDown();
        _model.CompleteSelection();
        _model.MoveSelectedLinesDown();
        Assert.AreEqual("789\r\n123\r\n456", _model.Text.ToString());
        Assert.AreEqual(new CursorPosition(2, 2), _model.TextCursor.Position);
        Assert.AreEqual(true, _model.TextSelection.IsExist);
        Assert.AreEqual(new CursorPosition(1, 2), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(2, 2), _model.TextSelection.EndPosition);

        _model.MoveSelectedLinesDown();
        Assert.AreEqual("789\r\n123\r\n456", _model.Text.ToString());
        Assert.AreEqual(new CursorPosition(2, 2), _model.TextCursor.Position);
        Assert.AreEqual(true, _model.TextSelection.IsExist);
        Assert.AreEqual(new CursorPosition(1, 2), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(2, 2), _model.TextSelection.EndPosition);
    }

    [Test]
    public void MoveSelectedLinesDown_TwoLines_2()
    {
        _model.SetText("123\n456\n789");

        _model.MoveCursorTo(new(1, 2));
        _model.ActivateSelection();
        _model.MoveCursorUp();
        _model.CompleteSelection();
        _model.MoveSelectedLinesDown();
        Assert.AreEqual("789\r\n123\r\n456", _model.Text.ToString());
        Assert.AreEqual(new CursorPosition(1, 2), _model.TextCursor.Position);
        Assert.AreEqual(true, _model.TextSelection.IsExist);
        Assert.AreEqual(new CursorPosition(2, 2), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(1, 2), _model.TextSelection.EndPosition);

        _model.MoveSelectedLinesDown();
        Assert.AreEqual("789\r\n123\r\n456", _model.Text.ToString());
        Assert.AreEqual(new CursorPosition(1, 2), _model.TextCursor.Position);
        Assert.AreEqual(true, _model.TextSelection.IsExist);
        Assert.AreEqual(new CursorPosition(2, 2), _model.TextSelection.StartPosition);
        Assert.AreEqual(new CursorPosition(1, 2), _model.TextSelection.EndPosition);
    }

    private void AppendString(string str)
    {
        str.ToList().ForEach(ch => _model.AppendChar(ch));
    }
}
