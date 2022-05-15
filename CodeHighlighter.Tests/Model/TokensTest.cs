using System.Collections.Generic;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model
{
    public class TokensTest
    {
        private Tokens _tokens;

        [SetUp]
        public void Setup()
        {
            _tokens = new Tokens();
        }

        [Test]
        public void SetTokens_Init()
        {
            var tokens = new List<Token>
            {
                new(0, 0, 2, 0),
                new(0, 2, 1, 1),
                new(2, 0, 1, 0),
                new(2, 1, 2, 1),
            };

            _tokens.SetTokens(tokens, 0, 3);

            Assert.AreEqual(3, _tokens.LinesCount);
            Assert.AreEqual(2, _tokens.GetMergedTokens(0).Count);
            Assert.AreEqual(0, _tokens.GetMergedTokens(1).Count);
            Assert.AreEqual(2, _tokens.GetMergedTokens(2).Count);
        }

        [Test]
        public void SetTokens_Replace()
        {
            var tokens = new List<Token>
            {
                new(0, 0, 2, 0),
                new(0, 2, 1, 1),
                new(2, 0, 1, 0),
                new(2, 1, 2, 1),
            };
            _tokens.SetTokens(tokens, 0, 3);

            tokens = new List<Token>
            {
                new(0, 0, 3, 0),
                new(1, 0, 1, 0),
                new(1, 1, 2, 1),
            };
            _tokens.SetTokens(tokens, 0, 2);

            Assert.AreEqual(3, _tokens.LinesCount);
            Assert.AreEqual(1, _tokens.GetMergedTokens(0).Count);
            Assert.AreEqual(2, _tokens.GetMergedTokens(1).Count);
            Assert.AreEqual(2, _tokens.GetMergedTokens(2).Count);
        }

        [Test]
        public void MergeTokens()
        {
            var tokens = new List<LineToken>
            {
                new(1, 2, 0),
                new(2, 2, 0),
                new(4, 1, 1),
                new(5, 2, 1),
                new(7, 3, 2),
            };

            var result = _tokens.MergeTokens(tokens);

            Assert.AreEqual(3, result.Count);

            Assert.AreEqual(0, result[0].StartColumnIndex);
            Assert.AreEqual(4, result[0].Length);
            Assert.AreEqual(0, result[0].Kind);

            Assert.AreEqual(4, result[1].StartColumnIndex);
            Assert.AreEqual(3, result[1].Length);
            Assert.AreEqual(1, result[1].Kind);

            Assert.AreEqual(7, result[2].StartColumnIndex);
            Assert.AreEqual(3, result[2].Length);
            Assert.AreEqual(2, result[2].Kind);
        }

        [Test]
        public void DeleteLine()
        {
            Assert.AreEqual(0, _tokens.LinesCount);
            _tokens.DeleteLine(0);
            Assert.AreEqual(0, _tokens.LinesCount);
        }
    }
}
