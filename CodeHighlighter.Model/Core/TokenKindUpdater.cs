using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.CodeProvidering;

namespace CodeHighlighter.Core;

internal class TokenKindUpdater
{
    private readonly ITokensInternal _tokens;

    public TokenKindUpdater(ITokensInternal tokens)
    {
        _tokens = tokens;
    }

    public void UpdateTokenKinds(IEnumerable<UpdatedTokenKind> updatedTokenKinds)
    {
        var dictionary = updatedTokenKinds.ToDictionary(k => k.Name, v => v.Kind);
        foreach (var token in _tokens.AllTokens)
        {
            if (dictionary.ContainsKey(token.Name))
            {
                token.Kind = dictionary[token.Name];
            }
        }
    }
}
