using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model
{
    public class TextTest
    {
        private Text _text;

        [Test]
        public void NewLine_First()
        {
            SetText("123");
            _text.NewLine(0, 0);
            Assert.AreEqual("\r\n123", _text.ToString());
        }

        [Test]
        public void NewLine_Middle()
        {
            SetText("123");
            _text.NewLine(0, 2);
            Assert.AreEqual("12\r\n3", _text.ToString());
        }

        [Test]
        public void NewLine_Last()
        {
            SetText("123");
            _text.NewLine(0, 3);
            Assert.AreEqual("123\r\n", _text.ToString());
        }

        [Test]
        public void AppendChar_First()
        {
            SetText("123");
            _text.AppendChar(0, 0, 'a');
            Assert.AreEqual("a123", _text.ToString());
        }

        [Test]
        public void AppendChar_Middle()
        {
            SetText("123");
            _text.AppendChar(0, 1, 'a');
            Assert.AreEqual("1a23", _text.ToString());
        }

        [Test]
        public void AppendChar_Last()
        {
            SetText("123");
            _text.AppendChar(0, 3, 'a');
            Assert.AreEqual("123a", _text.ToString());
        }

        [Test]
        public void LeftDelete_First()
        {
            SetText("123");
            _text.LeftDelete(0, 0);
            Assert.AreEqual("123", _text.ToString());
        }

        [Test]
        public void LeftDelete_Middle()
        {
            SetText("123");
            _text.LeftDelete(0, 1);
            Assert.AreEqual("23", _text.ToString());
        }

        [Test]
        public void LeftDelete_Last()
        {
            SetText("123");
            _text.LeftDelete(0, 3);
            Assert.AreEqual("12", _text.ToString());
        }

        [Test]
        public void LeftDelete_Return()
        {
            SetText("123\n456");
            var result = _text.LeftDelete(1, 0);
            Assert.AreEqual("123456", _text.ToString());
            Assert.True(result.IsLineDeleted);
        }

        [Test]
        public void RightDelete_First()
        {
            SetText("123");
            _text.RightDelete(0, 0);
            Assert.AreEqual("23", _text.ToString());
        }

        [Test]
        public void RightDelete_Middle()
        {
            SetText("123");
            _text.RightDelete(0, 1);
            Assert.AreEqual("13", _text.ToString());
        }

        [Test]
        public void RightDelete_Last()
        {
            SetText("123");
            _text.RightDelete(0, 2);
            Assert.AreEqual("12", _text.ToString());
        }

        [Test]
        public void RightDelete_EndLine()
        {
            SetText("123");
            _text.RightDelete(0, 3);
            Assert.AreEqual("123", _text.ToString());
        }

        [Test]
        public void RightDelete_Return()
        {
            SetText("123\n456");
            var result = _text.RightDelete(0, 3);
            Assert.AreEqual("123456", _text.ToString());
            Assert.True(result.IsLineDeleted);
        }

        [Test]
        public void DeleteSelection_OneLine_Begin()
        {
            SetText("012345");
            var result = _text.DeleteSelection(new TextSelection(0, 0, 0, 4));
            Assert.AreEqual("45", _text.ToString());
            Assert.AreEqual(0, result.FirstDeletedLineIndex);
            Assert.AreEqual(0, result.DeletedLinesCount);
        }

        [Test]
        public void DeleteSelection_OneLine_End()
        {
            SetText("012345");
            _text.DeleteSelection(new TextSelection(0, 1, 0, 6));
            Assert.AreEqual("0", _text.ToString());
        }

        [Test]
        public void DeleteSelection_OneLine_Middle()
        {
            SetText("012345");
            _text.DeleteSelection(new TextSelection(0, 1, 0, 5));
            Assert.AreEqual("05", _text.ToString());
        }

        [Test]
        public void DeleteSelection_OneLine_All()
        {
            SetText("012345");
            _text.DeleteSelection(new TextSelection(0, 0, 0, 6));
            Assert.AreEqual("", _text.ToString());
        }

        [Test]
        public void DeleteSelection_ManyLines_Begin()
        {
            SetText("012345\n012345");
            var result = _text.DeleteSelection(new TextSelection(0, 0, 1, 4));
            Assert.AreEqual("45", _text.ToString());
            Assert.AreEqual(1, result.FirstDeletedLineIndex);
            Assert.AreEqual(1, result.DeletedLinesCount);
        }

        [Test]
        public void DeleteSelection_ManyLines_Middle()
        {
            SetText("012345\n012345");
            var result = _text.DeleteSelection(new TextSelection(0, 2, 1, 4));
            Assert.AreEqual("0145", _text.ToString());
            Assert.AreEqual(1, result.FirstDeletedLineIndex);
            Assert.AreEqual(1, result.DeletedLinesCount);
        }

        [Test]
        public void DeleteSelection_ManyLines_End()
        {
            SetText("012345\n012345");
            var result = _text.DeleteSelection(new TextSelection(0, 2, 1, 6));
            Assert.AreEqual("01", _text.ToString());
            Assert.AreEqual(1, result.FirstDeletedLineIndex);
            Assert.AreEqual(1, result.DeletedLinesCount);
        }

        [Test]
        public void DeleteSelection_ManyLines_DeleteFirst()
        {
            SetText("012345\n555555");
            var result = _text.DeleteSelection(new TextSelection(0, 0, 0, 6));
            Assert.AreEqual("\r\n555555", _text.ToString());
            Assert.AreEqual(0, result.FirstDeletedLineIndex);
            Assert.AreEqual(0, result.DeletedLinesCount);
        }

        [Test]
        public void DeleteSelection_ManyLines_DeleteLast()
        {
            SetText("012345\n555555");
            var result = _text.DeleteSelection(new TextSelection(1, 0, 1, 6));
            Assert.AreEqual("012345\r\n", _text.ToString());
            Assert.AreEqual(0, result.FirstDeletedLineIndex);
            Assert.AreEqual(0, result.DeletedLinesCount);
        }

        [Test]
        public void DeleteSelection_ManyLines_All()
        {
            SetText("012345\n012345");
            var result = _text.DeleteSelection(new TextSelection(0, 0, 1, 6));
            Assert.AreEqual("", _text.ToString());
            Assert.AreEqual(1, result.FirstDeletedLineIndex);
            Assert.AreEqual(1, result.DeletedLinesCount);
        }

        private void SetText(string textString)
        {
            _text = new Text();
            _text.SetText(textString);
        }
    }
}
