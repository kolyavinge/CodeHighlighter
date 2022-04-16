using System.Linq;
using CodeHighlighter.CodeProviders;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model
{
    public class CodeTextBoxModelTest
    {
        private CodeTextBoxModel _model;

        [SetUp]
        public void Setup()
        {
            _model = new CodeTextBoxModel();
            _model.SetCodeProvider(new SqlCodeProvider());
            _model.SetText("");
        }

        [Test]
        public void CursorMove()
        {
            AppendString("0123456789");
            _model.NewLine();
            AppendString("0123456789");

            _model.MoveCursorTo(0, 5);
            _model.MoveCursorLeft();
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(4, _model.TextCursor.ColumnIndex);

            _model.MoveCursorRight();
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(5, _model.TextCursor.ColumnIndex);

            _model.MoveCursorDown();
            Assert.AreEqual(1, _model.TextCursor.LineIndex);
            Assert.AreEqual(5, _model.TextCursor.ColumnIndex);

            _model.MoveCursorUp();
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(5, _model.TextCursor.ColumnIndex);
        }

        [Test]
        public void CursorLimitsLeft()
        {
            AppendString("0123456789");

            _model.MoveCursorTextBegin();
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(0, _model.TextCursor.ColumnIndex);

            _model.MoveCursorTextBegin();
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(0, _model.TextCursor.ColumnIndex);

            _model.MoveCursorStartLine();
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(0, _model.TextCursor.ColumnIndex);

            _model.MoveCursorLeft();
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
        }

        [Test]
        public void CursorLimitsRight()
        {
            AppendString("0123456789");
            _model.NewLine();
            AppendString("0123456789");

            _model.MoveCursorTextEnd();
            Assert.AreEqual(1, _model.TextCursor.LineIndex);
            Assert.AreEqual(10, _model.TextCursor.ColumnIndex);

            _model.MoveCursorTextEnd();
            Assert.AreEqual(1, _model.TextCursor.LineIndex);
            Assert.AreEqual(10, _model.TextCursor.ColumnIndex);

            _model.MoveCursorEndLine();
            Assert.AreEqual(1, _model.TextCursor.LineIndex);
            Assert.AreEqual(10, _model.TextCursor.ColumnIndex);

            _model.MoveCursorRight();
            Assert.AreEqual(1, _model.TextCursor.LineIndex);
            Assert.AreEqual(10, _model.TextCursor.ColumnIndex);
        }

        [Test]
        public void CursorLimitsUp()
        {
            AppendString("0123456789");

            _model.MoveCursorTo(0, 5);
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(5, _model.TextCursor.ColumnIndex);

            _model.MoveCursorUp();
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(5, _model.TextCursor.ColumnIndex);

            _model.MoveCursorPageUp(1);
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(5, _model.TextCursor.ColumnIndex);
        }

        [Test]
        public void CursorLimitsDown()
        {
            AppendString("0123456789");
            _model.NewLine();
            AppendString("0123456789");

            _model.MoveCursorTo(1, 5);
            Assert.AreEqual(1, _model.TextCursor.LineIndex);
            Assert.AreEqual(5, _model.TextCursor.ColumnIndex);

            _model.MoveCursorDown();
            Assert.AreEqual(1, _model.TextCursor.LineIndex);
            Assert.AreEqual(5, _model.TextCursor.ColumnIndex);

            _model.MoveCursorPageDown(1);
            Assert.AreEqual(1, _model.TextCursor.LineIndex);
            Assert.AreEqual(5, _model.TextCursor.ColumnIndex);
        }

        [Test]
        public void CursorPageUp()
        {
            AppendString("0123456789");
            _model.NewLine();
            AppendString("0123456789");
            _model.NewLine();
            AppendString("0123456789");
            _model.NewLine();
            AppendString("0123456789");
            _model.NewLine();
            AppendString("0123456789");
            _model.MoveCursorTo(4, 5);

            _model.MoveCursorPageUp(3);
            Assert.AreEqual(1, _model.TextCursor.LineIndex);
            Assert.AreEqual(5, _model.TextCursor.ColumnIndex);

            _model.MoveCursorPageUp(3);
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(5, _model.TextCursor.ColumnIndex);
        }

        [Test]
        public void CursorPageDown()
        {
            AppendString("0123456789");
            _model.NewLine();
            AppendString("0123456789");
            _model.NewLine();
            AppendString("0123456789");
            _model.NewLine();
            AppendString("0123456789");
            _model.NewLine();
            AppendString("0123456789");
            _model.MoveCursorTo(0, 5);

            _model.MoveCursorPageDown(3);
            Assert.AreEqual(3, _model.TextCursor.LineIndex);
            Assert.AreEqual(5, _model.TextCursor.ColumnIndex);

            _model.MoveCursorPageDown(3);
            Assert.AreEqual(4, _model.TextCursor.LineIndex);
            Assert.AreEqual(5, _model.TextCursor.ColumnIndex);
        }

        [Test]
        public void CursorPositionAndTextEditing()
        {
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(0, _model.TextCursor.ColumnIndex);

            AppendString("DECLARE @x INT");
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(14, _model.TextCursor.ColumnIndex);

            _model.NewLine();
            AppendString("DECLARE @y FLOAT");
            Assert.AreEqual(1, _model.TextCursor.LineIndex);
            Assert.AreEqual(16, _model.TextCursor.ColumnIndex);

            Assert.AreEqual("DECLARE @x INT\r\nDECLARE @y FLOAT", _model.Text.ToString());

            _model.MoveCursorTextBegin();
            _model.MoveCursorRight();
            _model.MoveCursorRight();
            _model.NewLine();
            Assert.AreEqual(1, _model.TextCursor.LineIndex);
            Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
            Assert.AreEqual("DE\r\nCLARE @x INT\r\nDECLARE @y FLOAT", _model.Text.ToString());

            _model.LeftDelete();
            Assert.AreEqual("DECLARE @x INT\r\nDECLARE @y FLOAT", _model.Text.ToString());

            _model.MoveCursorTextBegin();
            _model.MoveCursorRight();
            _model.MoveCursorRight();
            _model.NewLine();
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
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(0, _model.TextCursor.ColumnIndex);

            _model.MoveCursorLeft();
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(0, _model.TextCursor.ColumnIndex);
        }

        [Test]
        public void MoveCursorRightFromLastLineEndPosition()
        {
            AppendString("0000000000");

            _model.MoveCursorEndLine();
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(10, _model.TextCursor.ColumnIndex);

            _model.MoveCursorRight();
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(10, _model.TextCursor.ColumnIndex);
        }

        [Test]
        public void MoveCursorFromOneLineToAnother()
        {
            AppendString("0000000000");
            _model.NewLine();
            AppendString("0000000000");
            _model.MoveCursorTextBegin();
            _model.MoveCursorEndLine();

            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(10, _model.TextCursor.ColumnIndex);

            _model.MoveCursorRight();
            Assert.AreEqual(1, _model.TextCursor.LineIndex);
            Assert.AreEqual(0, _model.TextCursor.ColumnIndex);

            _model.MoveCursorLeft();
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(10, _model.TextCursor.ColumnIndex);
        }

        [Test]
        public void SelectionFromStartLineToEnd()
        {
            AppendString("0000000000");
            _model.MoveCursorStartLine();
            _model.StartSelection();
            _model.MoveCursorEndLine();
            _model.EndSelection();
            Assert.AreEqual(0, _model.TextSelection.StartLineIndex);
            Assert.AreEqual(0, _model.TextSelection.StartColumnIndex);
            Assert.AreEqual(0, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(10, _model.TextSelection.EndColumnIndex);
            Assert.True(_model.TextSelection.GetTextSelectionLines(_model.Text).Any());
        }

        [Test]
        public void SelectionFromEndLineToStart()
        {
            AppendString("0000000000");
            _model.MoveCursorEndLine();
            _model.StartSelection();
            _model.MoveCursorStartLine();
            _model.EndSelection();
            Assert.AreEqual(0, _model.TextSelection.StartLineIndex);
            Assert.AreEqual(10, _model.TextSelection.StartColumnIndex);
            Assert.AreEqual(0, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(0, _model.TextSelection.EndColumnIndex);
            Assert.True(_model.TextSelection.GetTextSelectionLines(_model.Text).Any());
        }

        [Test]
        public void SelectionFromFirstLineToSecond()
        {
            AppendString("0000000000");
            _model.NewLine();
            AppendString("0000000000");
            _model.MoveCursorTo(0, 5);
            _model.StartSelection();
            _model.MoveCursorDown();
            _model.EndSelection();
            Assert.AreEqual(0, _model.TextSelection.StartLineIndex);
            Assert.AreEqual(5, _model.TextSelection.StartColumnIndex);
            Assert.AreEqual(1, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(5, _model.TextSelection.EndColumnIndex);
            Assert.True(_model.TextSelection.GetTextSelectionLines(_model.Text).Any());
        }

        [Test]
        public void SelectionFromSecondLineToFirst()
        {
            AppendString("0000000000");
            _model.NewLine();
            AppendString("0000000000");
            _model.MoveCursorTo(1, 5);
            _model.StartSelection();
            _model.MoveCursorUp();
            _model.EndSelection();
            Assert.AreEqual(1, _model.TextSelection.StartLineIndex);
            Assert.AreEqual(5, _model.TextSelection.StartColumnIndex);
            Assert.AreEqual(0, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(5, _model.TextSelection.EndColumnIndex);
            Assert.True(_model.TextSelection.GetTextSelectionLines(_model.Text).Any());
        }

        [Test]
        public void SelectionByCursor1()
        {
            AppendString("0000000000");
            _model.NewLine();
            AppendString("0000000000");
            _model.MoveCursorTo(0, 4);
            _model.StartSelection();
            _model.MoveCursorTo(1, 9);
            _model.EndSelection();
            Assert.AreEqual(0, _model.TextSelection.StartLineIndex);
            Assert.AreEqual(4, _model.TextSelection.StartColumnIndex);
            Assert.AreEqual(1, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(9, _model.TextSelection.EndColumnIndex);
            Assert.True(_model.TextSelection.GetTextSelectionLines(_model.Text).Any());
        }

        [Test]
        public void SelectionByCursor2()
        {
            AppendString("0000000000");
            _model.NewLine();
            AppendString("0000000000");
            _model.MoveCursorTo(1, 9);
            _model.StartSelection();
            _model.MoveCursorTo(0, 4);
            _model.EndSelection();
            Assert.AreEqual(1, _model.TextSelection.StartLineIndex);
            Assert.AreEqual(9, _model.TextSelection.StartColumnIndex);
            Assert.AreEqual(0, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(4, _model.TextSelection.EndColumnIndex);
            Assert.True(_model.TextSelection.GetTextSelectionLines(_model.Text).Any());
        }

        [Test]
        public void SelectionAll()
        {
            AppendString("0000000000");
            _model.NewLine();
            AppendString("0000000000");
            _model.SelectAll();
            Assert.AreEqual(0, _model.TextSelection.StartLineIndex);
            Assert.AreEqual(0, _model.TextSelection.StartColumnIndex);
            Assert.AreEqual(1, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(10, _model.TextSelection.EndColumnIndex);
            Assert.True(_model.TextSelection.GetTextSelectionLines(_model.Text).Any());
        }

        [Test]
        public void SelectionResetByClick()
        {
            AppendString("0000000000");
            _model.SelectAll();
            _model.MoveCursorTo(0, 5);
            Assert.False(_model.TextSelection.GetTextSelectionLines(_model.Text).Any());
        }

        [Test]
        public void SelectionResetByMoveCursor()
        {
            AppendString("0000000000");
            _model.SelectAll();
            _model.MoveCursorRight();
            Assert.False(_model.TextSelection.GetTextSelectionLines(_model.Text).Any());
        }

        [Test]
        public void DeleteSelection()
        {
            AppendString("0123456789");
            _model.NewLine();
            AppendString("9876543210");
            _model.MoveCursorTo(0, 3);
            _model.StartSelection();
            _model.MoveCursorTo(1, 7);
            _model.EndSelection();
            _model.LeftDelete();

            Assert.AreEqual("012210", _model.Text.ToString());
            Assert.AreEqual(0, _model.TextCursor.LineIndex);
            Assert.AreEqual(3, _model.TextCursor.ColumnIndex);
            Assert.AreEqual(1, _model.Lexems.Count);
        }

        [Test]
        public void GetSelectedText_NoSelection()
        {
            AppendString("0123456789");
            _model.MoveCursorTo(0, 5);
            var result = _model.GetSelectedText();
            Assert.AreEqual("", result);
        }

        [Test]
        public void GetSelectedText_OneLines()
        {
            AppendString("0123456789");
            _model.MoveCursorTo(0, 3);
            _model.StartSelection();
            _model.MoveCursorTo(0, 8);
            _model.EndSelection();
            var result = _model.GetSelectedText();
            Assert.AreEqual("34567", result);
        }

        [Test]
        public void GetSelectedText_OneLines_Whole()
        {
            AppendString("0123456789");
            _model.MoveCursorTo(0, 0);
            _model.StartSelection();
            _model.MoveCursorTo(0, 10);
            _model.EndSelection();
            var result = _model.GetSelectedText();
            Assert.AreEqual("0123456789", result);
        }

        [Test]
        public void GetSelectedText_MultyLines()
        {
            AppendString("0123456789");
            _model.NewLine();
            AppendString("0123456789");
            _model.MoveCursorTo(0, 3);
            _model.StartSelection();
            _model.MoveCursorTo(1, 2);
            _model.EndSelection();
            var result = _model.GetSelectedText();
            Assert.AreEqual("3456789\r\n01", result);
        }

        [Test]
        public void GetSelectedText_MultyLines_Whole()
        {
            AppendString("0123456789");
            _model.NewLine();
            AppendString("0123456789");
            _model.SelectAll();
            var result = _model.GetSelectedText();
            Assert.AreEqual("0123456789\r\n0123456789", result);
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
            _model.StartSelection();
            _model.MoveCursorTo(0, 7);
            _model.EndSelection();
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
}
