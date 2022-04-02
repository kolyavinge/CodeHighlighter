using System.Collections.Generic;
using System.Windows.Media;

namespace CodeHighlighter
{
    public interface ICodeProvider
    {
        IEnumerable<Lexem> GetLexems(ITextIterator textIterator);
        IEnumerable<LexemColor> GetColors();
    }

    public interface ITextIterator
    {
        void MoveNext();
        char Char { get; }
        int LineIndex { get; }
        int ColumnIndex { get; }
        bool Eof { get; }
        bool IsReturn { get; }
        bool IsSpace { get; }
        char NextChar { get; }
    }

    public struct Lexem
    {
        public readonly int LineIndex;
        public readonly int ColumnIndex;
        public readonly LexemKind Kind;

        public Lexem(int lineIndex, int columnIndex, LexemKind kind)
        {
            LineIndex = lineIndex;
            ColumnIndex = columnIndex;
            Kind = kind;
        }
    }

    public struct LexemColor
    {
        public readonly LexemKind Kind;
        public readonly Color Color;

        public LexemColor(LexemKind kind, Color color)
        {
            Kind = kind;
            Color = color;
        }
    }

    public enum LexemKind
    {
        Identifier,
        Keyword,
        Operator,
        Function,
        Method,
        Property,
        Variable,
        Constant,
        String,
        Comment,
        Delimiter,
        Other
    }
}
