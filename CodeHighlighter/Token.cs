using System;
using System.Windows.Media;

namespace CodeHighlighter;

public class Token
{
    public readonly string Name;
    public readonly int LineIndex;
    public readonly int StartColumnIndex;
    public readonly int Length;
    public readonly byte Kind;
    public int EndColumnIndex => StartColumnIndex + Length - 1;

    public Token(string name, int lineIndex, int startColumnIndex, int length, byte kind)
    {
        Name = name;
        LineIndex = lineIndex;
        StartColumnIndex = startColumnIndex;
        Length = length;
        Kind = kind;
    }

    public override bool Equals(object? obj)
    {
        return obj is Token token &&
               Name == token.Name &&
               LineIndex == token.LineIndex &&
               StartColumnIndex == token.StartColumnIndex &&
               Length == token.Length &&
               Kind == token.Kind;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, LineIndex, StartColumnIndex, Length, Kind);
    }
}

public readonly struct TokenColor
{
    public readonly byte Kind;
    public readonly Color Color;

    public TokenColor(byte kind, Color color)
    {
        Kind = kind;
        Color = color;
    }
}
