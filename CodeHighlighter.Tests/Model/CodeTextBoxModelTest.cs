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

        private void AppendString(string str)
        {
            str.ToList().ForEach(_model.AppendChar);
        }
    }
}
