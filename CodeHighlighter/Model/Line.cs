using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model
{
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

        public void AppendChar(int columnIndex, char ch)
        {
            _symbs.Insert(columnIndex, ch);
        }

        public void AppendLine(Line line)
        {
            _symbs.AddRange(line._symbs);
        }

        public void RemoveAt(int columnIndex)
        {
            _symbs.RemoveAt(columnIndex);
        }

        public void RemoveRange(int columnIndex, int count)
        {
            _symbs.RemoveRange(columnIndex, count);
        }

        public override string ToString()
        {
            return String.Join("", _symbs);
        }
    }
}
