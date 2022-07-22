using System;
using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.CodeProvidering;

namespace CodeHighlighter.Model;

internal interface ITokens
{
    int LinesCount { get; }
    IEnumerable<Token> AllTokens { get; }
    List<Token> GetTokens(int lineIndex);
}

public class Tokens : ITokens
{
    private readonly List<List<Token>> _tokens = new();

    public int LinesCount => _tokens.Count;

    public void SetTokens(IEnumerable<ICodeProvider.Token> tokens, int startLineIndex, int linesCount)
    {
        var groupedTokens = new Dictionary<int, List<Token>>();
        foreach (var token in tokens)
        {
            if (groupedTokens.TryGetValue(token.LineIndex, out var value))
            {
                value.Add(Token.FromCodeProviderToken(token));
            }
            else
            {
                groupedTokens.Add(token.LineIndex, new List<Token> { Token.FromCodeProviderToken(token) });
            }
        }
        var length = startLineIndex + linesCount;
        for (int lineIndex = startLineIndex; lineIndex < length; lineIndex++)
        {
            var lineTokens = groupedTokens.ContainsKey(lineIndex) ? groupedTokens[lineIndex] : new List<Token>();
            if (lineIndex < _tokens.Count)
            {
                _tokens[lineIndex] = lineTokens;
            }
            else
            {
                _tokens.Add(lineTokens);
            }
        }
    }

    public void InsertEmptyLine(int lineIndex)
    {
        _tokens.Insert(lineIndex, new List<Token>());
    }

    public void DeleteLine(int lineIndex)
    {
        if (lineIndex == 0 && !_tokens.Any()) return;
        _tokens.RemoveAt(lineIndex);
    }

    public void DeleteLines(int lineIndex, int count)
    {
        _tokens.RemoveRange(lineIndex, count);
    }

    public void ReplaceLines(int sourceLineIndex, int destinationLineIndex)
    {
        var lineTokens = _tokens[sourceLineIndex];
        _tokens.RemoveAt(sourceLineIndex);
        _tokens.Insert(destinationLineIndex, lineTokens);
    }

    public IEnumerable<Token> AllTokens => _tokens.SelectMany(x => x);

    public List<Token> GetTokens(int lineIndex)
    {
        return _tokens[lineIndex];
    }

    public Token? GetTokenOnPosition(int lineIndex, int columnIndex)
    {
        var selector = new TokenSelector();
        return selector.GetTokenOnPosition(this, lineIndex, columnIndex);
    }
}

public class Token
{
    public static readonly Token Default = new("", 0, 0, 0);

    public string Name { get; }
    public int StartColumnIndex { get; }
    public int Length { get; }
    public byte Kind { get; set; }
    public int EndColumnIndex => StartColumnIndex + Length - 1;

    public Token(string name, int startColumnIndex, int length, byte kind)
    {
        Name = name;
        StartColumnIndex = startColumnIndex;
        Length = length;
        Kind = kind;
    }

    public static Token FromCodeProviderToken(ICodeProvider.Token token)
    {
        return new(token.Name, token.StartColumnIndex, token.Length, token.Kind);
    }

    public override bool Equals(object? obj)
    {
        return obj is Token token &&
               Name == token.Name &&
               StartColumnIndex == token.StartColumnIndex &&
               Length == token.Length &&
               Kind == token.Kind;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, StartColumnIndex, Length, Kind);
    }
}
