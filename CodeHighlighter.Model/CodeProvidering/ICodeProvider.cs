using System.Collections.Generic;
using System.Xml.Linq;
using CodeHighlighter.Common;
using CodeHighlighter.Model;

namespace CodeHighlighter.CodeProvidering;

public interface ICodeProvider
{
    IEnumerable<Token> GetTokens(ITextIterator textIterator);
    IEnumerable<TokenColor> GetColors();
}

public class Token
{
    public readonly string Name;
    public readonly int LineIndex;
    public readonly int StartColumnIndex;
    public readonly byte Kind;
    public int EndColumnIndex => StartColumnIndex + Name.Length - 1;

    public Token(string name, int lineIndex, int startColumnIndex, byte kind)
    {
        Name = name;
        LineIndex = lineIndex;
        StartColumnIndex = startColumnIndex;
        Kind = kind;
    }

    public override bool Equals(object? obj) => obj is Token token &&
        Name == token.Name &&
        LineIndex == token.LineIndex &&
        StartColumnIndex == token.StartColumnIndex &&
        Kind == token.Kind;

    public override int GetHashCode() => HashCode.Combine(Name, LineIndex, StartColumnIndex, Kind);
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

public interface ITokenKindUpdatable
{
    event EventHandler<TokenKindUpdatedEventArgs> TokenKindUpdated;
}

public class TokenKindUpdatedEventArgs : EventArgs
{
    public IEnumerable<UpdatedTokenKind> UpdatedTokenKinds { get; }

    public TokenKindUpdatedEventArgs(IEnumerable<UpdatedTokenKind> updatedTokenKinds)
    {
        UpdatedTokenKinds = updatedTokenKinds;
    }
}

public class UpdatedTokenKind
{
    public string Name { get; }
    public byte Kind { get; }

    public UpdatedTokenKind(string name, byte kind)
    {
        Name = name;
        Kind = kind;
    }

    public override bool Equals(object? obj) => obj is UpdatedTokenKind kind &&
        Name == kind.Name &&
        Kind == kind.Kind;

    public override int GetHashCode() => HashCode.Combine(Name, Kind);
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

public static class TextIteratorBuilder
{
    public static ITextIterator FromString(string text)
    {
        var t = new Text(text);
        return new ForwardTextIterator(t, 0, t.LinesCount - 1);
    }
}
