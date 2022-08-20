using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Utils;

namespace CodeHighlighter.Model;

public class TextLine : IEnumerable<char>
{
    private readonly List<char> _symbs;

    public int Length => _symbs.Count;

    public char this[int i] => _symbs[i];

    internal TextLine(string str)
    {
        _symbs = str.ToCharArray().ToList();
    }

    internal string GetSubstring(int startIndex, int length)
    {
        return new(_symbs.Skip(startIndex).Take(length).ToArray());
    }

    internal int FindIndex(int startIndex, int count, Predicate<char> match)
    {
        return _symbs.FindIndex(startIndex, count, match);
    }

    internal void AppendChar(int columnIndex, char ch)
    {
        _symbs.Insert(columnIndex, ch);
    }

    internal void AppendLine(TextLine line)
    {
        _symbs.AddRange(line._symbs);
    }

    internal void AppendLine(TextLine appendedLine, int appendedLineColumnIndex, int appendedLineCount)
    {
        var endColumnIndex = appendedLineColumnIndex + appendedLineCount - 1;
        for (int i = appendedLineColumnIndex; i <= endColumnIndex; i++)
        {
            _symbs.Add(appendedLine._symbs[i]);
        }
    }

    internal void InsertLine(int columnIndex, TextLine appendedLine)
    {
        for (int i = 0; i < appendedLine.Length; i++)
        {
            _symbs.Insert(columnIndex + i, appendedLine[i]);
        }
    }

    internal void RemoveAt(int columnIndex)
    {
        _symbs.RemoveAt(columnIndex);
    }

    internal void RemoveRange(int columnIndex, int count)
    {
        _symbs.RemoveRange(columnIndex, count);
    }

    internal void Clear()
    {
        _symbs.Clear();
    }

    internal void SetCase(int startColumnIndex, int length, TextCase textCase)
    {
        if (textCase == TextCase.Upper)
        {
            Enumerable.Range(startColumnIndex, length).Each(i => _symbs[i] = Char.ToUpper(_symbs[i]));
        }
        else
        {
            Enumerable.Range(startColumnIndex, length).Each(i => _symbs[i] = Char.ToLower(_symbs[i]));
        }
    }

    public override string ToString() => String.Join("", _symbs);

    public IEnumerator<char> GetEnumerator() => _symbs.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _symbs.GetEnumerator();
}
