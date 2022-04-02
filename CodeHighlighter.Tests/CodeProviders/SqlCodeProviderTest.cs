using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeHighlighter.CodeProviders;
using CodeHighlighter.TextProcessing;
using NUnit.Framework;

namespace CodeHighlighter.Tests.CodeProviders
{
    public class SqlCodeProviderTest
    {
        private SqlCodeProvider _provider;

        [SetUp]
        public void Init()
        {
            _provider = new SqlCodeProvider();
        }

        [Test]
        public void Query()
        {
            var text = "select * from MyTable";
            var lexems = GetLexems(text);
            Assert.AreEqual(4, lexems.Count);
            int i = 0;
            Assert.AreEqual(0, lexems[i].ColumnIndex);
            Assert.AreEqual(LexemKind.Keyword, lexems[i].Kind);
            i++;
            Assert.AreEqual(7, lexems[i].ColumnIndex);
            Assert.AreEqual(LexemKind.Delimiter, lexems[i].Kind);
            i++;
            Assert.AreEqual(9, lexems[i].ColumnIndex);
            Assert.AreEqual(LexemKind.Keyword, lexems[i].Kind);
            i++;
            Assert.AreEqual(14, lexems[i].ColumnIndex);
            Assert.AreEqual(LexemKind.Identifier, lexems[i].Kind);
        }

        [Test]
        public void QueryTempTable()
        {
            var text = "select * from #TempTable";
            var lexems = GetLexems(text);
            Assert.AreEqual(4, lexems.Count);
            Assert.AreEqual(LexemKind.Identifier, lexems[3].Kind);
        }

        [Test]
        public void QueryWithWhere()
        {
            var text = "select * from MyTable where a > 1";
            var lexems = GetLexems(text);
            Assert.AreEqual(8, lexems.Count);
            int i = 0;
            Assert.AreEqual(0, lexems[i].ColumnIndex);
            Assert.AreEqual(LexemKind.Keyword, lexems[i].Kind);
            i++;
            Assert.AreEqual(7, lexems[i].ColumnIndex);
            Assert.AreEqual(LexemKind.Delimiter, lexems[i].Kind);
            i++;
            Assert.AreEqual(9, lexems[i].ColumnIndex);
            Assert.AreEqual(LexemKind.Keyword, lexems[i].Kind);
            i++;
            Assert.AreEqual(14, lexems[i].ColumnIndex);
            Assert.AreEqual(LexemKind.Identifier, lexems[i].Kind);
            i++;
            Assert.AreEqual(22, lexems[i].ColumnIndex);
            Assert.AreEqual(LexemKind.Keyword, lexems[i].Kind);
            i++;
            Assert.AreEqual(28, lexems[i].ColumnIndex);
            Assert.AreEqual(LexemKind.Identifier, lexems[i].Kind);
            i++;
            Assert.AreEqual(30, lexems[i].ColumnIndex);
            Assert.AreEqual(LexemKind.Delimiter, lexems[i].Kind);
            i++;
            Assert.AreEqual(32, lexems[i].ColumnIndex);
            Assert.AreEqual(LexemKind.Other, lexems[i].Kind);
        }

        [Test]
        public void Unescape()
        {
            var text = "select [group] from MyTable";
            var lexems = GetLexems(text);
            Assert.AreEqual(4, lexems.Count);
            Assert.AreEqual(7, lexems[1].ColumnIndex);
            Assert.AreEqual(LexemKind.Identifier, lexems[1].Kind);
        }

        [Test]
        public void Unescape2()
        {
            var text = "select [group]from MyTable";
            var lexems = GetLexems(text);

            Assert.AreEqual(4, lexems.Count);

            Assert.AreEqual(7, lexems[1].ColumnIndex);
            Assert.AreEqual(LexemKind.Identifier, lexems[1].Kind);

            Assert.AreEqual(14, lexems[2].ColumnIndex);
            Assert.AreEqual(LexemKind.Keyword, lexems[2].Kind);
        }

        [Test]
        public void UnescapeWithDelimiter()
        {
            var text = "select [group-1] from MyTable";
            var lexems = GetLexems(text);
            Assert.AreEqual(4, lexems.Count);
            Assert.AreEqual(LexemKind.Identifier, lexems[1].Kind);
        }

        [Test]
        public void Comments()
        {
            var text = "select * from MyTable -- comments";
            var lexems = GetLexems(text);
            Assert.AreEqual(5, lexems.Count);
            Assert.AreEqual(22, lexems[4].ColumnIndex);
            Assert.AreEqual(LexemKind.Comment, lexems[4].Kind);
        }

        [Test]
        public void Strings()
        {
            var text = "'select [group] from MyTable'";
            var lexems = GetLexems(text);
            Assert.AreEqual(1, lexems.Count);
            Assert.AreEqual(0, lexems[0].ColumnIndex);
            Assert.AreEqual(LexemKind.String, lexems[0].Kind);
        }

        [Test]
        public void StringsMultiline()
        {
            var text = @"'select [group]
from MyTable'";
            var lexems = GetLexems(text);
            Assert.AreEqual(1, lexems.Count);
            Assert.AreEqual(0, lexems[0].ColumnIndex);
            Assert.AreEqual(LexemKind.String, lexems[0].Kind);
        }

        [Test]
        public void Variables()
        {
            var text = "declare @var int";
            var lexems = GetLexems(text);
            Assert.AreEqual(3, lexems.Count);
            Assert.AreEqual(LexemKind.Keyword, lexems[0].Kind);
            Assert.AreEqual(LexemKind.Variable, lexems[1].Kind);
            Assert.AreEqual(LexemKind.Keyword, lexems[2].Kind);
        }

        [Test]
        public void N()
        {
            var text = "N'string'";
            var lexems = GetLexems(text);
            Assert.AreEqual(2, lexems.Count);
            Assert.AreEqual(LexemKind.Other, lexems[0].Kind);
            Assert.AreEqual(LexemKind.String, lexems[1].Kind);
        }

        [Test]
        public void SQLFile_1()
        {
            var text = File.ReadAllText(@"D:\Projects\CodeHighlighter\CodeHighlighter.Tests\SQLFiles\1.sql");
            var lexems = GetLexems(text);
            Assert.AreEqual(637, lexems.Count);
            lexems.ForEach(x => Assert.IsTrue(x.ColumnIndex <= text.Length));
        }

        [Test]
        public void SQLFile_2()
        {
            var text = File.ReadAllText(@"D:\Projects\CodeHighlighter\CodeHighlighter.Tests\SQLFiles\2.sql");
            var lexems = GetLexems(text);
            Assert.AreEqual(1744, lexems.Count);
            lexems.ForEach(x => Assert.IsTrue(x.ColumnIndex <= text.Length));
        }

        [Test]
        public void SQLFile_3()
        {
            var text = File.ReadAllText(@"D:\Projects\CodeHighlighter\CodeHighlighter.Tests\SQLFiles\3.sql");
            var lexems = GetLexems(text);
            Assert.AreEqual(31058, lexems.Count);
            lexems.ForEach(x => Assert.IsTrue(x.ColumnIndex <= text.Length));
        }

        private List<Lexem> GetLexems(string text)
        {
            return _provider.GetLexems(new TextIterator(new Text(text))).ToList();
        }
    }
}
