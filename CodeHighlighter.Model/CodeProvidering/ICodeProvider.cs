using System.Collections.Generic;
using CodeHighlighter.Common;
using CodeHighlighter.Core;

namespace CodeHighlighter.CodeProvidering;

public interface ICodeProvider
{
    IEnumerable<Token> GetTokens(ITextIterator textIterator);
    IEnumerable<TokenColor> GetColors();
}

public record Token(string Name, int LineIndex, int StartColumnIndex, byte Kind)
{
    public int EndColumnIndex => StartColumnIndex + Name.Length - 1;
}

public readonly record struct TokenColor(byte Kind, Color Color);

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

public record UpdatedTokenKind(string Name, byte Kind);

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
