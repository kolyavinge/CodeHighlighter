using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.CodeProvidering;

internal class EmptyCodeProvider : ICodeProvider
{
    public IEnumerable<ICodeProvider.Token> GetTokens(ITextIterator textIterator)
    {
        return Enumerable.Empty<ICodeProvider.Token>();
    }
    public IEnumerable<ICodeProvider.TokenColor> GetColors()
    {
        return Enumerable.Empty<ICodeProvider.TokenColor>();
    }
}
