using System.Collections.Generic;
using System.Windows.Media;

namespace CodeHighlighter.Model
{
    internal interface ILexemsColors
    {
        Brush? GetColorBrushOrNull(byte lexemKind);
    }

    internal class LexemsColors : ILexemsColors
    {
        private readonly Dictionary<byte, Brush> _colors = new();

        public void SetColors(IEnumerable<LexemColor> lexemColors)
        {
            _colors.Clear();
            foreach (var lexemColor in lexemColors)
            {
                _colors.Add(lexemColor.Kind, new SolidColorBrush(lexemColor.Color));
            }
        }

        public Brush? GetColorBrushOrNull(byte lexemKind)
        {
            return _colors.ContainsKey(lexemKind) ? _colors[lexemKind] : null;
        }
    }
}
