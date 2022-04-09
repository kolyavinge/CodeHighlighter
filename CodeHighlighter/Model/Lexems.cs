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
        private readonly List<MergedLexem[]> _lines = new();

        public void SetLexems(IText text, IReadOnlyCollection<Lexem> lexems)
        {
            _lines.Clear();
            if (!lexems.Any()) return;

            var merged = lexems
                .GroupBy(x => x.LineIndex)
                .ToDictionary(group => group.Key, group => MergeLexems(text.GetLine(group.Key), group.ToArray()).ToArray());

            var linesCount = merged.Keys.Max(x => x) + 1;
            for (int lineIndex = 0; lineIndex < linesCount; lineIndex++)
            {
                _lines.Add(merged.ContainsKey(lineIndex) ? merged[lineIndex] : new MergedLexem[0]);
            }
        }

        public void ReplaceLexems(IText text, IReadOnlyCollection<Lexem> lexems)
        {
            if (!lexems.Any()) return;

            var merged = lexems
                .GroupBy(x => x.LineIndex)
                .ToDictionary(group => group.Key, group => MergeLexems(text.GetLine(group.Key), group.ToArray()).ToArray());

            foreach (var lineLexems in merged)
            {
                if (lineLexems.Key < _lines.Count)
                {
                    _lines[lineLexems.Key] = lineLexems.Value;
                }
                else
                {
                    _lines.Add(new MergedLexem[0]);
                }
            }
        }

        public void InsertEmpty(int lineIndex)
        {
            _lines.Insert(lineIndex, new MergedLexem[0]);
        }

        public void RemoveAt(int lineIndex)
        {
            _lines.RemoveAt(lineIndex);
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
            return _lines[lineIndex];
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
