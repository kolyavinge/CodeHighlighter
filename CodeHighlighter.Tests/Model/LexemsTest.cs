using System.Collections.Generic;
using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model
{
    public class LexemsTest
    {
        private Lexems _lexems;

        [SetUp]
        public void Setup()
        {
            _lexems = new Lexems();
        }

        [Test]
        public void SetLexems_Init()
        {
            var lexems = new List<Lexem>
            {
                new(0, 0, 2, 0),
                new(0, 2, 1, 1),
                new(2, 0, 1, 0),
                new(2, 1, 2, 1),
            };

            _lexems.SetLexems(lexems, 0, 3);

            Assert.AreEqual(3, _lexems.LinesCount);
            Assert.AreEqual(2, _lexems.GetMergedLexems(0).Count);
            Assert.AreEqual(0, _lexems.GetMergedLexems(1).Count);
            Assert.AreEqual(2, _lexems.GetMergedLexems(2).Count);
        }

        [Test]
        public void SetLexems_Replace()
        {
            var lexems = new List<Lexem>
            {
                new(0, 0, 2, 0),
                new(0, 2, 1, 1),
                new(2, 0, 1, 0),
                new(2, 1, 2, 1),
            };
            _lexems.SetLexems(lexems, 0, 3);

            lexems = new List<Lexem>
            {
                new(0, 0, 3, 0),
                new(1, 0, 1, 0),
                new(1, 1, 2, 1),
            };
            _lexems.SetLexems(lexems, 0, 2);

            Assert.AreEqual(3, _lexems.LinesCount);
            Assert.AreEqual(1, _lexems.GetMergedLexems(0).Count);
            Assert.AreEqual(2, _lexems.GetMergedLexems(1).Count);
            Assert.AreEqual(2, _lexems.GetMergedLexems(2).Count);
        }

        [Test]
        public void MergeLexems()
        {
            var lexems = new List<Lexem>
            {
                new(0, 1, 2, 0),
                new(0, 2, 2, 0),
                new(0, 4, 1, 1),
                new(0, 5, 2, 1),
                new(0, 7, 3, 2),
            };

            var result = _lexems.MergeLexems(lexems);

            Assert.AreEqual(3, result.Count);

            Assert.AreEqual(0, result[0].ColumnIndex);
            Assert.AreEqual(4, result[0].Length);
            Assert.AreEqual(0, result[0].Kind);

            Assert.AreEqual(4, result[1].ColumnIndex);
            Assert.AreEqual(3, result[1].Length);
            Assert.AreEqual(1, result[1].Kind);

            Assert.AreEqual(7, result[2].ColumnIndex);
            Assert.AreEqual(3, result[2].Length);
            Assert.AreEqual(2, result[2].Kind);
        }

        [Test]
        public void GetLexem_Empty()
        {
            _lexems.SetLexems(new List<Lexem>(), 0, 0);
            Assert.AreEqual(default(Lexem), _lexems.GetLexem(0, 0));
            Assert.AreEqual(default(Lexem), _lexems.GetLexem(0, 1));
            Assert.AreEqual(default(Lexem), _lexems.GetLexem(0, 10));
            Assert.AreEqual(default(Lexem), _lexems.GetLexem(1, 0));
            Assert.AreEqual(default(Lexem), _lexems.GetLexem(1, 1));
            Assert.AreEqual(default(Lexem), _lexems.GetLexem(1, 10));
        }

        [Test]
        public void GetLexem_EmptyLine()
        {
            _lexems.SetLexems(new List<Lexem>(), 0, 1);
            Assert.AreEqual(default(Lexem), _lexems.GetLexem(0, 0));
            Assert.AreEqual(default(Lexem), _lexems.GetLexem(0, 1));
            Assert.AreEqual(default(Lexem), _lexems.GetLexem(0, 10));
            Assert.AreEqual(default(Lexem), _lexems.GetLexem(1, 0));
            Assert.AreEqual(default(Lexem), _lexems.GetLexem(1, 1));
            Assert.AreEqual(default(Lexem), _lexems.GetLexem(1, 10));
        }

        [Test]
        public void GetLexem()
        {
            // '  xx  yzz'
            var lexems = new List<Lexem>
            {
                new(0, 2, 2, 0), // x
                new(0, 6, 1, 1), // y
                new(0, 7, 2, 2), // z
            };
            _lexems.SetLexems(lexems, 0, 1);

            Assert.AreEqual(new Lexem(0, 2, 2, 0), _lexems.GetLexem(0, 0));
            Assert.AreEqual(new Lexem(0, 2, 2, 0), _lexems.GetLexem(0, 1));
            Assert.AreEqual(new Lexem(0, 2, 2, 0), _lexems.GetLexem(0, 2));
            Assert.AreEqual(new Lexem(0, 2, 2, 0), _lexems.GetLexem(0, 3));
            Assert.AreEqual(new Lexem(0, 2, 2, 0), _lexems.GetLexem(0, 4));

            Assert.AreEqual(new Lexem(0, 6, 1, 1), _lexems.GetLexem(0, 5));
            Assert.AreEqual(new Lexem(0, 6, 1, 1), _lexems.GetLexem(0, 6));

            Assert.AreEqual(new Lexem(0, 7, 2, 2), _lexems.GetLexem(0, 7));
            Assert.AreEqual(new Lexem(0, 7, 2, 2), _lexems.GetLexem(0, 8));
            Assert.AreEqual(new Lexem(0, 7, 2, 2), _lexems.GetLexem(0, 9));
            Assert.AreEqual(new Lexem(0, 7, 2, 2), _lexems.GetLexem(0, 50));
        }

        [Test]
        public void DeleteLine()
        {
            Assert.AreEqual(0, _lexems.LinesCount);
            _lexems.DeleteLine(0);
            Assert.AreEqual(0, _lexems.LinesCount);
        }
    }
}
