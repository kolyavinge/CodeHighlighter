using System.Collections.Generic;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

public class TokenCursorPositionTest
{
    private List<LineToken> _tokens;

    [SetUp]
    public void Setup()
    {
        _tokens = new();
    }

    [Test]
    public void GetPosition_EmptyTokens()
    {
        var result = TokenCursorPosition.GetPosition(_tokens, 0, 0);
        Assert.AreEqual(default(TokenCursorPosition), result);
    }

    [Test]
    public void GetPosition()
    {
        // '  xx  yzz'
        _tokens = new List<LineToken>
        {
            new(2, 2, 0), // x
            new(6, 1, 1), // y
            new(7, 2, 2), // z
        };

        var result = TokenCursorPosition.GetPosition(_tokens, 0, 0);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.StartLine, default, new(2, 2, 0)), result);

        result = TokenCursorPosition.GetPosition(_tokens, 0, 1);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.StartLine, default, new(2, 2, 0)), result);

        result = TokenCursorPosition.GetPosition(_tokens, 0, 2);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.StartLine, default, new(2, 2, 0)), result);

        result = TokenCursorPosition.GetPosition(_tokens, 0, 3);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.InToken, new(2, 2, 0), new(2, 2, 0)), result);

        result = TokenCursorPosition.GetPosition(_tokens, 0, 4);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.BetweenTokens, new(2, 2, 0), new(6, 1, 1)), result);

        result = TokenCursorPosition.GetPosition(_tokens, 0, 5);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.BetweenTokens, new(2, 2, 0), new(6, 1, 1)), result);

        result = TokenCursorPosition.GetPosition(_tokens, 0, 6);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.BetweenTokens, new(2, 2, 0), new(6, 1, 1)), result);

        result = TokenCursorPosition.GetPosition(_tokens, 0, 7);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.BetweenTokens, new(6, 1, 1), new(7, 2, 2)), result);

        result = TokenCursorPosition.GetPosition(_tokens, 0, 8);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.InToken, new(7, 2, 2), new(7, 2, 2)), result);

        result = TokenCursorPosition.GetPosition(_tokens, 0, 9);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.EndLine, new(7, 2, 2), default), result);

        result = TokenCursorPosition.GetPosition(_tokens, 0, 10);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.EndLine, new(7, 2, 2), default), result);
    }
}
