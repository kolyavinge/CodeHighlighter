﻿using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model;

internal interface ITokens
{
    int LinesCount { get; }
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

    public List<LineToken> GetTokens(int lineIndex)
    {
        return _tokens[lineIndex];
    }
}

internal readonly struct LineToken
{
    public readonly int StartColumnIndex;
    public readonly int Length;
    public readonly byte Kind;
    public int EndColumnIndex => StartColumnIndex + Length - 1;

    public LineToken(int startColumnIndex, int length, byte kind)
    {
        StartColumnIndex = startColumnIndex;
        Length = length;
        Kind = kind;
    }

    public static LineToken FromCodeHighlighterToken(Token token)
    {
        return new(token.StartColumnIndex, token.Length, token.Kind);
    }
}
