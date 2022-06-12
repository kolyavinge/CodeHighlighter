using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model;

internal interface ITokens
{
    int LinesCount { get; }
    IEnumerable<LineToken> AllTokens { get; }
    List<LineToken> GetTokens(int lineIndex);
}

internal class Tokens : ITokens
{
    private readonly List<List<LineToken>> _tokens = new();

    public int LinesCount => _tokens.Count;

    public void SetTokens(IEnumerable<Token> tokens, int startLineIndex, int linesCount)
    {
        var groupedTokens = new Dictionary<int, List<LineToken>>();
        foreach (var token in tokens)
        {
            if (groupedTokens.TryGetValue(token.LineIndex, out var value))
            {
                value.Add(LineToken.FromCodeHighlighterToken(token));
            }
            else
            {
                groupedTokens.Add(token.LineIndex, new List<LineToken> { LineToken.FromCodeHighlighterToken(token) });
            }
        }
        var length = startLineIndex + linesCount;
        for (int lineIndex = startLineIndex; lineIndex < length; lineIndex++)
        {
            var lineTokens = groupedTokens.ContainsKey(lineIndex) ? groupedTokens[lineIndex] : new List<LineToken>();
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
        _tokens.Insert(lineIndex, new List<LineToken>());
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

    public IEnumerable<LineToken> AllTokens => _tokens.SelectMany(x => x);

    public List<LineToken> GetTokens(int lineIndex)
    {
        return _tokens[lineIndex];
    }
}

internal class LineToken
{
    public static readonly LineToken Default = new("", 0, 0, 0);

    public readonly string Name;
    public readonly int StartColumnIndex;
    public readonly int Length;
    public byte Kind;
    public int EndColumnIndex => StartColumnIndex + Length - 1;

    public LineToken(string name, int startColumnIndex, int length, byte kind)
    {
        Name = name;
        StartColumnIndex = startColumnIndex;
        Length = length;
        Kind = kind;
    }

    public static LineToken FromCodeHighlighterToken(Token token)
    {
        return new(token.Name, token.StartColumnIndex, token.Length, token.Kind);
    }

    public override bool Equals(object? obj)
    {
        return obj is LineToken token &&
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
