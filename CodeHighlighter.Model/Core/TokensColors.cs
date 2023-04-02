using System.Collections;
using System.Collections.Generic;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Common;

namespace CodeHighlighter.Core;

public interface ITokensColors : IEnumerable<Color>
{
    Color? GetColor(byte tokenKind);
}

internal class TokensColors : ITokensColors
{
    private readonly Dictionary<byte, Color> _colors = new();

    public TokensColors(ICodeProvider codeProvider)
    {
        SetColors(codeProvider.GetColors());
    }

    private void SetColors(IEnumerable<TokenColor> tokenColors)
    {
        _colors.Clear();
        foreach (var tokenColor in tokenColors)
        {
            _colors.Add(tokenColor.Kind, tokenColor.Color);
        }
    }

    public Color? GetColor(byte tokenKind) => _colors.ContainsKey(tokenKind) ? _colors[tokenKind] : null;

    public IEnumerator<Color> GetEnumerator() => _colors.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _colors.Values.GetEnumerator();
}
