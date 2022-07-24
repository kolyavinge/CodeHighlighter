using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.CodeProvidering;

internal class EmptyCodeProvider : ICodeProvider
{
    public IEnumerable<Token> GetTokens(ITextIterator textIterator)
    {
        return Enumerable.Empty<Token>();
    }
    public IEnumerable<TokenColor> GetColors()
    {
        return Enumerable.Empty<TokenColor>();
    }
}
