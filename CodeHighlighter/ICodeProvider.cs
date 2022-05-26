using System.Collections.Generic;
using System.Windows.Media;

namespace CodeHighlighter;

public interface ICodeProvider
{
    IEnumerable<Token> GetTokens(ITextIterator textIterator);
    IEnumerable<TokenColor> GetColors();
}

public interface ITextIterator
{
    char Char { get; }
    char NextChar { get; }
    int LineIndex { get; }
    int ColumnIndex { get; }
    bool Eof { get; }
    void MoveNext();
}

public readonly struct Token
{
    public readonly int LineIndex;
    public readonly int StartColumnIndex;
    public readonly int Length;
    public readonly byte Kind;
    public int EndColumnIndex => StartColumnIndex + Length - 1;

    public Token(int lineIndex, int startColumnIndex, int length, byte kind)
    {
        LineIndex = lineIndex;
        StartColumnIndex = startColumnIndex;
        Length = length;
        Kind = kind;
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
