using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model
{
    internal interface ILexems
    {
        int LinesCount { get; }
        List<MergedLexem> GetLine(int lineIndex);
    }

    internal class Lexems : ILexems
    {
        private readonly List<List<MergedLexem>> _lines = new();

        public int LinesCount => _lines.Count;

        public void SetLexems(IText text, IReadOnlyCollection<Lexem> lexems)
        {
            _lines.Clear();
            if (!lexems.Any()) return;

            var merged = lexems
                .GroupBy(x => x.LineIndex)
                .ToDictionary(group => group.Key, group => MergeLexems(text.GetLine(group.Key), group.ToList()).ToList());

            for (int lineIndex = 0; lineIndex < text.LinesCount; lineIndex++)
            {
                _lines.Add(merged.ContainsKey(lineIndex) ? merged[lineIndex] : new List<MergedLexem>());
            }
        }

        public void ReplaceLexems(IText text, IReadOnlyCollection<Lexem> lexems, int startLineIndex, int linesCount)
        {
            var merged = lexems
                .GroupBy(x => x.LineIndex)
                .ToDictionary(group => group.Key, group => MergeLexems(text.GetLine(group.Key), group.ToList()).ToList());

            var length = startLineIndex + linesCount;
            for (int lineIndex = startLineIndex; lineIndex < length; lineIndex++)
            {
                var lineLexems = merged.ContainsKey(lineIndex) ? merged[lineIndex] : new List<MergedLexem>();
                if (lineIndex < _lines.Count)
                {
                    _lines[lineIndex] = lineLexems;
                }
                else
                {
                    _lines.Add(lineLexems);
                }
            }
        }

        public void InsertEmpty(int lineIndex)
        {
            _lines.Insert(lineIndex, new List<MergedLexem>());
        }

        public void DeleteLine(int lineIndex)
        {
            if (lineIndex == 0 && !_lines.Any()) return;
            _lines.RemoveAt(lineIndex);
        }

        public void DeleteLines(int lineIndex, int count)
        {
            _lines.RemoveRange(lineIndex, count);
        }

        private IEnumerable<MergedLexem> MergeLexems(Line line, List<Lexem> lexems)
        {
            int columnIndex = 0;
            int length;
            for (int i = 0; i < lexems.Count;)
            {
                var kind = lexems[i].Kind;
                i++;
                while (i < lexems.Count && kind == lexems[i].Kind) i++;
                if (i < lexems.Count)
                {
                    length = lexems[i].StartColumnIndex - columnIndex;
                    yield return new(columnIndex, length, kind);
                    columnIndex = lexems[i].StartColumnIndex;
                }
                else
                {
                    length = line.Length;
                    yield return new(columnIndex, length, kind);
                }
            }
        }

        public List<MergedLexem> GetLine(int lineIndex)
        {
            return _lines[lineIndex];
        }
    }

    internal struct MergedLexem
    {
        public readonly int ColumnIndex;
        public readonly int Length;
        public readonly byte Kind;

        public MergedLexem(int columnIndex, int length, byte kind)
        {
            ColumnIndex = columnIndex;
            Length = length;
            Kind = kind;
        }
    }
}
