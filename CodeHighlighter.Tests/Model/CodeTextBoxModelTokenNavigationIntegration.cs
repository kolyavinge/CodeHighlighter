using CodeHighlighter.CodeProviders;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model
{
    public class CodeTextBoxModelTokenNavigationIntegration
    {
        private CodeTextBoxModel _model;

        [SetUp]
        public void Setup()
        {
            _model = new CodeTextBoxModel();
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
            _model.StartSelection();
            _model.MoveToNextToken();
            _model.MoveToNextToken();
            _model.MoveToNextToken();
            _model.MoveToNextToken();
            Assert.AreEqual(0, _model.TextSelection.StartLineIndex);
            Assert.AreEqual(0, _model.TextSelection.StartCursorColumnIndex);
            Assert.AreEqual(0, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(20, _model.TextSelection.EndCursorColumnIndex);

            _model.MoveToNextToken();
            _model.MoveToNextToken();
            _model.MoveToNextToken();
            _model.MoveToNextToken();
            _model.MoveToNextToken();
            _model.MoveToNextToken();
            _model.MoveToNextToken();
            _model.EndSelection();
            Assert.AreEqual(0, _model.TextSelection.StartLineIndex);
            Assert.AreEqual(0, _model.TextSelection.StartCursorColumnIndex);
            Assert.AreEqual(1, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(22, _model.TextSelection.EndCursorColumnIndex);
        }

        [Test]
        public void MoveToPrevToken()
        {
            _model.MoveCursorTo(1, 22);

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
            _model.MoveCursorTo(1, 22);

            _model.StartSelection();
            _model.MoveToPrevToken();
            _model.MoveToPrevToken();
            _model.MoveToPrevToken();
            _model.MoveToPrevToken();
            _model.MoveToPrevToken();
            _model.MoveToPrevToken();
            Assert.AreEqual(1, _model.TextSelection.StartLineIndex);
            Assert.AreEqual(22, _model.TextSelection.StartCursorColumnIndex);
            Assert.AreEqual(1, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(0, _model.TextSelection.EndCursorColumnIndex);

            _model.MoveToPrevToken();
            _model.MoveToPrevToken();
            _model.MoveToPrevToken();
            _model.MoveToPrevToken();
            _model.MoveToPrevToken();
            _model.EndSelection();
            Assert.AreEqual(1, _model.TextSelection.StartLineIndex);
            Assert.AreEqual(22, _model.TextSelection.StartCursorColumnIndex);
            Assert.AreEqual(0, _model.TextSelection.EndLineIndex);
            Assert.AreEqual(0, _model.TextSelection.EndCursorColumnIndex);
        }
    }
}
