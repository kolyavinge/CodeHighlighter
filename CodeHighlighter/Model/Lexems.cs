using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model
{
    internal interface ILexems
    {
        MergedLexem[] GetLexemsForLine(int lineIndex);
    }

    internal class Lexems : ILexems
    {
        private readonly Dictionary<int, MergedLexem[]> _lines = new();

        public void SetLexems(IText text, IEnumerable<Lexem> lexems)
        {
            _lines.Clear();
            var groups = lexems.GroupBy(x => x.LineIndex).ToList();
            foreach (var group in groups)
            {
                if (_lines.ContainsKey(group.Key)) _lines.Remove(group.Key);
                _lines.Add(group.Key, MergeLexems(text.GetLine(group.Key), group.ToArray()).ToArray());
            }
        }

        private IEnumerable<MergedLexem> MergeLexems(Line line, Lexem[] lexems)
        {
            int columnIndex = 0;
            int length;
            for (int i = 0; i < lexems.Length;)
            {
                var kind = lexems[i].Kind;
                i++;
                while (i < lexems.Length && kind == lexems[i].Kind) i++;
                if (i < lexems.Length)
                {
                    length = lexems[i].ColumnIndex - columnIndex;
                    yield return new(columnIndex, length, kind);
                    columnIndex = lexems[i].ColumnIndex;
                }
                else
                {
                    length = line.Length;
                    yield return new(columnIndex, length, kind);
                }
            }
        }

        public MergedLexem[] GetLexemsForLine(int lineIndex)
        {
            if (_lines.ContainsKey(lineIndex))
            {
                return _lines[lineIndex];
            }
            else
            {
                return Array.Empty<MergedLexem>();
            }
        }
    }

    internal struct MergedLexem
    {
        public readonly int ColumnIndex;
        public readonly int Length;
        public readonly LexemKind Kind;

        public MergedLexem(int columnIndex, int length, LexemKind kind)
        {
            ColumnIndex = columnIndex;
            Length = length;
            Kind = kind;
        }
    }
}
