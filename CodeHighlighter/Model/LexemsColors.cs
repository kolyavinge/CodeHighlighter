using System.Collections.Generic;
using System.Windows.Media;

namespace CodeHighlighter.Model
{
    internal interface ILexemsColors
    {
        Brush? GetColorBrushOrNull(LexemKind lexemKind);
    }

    internal class LexemsColors : ILexemsColors
    {
        private readonly Dictionary<LexemKind, Brush> _colors = new();

        public void SetColors(IEnumerable<LexemColor> lexemColors)
        {
            _colors.Clear();
            foreach (var lexemColor in lexemColors)
            {
                _colors.Add(lexemColor.Kind, new SolidColorBrush(lexemColor.Color));
            }
        }

        public Brush? GetColorBrushOrNull(LexemKind lexemKind)
        {
            return _colors.ContainsKey(lexemKind) ? _colors[lexemKind] : null;
        }
    }
}
