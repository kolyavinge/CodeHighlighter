using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model
{
    internal interface ILexems
    {
        int LinesCount { get; }
        List<MergedLexem> GetMergedLexems(int lineIndex);
    }

    internal class Lexems : ILexems
    {
        private readonly List<List<Lexem>> _lexems = new();
        private readonly List<List<MergedLexem>> _mergedLexems = new();

        public int LinesCount => _lexems.Count;

        public void SetLexems(IEnumerable<Lexem> lexems, int startLineIndex, int linesCount)
        {
            var groupedLexems = lexems
                .GroupBy(x => x.LineIndex)
                .ToDictionary(group => group.Key, group => group.ToList());

            var length = startLineIndex + linesCount;
            for (int lineIndex = startLineIndex; lineIndex < length; lineIndex++)
            {
                var lineLexems = groupedLexems.ContainsKey(lineIndex) ? groupedLexems[lineIndex] : new List<Lexem>();
                if (lineIndex < _lexems.Count)
                {
                    _lexems[lineIndex] = lineLexems;
                    _mergedLexems[lineIndex] = MergeLexems(lineLexems);
                }
                else
                {
                    _lexems.Add(lineLexems);
                    _mergedLexems.Add(MergeLexems(lineLexems));
                }
            }
        }

        public List<MergedLexem> MergeLexems(IEnumerable<Lexem> lexems)
        {
            var result = new List<MergedLexem>();
            var lexemsArray = lexems.ToArray();
            int columnIndex = 0;
            int length;
            for (int i = 0; i < lexemsArray.Length;)
            {
                var kind = lexemsArray[i].Kind;
                i++;
                while (i < lexemsArray.Length && kind == lexemsArray[i].Kind) i++;
                if (i < lexemsArray.Length)
                {
                    length = lexemsArray[i].StartColumnIndex - columnIndex;
                    result.Add(new(columnIndex, length, kind));
                    columnIndex = lexemsArray[i].StartColumnIndex;
                }
                else
                {
                    var lineLength = lexems.Last().StartColumnIndex + lexems.Last().Length;
                    result.Add(new(columnIndex, lineLength - columnIndex, kind));
                }
            }

            return result;
        }

        public void InsertEmptyLine(int lineIndex)
        {
            _lexems.Insert(lineIndex, new List<Lexem>());
            _mergedLexems.Insert(lineIndex, new List<MergedLexem>());
        }

        public void DeleteLine(int lineIndex)
        {
            if (lineIndex == 0 && !_lexems.Any()) return;
            _lexems.RemoveAt(lineIndex);
            _mergedLexems.RemoveAt(lineIndex);
        }

        public void DeleteLines(int lineIndex, int count)
        {
            _lexems.RemoveRange(lineIndex, count);
            _mergedLexems.RemoveRange(lineIndex, count);
        }

        public List<MergedLexem> GetMergedLexems(int lineIndex)
        {
            return _mergedLexems[lineIndex];
        }

        public Lexem GetLexem(int lineIndex, int columnIndex)
        {
            if (lineIndex >= _lexems.Count) return default;
            var line = _lexems[lineIndex];
            if (columnIndex >= line.LastOrDefault().EndColumnIndex) return line.LastOrDefault();
            var index = line.FindIndex(x => x.StartColumnIndex <= columnIndex && columnIndex <= x.EndColumnIndex);
            if (index != -1)
            {
                if (index + 1 < line.Count && columnIndex == line[index + 1].StartColumnIndex) return line[index + 1];
                else return line[index];
            }
            else
            {
                return line.FirstOrDefault(x => x.StartColumnIndex > columnIndex);
            }
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
