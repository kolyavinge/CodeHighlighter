using CodeHighlighter.Model;
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
        public void DeleteLine()
        {
            Assert.AreEqual(0, _lexems.LinesCount);
            _lexems.DeleteLine(0);
            Assert.AreEqual(0, _lexems.LinesCount);
        }
    }
}
