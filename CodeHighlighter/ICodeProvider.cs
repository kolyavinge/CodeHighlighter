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
        char Char { get; }
        char NextChar { get; }
        int LineIndex { get; }
        int ColumnIndex { get; }
        bool Eof { get; }
        void MoveNext();
    }

    public struct Lexem
    {
        public readonly int LineIndex;
        public readonly int StartColumnIndex;
        public readonly int Length;
        public readonly byte Kind;
        public int EndColumnIndex => StartColumnIndex + Length;

        public Lexem(int lineIndex, int startColumnIndex, int length, byte kind)
        {
            LineIndex = lineIndex;
            StartColumnIndex = startColumnIndex;
            Length = length;
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
