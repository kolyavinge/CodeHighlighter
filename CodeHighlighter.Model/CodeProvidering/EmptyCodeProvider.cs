using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.CodeProvidering;

public class EmptyCodeProvider : ICodeProvider
{
    public IEnumerable<Token> GetTokens(ITextIterator textIterator) => Enumerable.Empty<Token>();

    public IEnumerable<TokenColor> GetColors() => Enumerable.Empty<TokenColor>();
}
