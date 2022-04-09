using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model
{
    internal interface IText
    {
        int LinesCount { get; }
        string GetSubstring(int lineIndex, int startIndex, int length);
        Line GetLine(int lineIndex);
        int GetMaxLineWidth();
    }

    internal class Text : IText
    {
        private readonly List<Line> _lines = new();

        public int LinesCount => _lines.Count;

        public void SetText(string text)
        {
            _lines.Clear();
            _lines.AddRange(text.Split('\n').Select(line => new Line(line.Replace("\r", ""))).ToList());
        }

        public string GetSubstring(int lineIndex, int startIndex, int length)
        {
            return _lines[lineIndex].GetSubstring(startIndex, length);
        }

        public Line GetLine(int lineIndex)
        {
            return _lines[lineIndex];
        }

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

        public override string ToString()
        {
            return String.Join(Environment.NewLine, _lines.Select(line => line.ToString()));
        }

        public struct DeleteResult
        {
            public bool IsLineDeleted;
        }
    }
}
