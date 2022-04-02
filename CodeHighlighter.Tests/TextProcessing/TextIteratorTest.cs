using CodeHighlighter.TextProcessing;
using NUnit.Framework;

namespace CodeHighlighter.Tests.TextProcessing
{
    class TextIteratorTest
    {
        private TextIterator _iterator;

        [Test]
        public void Empty()
        {
            Init("");

            Assert.AreEqual(0, _iterator.Char);
            Assert.AreEqual(0, _iterator.NextChar);
            Assert.AreEqual(0, _iterator.LineIndex);
            Assert.AreEqual(0, _iterator.ColumnIndex);
            Assert.True(_iterator.Eof);
        }

        [Test]
        public void Return()
        {
            Init("\n");

            Assert.AreEqual('\n', _iterator.Char);
            Assert.AreEqual(0, _iterator.NextChar);
            Assert.AreEqual(0, _iterator.LineIndex);
            Assert.AreEqual(0, _iterator.ColumnIndex);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual(0, _iterator.Char);
            Assert.AreEqual(0, _iterator.NextChar);
            Assert.AreEqual(1, _iterator.LineIndex);
            Assert.AreEqual(0, _iterator.ColumnIndex);
            Assert.True(_iterator.Eof);
        }

        [Test]
        public void OneLine()
        {
            Init("123");

            Assert.AreEqual('1', _iterator.Char);
            Assert.AreEqual('2', _iterator.NextChar);
            Assert.AreEqual(0, _iterator.LineIndex);
            Assert.AreEqual(0, _iterator.ColumnIndex);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual('2', _iterator.Char);
            Assert.AreEqual('3', _iterator.NextChar);
            Assert.AreEqual(0, _iterator.LineIndex);
            Assert.AreEqual(1, _iterator.ColumnIndex);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual('3', _iterator.Char);
            Assert.AreEqual(0, _iterator.NextChar);
            Assert.AreEqual(0, _iterator.LineIndex);
            Assert.AreEqual(2, _iterator.ColumnIndex);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual(0, _iterator.LineIndex);
            Assert.AreEqual(3, _iterator.ColumnIndex);
            Assert.AreEqual(0, _iterator.Char);
            Assert.True(_iterator.Eof);
        }

        [Test]
        public void OneLineAndReturn()
        {
            Init("123\n");

            Assert.AreEqual('1', _iterator.Char);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual('2', _iterator.Char);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual('3', _iterator.Char);
            Assert.AreEqual('\n', _iterator.NextChar);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual('\n', _iterator.Char);
            Assert.AreEqual(0, _iterator.NextChar);
            Assert.AreEqual(0, _iterator.LineIndex);
            Assert.AreEqual(3, _iterator.ColumnIndex);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual(0, _iterator.Char);
            Assert.True(_iterator.Eof);
        }

        [Test]
        public void TwoLines()
        {
            Init("123\n456");

            Assert.AreEqual('1', _iterator.Char);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual('2', _iterator.Char);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual('3', _iterator.Char);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual('\n', _iterator.Char);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual('4', _iterator.Char);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual('5', _iterator.Char);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual('6', _iterator.Char);
            Assert.AreEqual(1, _iterator.LineIndex);
            Assert.AreEqual(2, _iterator.ColumnIndex);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual(0, _iterator.Char);
            Assert.AreEqual(1, _iterator.LineIndex);
            Assert.AreEqual(3, _iterator.ColumnIndex);
            Assert.True(_iterator.Eof);
        }

        [Test]
        public void TwoLinesAndReturn()
        {
            Init("123\n456\n");

            Assert.AreEqual('1', _iterator.Char);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual('2', _iterator.Char);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual('3', _iterator.Char);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual('\n', _iterator.Char);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual('4', _iterator.Char);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual('5', _iterator.Char);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual('6', _iterator.Char);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual('\n', _iterator.Char);
            Assert.AreEqual(1, _iterator.LineIndex);
            Assert.AreEqual(3, _iterator.ColumnIndex);
            Assert.False(_iterator.Eof);

            _iterator.MoveNext();
            Assert.AreEqual(0, _iterator.Char);
            Assert.AreEqual(2, _iterator.LineIndex);
            Assert.AreEqual(0, _iterator.ColumnIndex);
            Assert.True(_iterator.Eof);
        }

        private void Init(string text)
        {
            _iterator = new TextIterator(new Text(text));
        }
    }
}
