using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model
{
    internal interface IText
    {
        int LinesCount { get; }
        int VisibleLinesCount { get; }
        string GetSubstring(int lineIndex, int startIndex, int length);
        Line GetLine(int lineIndex);
        Line GetFirstLine();
        Line GetLastLine();
        int GetMaxLineWidth();
    }

    internal class Text : IText
    {
        private readonly List<Line> _lines = new();

        public int LinesCount => _lines.Count;

        public int VisibleLinesCount => _lines.Count == 1 && !_lines[0].Any() ? 0 : _lines.Count;

        public Text() { }

        public Text(string text)
        {
            SetText(text);
        }

        public void SetText(string text)
        {
            _lines.Clear();
            _lines.AddRange(text.Split('\n').Select(line => new Line(line.Replace("\r", ""))).ToList());
        }

        public string GetSubstring(int lineIndex, int startIndex, int length)
        {
            return _lines[lineIndex].GetSubstring(startIndex, length);
        }

        public Line GetLine(int lineIndex) => _lines[lineIndex];

        public Line GetFirstLine() => _lines.First();

        public Line GetLastLine() => _lines.Last();

        public int GetMaxLineWidth()
        {
            if (!_lines.Any()) return 0;
            return _lines.Select(x => x.Length).Max();
        }

        public void NewLine(int lineIndex, int columnIndex)
        {
            var line = _lines[lineIndex];
            var remains = line.GetSubstring(columnIndex, line.Length - columnIndex);
            line.RemoveRange(columnIndex, line.Length - columnIndex);
            _lines.Insert(lineIndex + 1, new Line(remains));
        }

        public void AppendChar(int lineIndex, int columnIndex, char ch)
        {
            _lines[lineIndex].AppendChar(columnIndex, ch);
        }

        public void Insert(int lineIndex, int columnIndex, IText insertedText)
        {
            if (insertedText.LinesCount == 0) return;
            if (insertedText.LinesCount == 1)
            {
                _lines[lineIndex].InsertLine(columnIndex, insertedText.GetFirstLine());
            }
            else
            {
                NewLine(lineIndex, columnIndex);
                _lines[lineIndex].AppendLine(insertedText.GetFirstLine());
                for (int insertedLineIndex = 1; insertedLineIndex < insertedText.LinesCount - 1; insertedLineIndex++)
                {
                    _lines.Insert(lineIndex + insertedLineIndex, insertedText.GetLine(insertedLineIndex));
                }
                _lines[lineIndex + insertedText.LinesCount - 1].InsertLine(0, insertedText.GetLastLine());
            }
        }

        public (int, int) GetCursorPositionAfterLeftDelete(int currentLineIndex, int currentColumnIndex)
        {
            if (currentColumnIndex > 0)
            {
                return (currentLineIndex, currentColumnIndex - 1);
            }
            else if (currentLineIndex > 0)
            {
                return (currentLineIndex - 1, _lines[currentLineIndex - 1].Length);
            }
            else
            {
                return (currentLineIndex, currentColumnIndex);
            }
        }

        public DeleteResult LeftDelete(int lineIndex, int columnIndex)
        {
            if (columnIndex > 0)
            {
                _lines[lineIndex].RemoveAt(columnIndex - 1);
            }
            else if (lineIndex > 0)
            {
                _lines[lineIndex - 1].AppendLine(_lines[lineIndex]);
                _lines.RemoveAt(lineIndex);
                return new DeleteResult { IsLineDeleted = true };
            }

            return new DeleteResult { IsLineDeleted = false };
        }

        public DeleteResult RightDelete(int lineIndex, int columnIndex)
        {
            if (columnIndex < _lines[lineIndex].Length)
            {
                _lines[lineIndex].RemoveAt(columnIndex);
            }
            else if (lineIndex < _lines.Count - 1)
            {
                _lines[lineIndex].AppendLine(_lines[lineIndex + 1]);
                _lines.RemoveAt(lineIndex + 1);
                return new DeleteResult { IsLineDeleted = true };
            }

            return new DeleteResult { IsLineDeleted = false };
        }

        public DeleteSelectionResult DeleteSelection(ITextSelection textSelection)
        {
            var selectionLines = textSelection.GetTextSelectionLines(this).ToList();
            if (selectionLines.Count == 1)
            {
                var selectionLine = selectionLines.First();
                var line = _lines[selectionLine.LineIndex];
                line.RemoveRange(selectionLine.LeftColumnIndex, selectionLine.RightColumnIndex - selectionLine.LeftColumnIndex);

                return new DeleteSelectionResult();
            }
            else
            {
                var firstSelectionLine = selectionLines.First();
                var lastSelectionLine = selectionLines.Last();
                var firstLine = _lines[firstSelectionLine.LineIndex];
                var lastLine = _lines[lastSelectionLine.LineIndex];
                firstLine.RemoveRange(firstSelectionLine.LeftColumnIndex, firstSelectionLine.RightColumnIndex - firstSelectionLine.LeftColumnIndex);
                firstLine.AppendLine(lastLine, lastSelectionLine.RightColumnIndex, lastLine.Length - lastSelectionLine.RightColumnIndex);
                var secondSelectionLine = selectionLines.Skip(1).First();
                _lines.RemoveRange(secondSelectionLine.LineIndex, selectionLines.Count - 1);

                return new DeleteSelectionResult(secondSelectionLine.LineIndex, selectionLines.Count - 1);
            }
        }

        public override string ToString()
        {
            return String.Join(Environment.NewLine, _lines.Select(line => line.ToString()));
        }

        public struct DeleteResult
        {
            public bool IsLineDeleted;
        }

        public struct DeleteSelectionResult
        {
            public readonly int FirstDeletedLineIndex;
            public readonly int DeletedLinesCount;
            public DeleteSelectionResult(int firstDeletedLineIndex, int deletedLinesCount)
            {
                FirstDeletedLineIndex = firstDeletedLineIndex;
                DeletedLinesCount = deletedLinesCount;
            }
        }
    }
}
