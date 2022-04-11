using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.CodeProviders
{
    internal class EmptyCodeProvider : ICodeProvider
    {
        public IEnumerable<Lexem> GetLexems(ITextIterator textIterator)
        {
            return Enumerable.Empty<Lexem>();
        }
        public IEnumerable<LexemColor> GetColors()
        {
            return Enumerable.Empty<LexemColor>();
        }
    }
}
