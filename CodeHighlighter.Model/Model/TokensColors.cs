using System.Collections.Generic;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Common;

namespace CodeHighlighter.Model;

public interface ITokensColorCollection
{
    Color? GetColor(byte tokenKind);
}

public interface ITokensColors : ITokensColorCollection
{
    void SetColors(IEnumerable<TokenColor> tokenColors);
}

internal class TokensColors : ITokensColors
{
    private readonly Dictionary<byte, Color> _colors = new();

    public void SetColors(IEnumerable<TokenColor> tokenColors)
    {
        _colors.Clear();
        foreach (var tokenColor in tokenColors)
        {
            _colors.Add(tokenColor.Kind, tokenColor.Color);
        }
    }

    public Color? GetColor(byte tokenKind) => _colors.ContainsKey(tokenKind) ? _colors[tokenKind] : null;
}
