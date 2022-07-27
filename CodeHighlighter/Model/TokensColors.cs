using System.Collections.Generic;
using System.Windows.Media;
using CodeHighlighter.CodeProvidering;

namespace CodeHighlighter.Model;

internal class TokensColors
{
    private readonly Dictionary<byte, Brush> _colors = new();

    public void SetColors(IEnumerable<TokenColor> tokenColors)
    {
        _colors.Clear();
        foreach (var tokenColor in tokenColors)
        {
            _colors.Add(tokenColor.Kind, new SolidColorBrush(tokenColor.Color));
        }
    }

    public Brush? GetColorBrushOrNull(byte tokenKind)
    {
        return _colors.ContainsKey(tokenKind) ? _colors[tokenKind] : null;
    }
}
