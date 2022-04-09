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
            _model = new CodeTextBoxModel(new FontSettings());
            _model.SetCodeProvider(new SqlCodeProvider());
        }

        [Test]
        public void CursorPosition()
        {
            _model.SetText("");
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

        private void AppendString(string str)
        {
            str.ToList().ForEach(_model.AppendChar);
        }
    }
}
