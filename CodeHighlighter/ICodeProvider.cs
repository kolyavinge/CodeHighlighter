using System;
using System.Collections.Generic;

namespace CodeHighlighter;

public interface ICodeProvider
{
    IEnumerable<Token> GetTokens(ITextIterator textIterator);
    IEnumerable<TokenColor> GetColors();
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

    public override bool Equals(object? obj)
    {
        return obj is UpdatedTokenKind kind &&
               Name == kind.Name &&
               Kind == kind.Kind;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Kind);
    }
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
