﻿using CodeHighlighter.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model
{
    class Line : IEnumerable<char>
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

        public void AppendLine(Line appendedLine, int appendedLineColumnIndex, int appendedLineCount)
        {
            var endColumnIndex = appendedLineColumnIndex + appendedLineCount - 1;
            for (int i = appendedLineColumnIndex; i <= endColumnIndex; i++)
            {
                _symbs.Add(appendedLine._symbs[i]);
            }
        }

        public void InsertLine(int columnIndex, Line appendedLine)
        {
            for (int i = 0; i < appendedLine.Length; i++)
            {
                _symbs.Insert(columnIndex + i, appendedLine[i]);
            }
        }

        public void RemoveAt(int columnIndex)
        {
            _symbs.RemoveAt(columnIndex);
        }

        public void RemoveRange(int columnIndex, int count)
        {
            _symbs.RemoveRange(columnIndex, count);
        }

        public void Clear()
        {
            _symbs.Clear();
        }

        public override string ToString()
        {
            return String.Join("", _symbs);
        }

        public IEnumerator<char> GetEnumerator() => _symbs.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _symbs.GetEnumerator();
    }
}
