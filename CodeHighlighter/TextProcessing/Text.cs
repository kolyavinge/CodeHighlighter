using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.TextProcessing
{
    class Text
    {
        private readonly List<Line> _lines;

        public int LinesCount => _lines.Count;

        public Text(string text)
        {
            _lines = text.Split('\r', '\n').Select(line => new Line(line)).ToList();
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
            return _lines.Select(x => x.Length).Max();
        }
    }

    class Line
    {
        private readonly List<char> _symbs;

        public int Length => _symbs.Count;

        public char this[int i] => _symbs[i];

        public Line(string str)
        {
            _symbs = (str ?? "").ToCharArray().ToList();
        }

        public string GetSubstring(int startIndex, int length)
        {
            return new string(_symbs.Skip(startIndex).Take(length).ToArray());
        }

        public override string ToString()
        {
            return String.Join("", _symbs);
        }
    }
}
