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
        public readonly int StartColumnIndex;
        public readonly byte Kind;

        public Lexem(int lineIndex, int startColumnIndex, byte kind)
        {
            LineIndex = lineIndex;
            StartColumnIndex = startColumnIndex;
            Kind = kind;
        }
    }

    public struct LexemColor
    {
        public readonly byte Kind;
        public readonly Color Color;

        public LexemColor(byte kind, Color color)
        {
            Kind = kind;
            Color = color;
        }
    }
}
