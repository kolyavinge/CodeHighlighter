﻿using System.Linq;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model
{
    public class TextSelectionTest
    {
        private Text _text;
        private TextSelection _textSelection;

        [SetUp]
        public void Setup()
        {
            _text = new Text();
            _textSelection = new TextSelection();
        }

        [Test]
        public void GetTextSelectionLines()
        {
            _text.SetText("01234\n01234\n01234\n01234\n01234");
            _textSelection.StartLineIndex = 0;
            _textSelection.StartColumnIndex = 2;
            _textSelection.EndLineIndex = 3;
            _textSelection.EndColumnIndex = 4;

            var result = _textSelection.GetTextSelectionLines(_text).ToList();

            Assert.AreEqual(4, result.Count);

            Assert.AreEqual(0, result[0].LineIndex);
            Assert.AreEqual(2, result[0].LeftColumnIndex);
            Assert.AreEqual(5, result[0].RightColumnIndex);

            Assert.AreEqual(1, result[1].LineIndex);
            Assert.AreEqual(0, result[1].LeftColumnIndex);
            Assert.AreEqual(5, result[1].RightColumnIndex);

            Assert.AreEqual(2, result[2].LineIndex);
            Assert.AreEqual(0, result[2].LeftColumnIndex);
            Assert.AreEqual(5, result[2].RightColumnIndex);

            Assert.AreEqual(3, result[3].LineIndex);
            Assert.AreEqual(0, result[3].LeftColumnIndex);
            Assert.AreEqual(4, result[3].RightColumnIndex);
        }
    }
}
