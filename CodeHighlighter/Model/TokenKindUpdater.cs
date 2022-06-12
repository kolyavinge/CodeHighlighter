using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model;

internal class TokenKindUpdater
{
    private readonly ITokens _tokens;

    public TokenKindUpdater(ITokens tokens)
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
