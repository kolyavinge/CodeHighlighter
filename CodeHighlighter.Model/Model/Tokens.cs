using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model;

public interface ITokens
{
    IEnumerable<Token> AllTokens { get; }
    int LinesCount { get; }
    void DeleteLine(int lineIndex);
    void DeleteLines(int lineIndex, int count);
    TokenCursorPosition? GetTokenOnPosition(CursorPosition position);
    TokenList GetTokens(int lineIndex);
    void InsertEmptyLine(int lineIndex);
    void InsertEmptyLines(int lineIndex, int count);
    void ReplaceLines(int sourceLineIndex, int destinationLineIndex);
    void SetTokens(IEnumerable<CodeProvidering.Token> tokens, int startLineIndex, int linesCount);
}

public class Tokens : ITokens
{
    private readonly List<TokenList> _tokens = new();

    public int LinesCount => _tokens.Count;

    public IEnumerable<Token> AllTokens => _tokens.SelectMany(x => x);

    public Tokens() { }

    public TokenList GetTokens(int lineIndex)
    {
        return _tokens[lineIndex];
    }

    public TokenCursorPosition? GetTokenOnPosition(CursorPosition position)
    {
        var lineTokens = _tokens[position.LineIndex];
        var logic = new TokenCursorPositionLogic();
        return logic.GetPosition(lineTokens, position.ColumnIndex);
    }

    public void SetTokens(IEnumerable<CodeProvidering.Token> tokens, int startLineIndex, int linesCount)
    {
        var groupedTokens = new Dictionary<int, TokenList>();
        foreach (var token in tokens)
        {
            if (groupedTokens.TryGetValue(token.LineIndex, out var value))
            {
                value.Add(Token.FromCodeProviderToken(token));
            }
            else
            {
                groupedTokens.Add(token.LineIndex, new TokenList { Token.FromCodeProviderToken(token) });
            }
        }
        var length = startLineIndex + linesCount;
        for (int lineIndex = startLineIndex; lineIndex < length; lineIndex++)
        {
            var lineTokens = groupedTokens.ContainsKey(lineIndex) ? groupedTokens[lineIndex] : new TokenList();
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
        _tokens.Insert(lineIndex, new TokenList());
    }

    public void InsertEmptyLines(int lineIndex, int count)
    {
        for (int i = 0; i < count; i++)
        {
            _tokens.Insert(lineIndex, new TokenList());
        }
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

    internal static Token FromCodeProviderToken(CodeProvidering.Token token) => new(token.Name, token.StartColumnIndex, token.Length, token.Kind);

    public override bool Equals(object? obj) => obj is Token token &&
        Name == token.Name &&
        StartColumnIndex == token.StartColumnIndex &&
        Length == token.Length &&
        Kind == token.Kind;

    public override int GetHashCode() => HashCode.Combine(Name, StartColumnIndex, Length, Kind);
}
