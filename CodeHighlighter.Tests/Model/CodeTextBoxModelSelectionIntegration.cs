using System.Linq;
using CodeHighlighter.CodeProviders;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model
{
    public class CodeTextBoxModelSelectionIntegration
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
        public void SelectionFromStartLineToEnd()
        {
            AppendString("0000000000");
            _model.MoveCursorStartLine();
            _model.StartSelection();
            _model.MoveCursorEndLine();
            _model.EndSelection();
            Assert.AreEqual(0, _model.TextSelection.StartLineIndex);
            Assert.AreEqual(0, _model.TextSelection.StartCursorColumnIndex);
            Assert.AreEqual(0, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(10, _model.TextSelection.EndCursorColumnIndex);
            Assert.True(_model.TextSelection.GetSelectedLines(_model.Text).Any());
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
            Assert.AreEqual(10, _model.TextSelection.StartCursorColumnIndex);
            Assert.AreEqual(0, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(0, _model.TextSelection.EndCursorColumnIndex);
            Assert.True(_model.TextSelection.GetSelectedLines(_model.Text).Any());
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
            Assert.AreEqual(5, _model.TextSelection.StartCursorColumnIndex);
            Assert.AreEqual(1, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(5, _model.TextSelection.EndCursorColumnIndex);
            Assert.True(_model.TextSelection.GetSelectedLines(_model.Text).Any());
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
            Assert.AreEqual(5, _model.TextSelection.StartCursorColumnIndex);
            Assert.AreEqual(0, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(5, _model.TextSelection.EndCursorColumnIndex);
            Assert.True(_model.TextSelection.GetSelectedLines(_model.Text).Any());
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
            Assert.AreEqual(4, _model.TextSelection.StartCursorColumnIndex);
            Assert.AreEqual(1, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(9, _model.TextSelection.EndCursorColumnIndex);
            Assert.True(_model.TextSelection.GetSelectedLines(_model.Text).Any());
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
            Assert.AreEqual(9, _model.TextSelection.StartCursorColumnIndex);
            Assert.AreEqual(0, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(4, _model.TextSelection.EndCursorColumnIndex);
            Assert.True(_model.TextSelection.GetSelectedLines(_model.Text).Any());
        }

        [Test]
        public void SelectionAll()
        {
            AppendString("0000000000");
            _model.NewLine();
            AppendString("0000000000");
            _model.SelectAll();
            Assert.AreEqual(0, _model.TextSelection.StartLineIndex);
            Assert.AreEqual(0, _model.TextSelection.StartCursorColumnIndex);
            Assert.AreEqual(1, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(10, _model.TextSelection.EndCursorColumnIndex);
            Assert.True(_model.TextSelection.GetSelectedLines(_model.Text).Any());
        }

        [Test]
        public void SelectionResetByClick()
        {
            AppendString("0000000000");
            _model.SelectAll();
            _model.MoveCursorTo(0, 5);
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
            Assert.AreEqual(1, _model.Tokens.LinesCount);
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
        public void SelectToken()
        {
            AppendString("SELECT Name FROM Table");

            _model.SelectToken(0, 0);
            Assert.AreEqual(0, _model.TextSelection.StartLineIndex);
            Assert.AreEqual(0, _model.TextSelection.StartCursorColumnIndex);
            Assert.AreEqual(0, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(6, _model.TextSelection.EndCursorColumnIndex);

            _model.SelectToken(0, 8);
            Assert.AreEqual(0, _model.TextSelection.StartLineIndex);
            Assert.AreEqual(7, _model.TextSelection.StartCursorColumnIndex);
            Assert.AreEqual(0, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(11, _model.TextSelection.EndCursorColumnIndex);

            _model.SelectToken(0, 12);
            Assert.AreEqual(0, _model.TextSelection.StartLineIndex);
            Assert.AreEqual(12, _model.TextSelection.StartCursorColumnIndex);
            Assert.AreEqual(0, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(16, _model.TextSelection.EndCursorColumnIndex);

            _model.SelectToken(0, 20);
            Assert.AreEqual(0, _model.TextSelection.StartLineIndex);
            Assert.AreEqual(17, _model.TextSelection.StartCursorColumnIndex);
            Assert.AreEqual(0, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(22, _model.TextSelection.EndCursorColumnIndex);
        }

        [Test]
        public void DeleteCurrentLine()
        {
            AppendString("123");
            _model.NewLine();
            AppendString("456");
            _model.NewLine();
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
            _model.NewLine();
            AppendString("456");
            _model.NewLine();
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
            _model.NewLine();
            AppendString("45600");
            _model.NewLine();
            AppendString("789");
            _model.MoveCursorTo(0, 3);

            _model.DeleteSelectedLines();
            Assert.AreEqual((0, 3), _model.TextCursor.GetLineAndColumnIndex);

            _model.MoveCursorTo(0, 5);
            _model.DeleteSelectedLines();
            Assert.AreEqual((0, 3), _model.TextCursor.GetLineAndColumnIndex);

            _model.DeleteSelectedLines();
            Assert.AreEqual((0, 0), _model.TextCursor.GetLineAndColumnIndex);

            _model.DeleteSelectedLines();
            Assert.AreEqual((0, 0), _model.TextCursor.GetLineAndColumnIndex);
        }

        [Test]
        public void DeleteLines_SelectionFirstTwoLines()
        {
            AppendString("123");
            _model.NewLine();
            AppendString("45600");
            _model.NewLine();
            AppendString("789");
            _model.MoveCursorTo(0, 2);
            _model.StartSelection();
            _model.MoveCursorTo(1, 5);
            _model.EndSelection();

            _model.DeleteSelectedLines();
            Assert.AreEqual((0, 2), _model.TextCursor.GetLineAndColumnIndex);
            Assert.AreEqual("789", _model.Text.ToString());
            Assert.AreEqual(1, _model.Tokens.LinesCount);
            Assert.AreEqual(1, _model.Tokens.GetMergedTokens(0).Count);
        }

        [Test]
        public void DeleteLines_SelectionLastTwoLines_1()
        {
            AppendString("123");
            _model.NewLine();
            AppendString("45600");
            _model.NewLine();
            AppendString("789");
            _model.MoveCursorTo(1, 2);
            _model.StartSelection();
            _model.MoveCursorTo(2, 3);
            _model.EndSelection();

            _model.DeleteSelectedLines();
            Assert.AreEqual((1, 0), _model.TextCursor.GetLineAndColumnIndex);
            Assert.AreEqual("123\r\n", _model.Text.ToString());
            Assert.AreEqual(2, _model.Tokens.LinesCount);
            Assert.AreEqual(1, _model.Tokens.GetMergedTokens(0).Count);
            Assert.AreEqual(0, _model.Tokens.GetMergedTokens(1).Count);
        }

        [Test]
        public void DeleteLines_SelectionLastTwoLines_2()
        {
            AppendString("123");
            _model.NewLine();
            AppendString("45600");
            _model.NewLine();
            AppendString("789");
            _model.MoveCursorTo(2, 3);
            _model.StartSelection();
            _model.MoveCursorTo(1, 2);
            _model.EndSelection();

            _model.DeleteSelectedLines();
            Assert.AreEqual((1, 0), _model.TextCursor.GetLineAndColumnIndex);
            Assert.AreEqual("123\r\n", _model.Text.ToString());
            Assert.AreEqual(2, _model.Tokens.LinesCount);
            Assert.AreEqual(1, _model.Tokens.GetMergedTokens(0).Count);
            Assert.AreEqual(0, _model.Tokens.GetMergedTokens(1).Count);
        }

        [Test]
        public void DeleteLines_All()
        {
            AppendString("123");
            _model.NewLine();
            AppendString("45600");
            _model.NewLine();
            AppendString("789");
            _model.SelectAll();

            _model.DeleteSelectedLines();

            Assert.AreEqual("", _model.Text.ToString());
            Assert.AreEqual(1, _model.Tokens.LinesCount);
            Assert.AreEqual(0, _model.Tokens.GetMergedTokens(0).Count);
        }

        private void AppendString(string str)
        {
            str.ToList().ForEach(_model.AppendChar);
        }
    }
}
