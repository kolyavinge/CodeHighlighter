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
        var result = TokenCursorPosition.GetPosition(_tokens, 0);
        Assert.AreEqual(default(TokenCursorPosition), result);
    }

    [Test]
    public void GetPosition()
    {
        // '  xx  yzz'
        _tokens = new List<LineToken>
        {
            new("xx", 2, 2, 0), // x
            new("y", 6, 1, 1), // y
            new("zz", 7, 2, 2), // z
        };

        var result = TokenCursorPosition.GetPosition(_tokens, 0);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.StartLine, LineToken.Default, new("xx", 2, 2, 0)), result);

        result = TokenCursorPosition.GetPosition(_tokens, 1);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.StartLine, LineToken.Default, new("xx", 2, 2, 0)), result);

        result = TokenCursorPosition.GetPosition(_tokens, 2);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.StartLine, LineToken.Default, new("xx", 2, 2, 0)), result);

        result = TokenCursorPosition.GetPosition(_tokens, 3);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.InToken, new("xx", 2, 2, 0), new("xx", 2, 2, 0)), result);

        result = TokenCursorPosition.GetPosition(_tokens, 4);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.BetweenTokens, new("xx", 2, 2, 0), new("y", 6, 1, 1)), result);

        result = TokenCursorPosition.GetPosition(_tokens, 5);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.BetweenTokens, new("xx", 2, 2, 0), new("y", 6, 1, 1)), result);

        result = TokenCursorPosition.GetPosition(_tokens, 6);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.BetweenTokens, new("xx", 2, 2, 0), new("y", 6, 1, 1)), result);

        result = TokenCursorPosition.GetPosition(_tokens, 7);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.BetweenTokens, new("y", 6, 1, 1), new("zz", 7, 2, 2)), result);

        result = TokenCursorPosition.GetPosition(_tokens, 8);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.InToken, new("zz", 7, 2, 2), new("zz", 7, 2, 2)), result);

        result = TokenCursorPosition.GetPosition(_tokens, 9);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.EndLine, new("zz", 7, 2, 2), LineToken.Default), result);

        result = TokenCursorPosition.GetPosition(_tokens, 10);
        Assert.AreEqual(new TokenCursorPosition(TokenCursorPositionKind.EndLine, new("zz", 7, 2, 2), LineToken.Default), result);
    }
}
