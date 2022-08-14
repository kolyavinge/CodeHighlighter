using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class TokenCursorPositionLogicTest
{
    private TokenList _tokens;
    private TokenCursorPositionLogic _logic;

    [SetUp]
    public void Setup()
    {
        _tokens = new();
        _logic = new();
    }

    [Test]
    public void GetPosition_EmptyTokens()
    {
        var result = _logic.GetPosition(_tokens, 0);
        Assert.Null(result);
    }

    [Test]
    public void GetPosition()
    {
        // '  xx  yzz'
        _tokens = new TokenList
        {
            new("xx", 2, 2, 0),
            new("y", 6, 1, 1),
            new("zz", 7, 2, 2),
        };

        Assert.AreEqual(null, _logic.GetPosition(_tokens, 0));
        Assert.AreEqual(null, _logic.GetPosition(_tokens, 1));
        Assert.AreEqual(new TokenCursorPosition(new("xx", 2, 2, 0), null), _logic.GetPosition(_tokens, 2));
        Assert.AreEqual(new TokenCursorPosition(new("xx", 2, 2, 0), null), _logic.GetPosition(_tokens, 3));
        Assert.AreEqual(new TokenCursorPosition(new("xx", 2, 2, 0), null), _logic.GetPosition(_tokens, 4));
        Assert.AreEqual(null, _logic.GetPosition(_tokens, 5));
        Assert.AreEqual(new TokenCursorPosition(new("y", 6, 1, 1), null), _logic.GetPosition(_tokens, 6));
        Assert.AreEqual(new TokenCursorPosition(new("y", 6, 1, 1), new("zz", 7, 2, 2)), _logic.GetPosition(_tokens, 7));
        Assert.AreEqual(new TokenCursorPosition(new("zz", 7, 2, 2), null), _logic.GetPosition(_tokens, 8));
        Assert.AreEqual(new TokenCursorPosition(new("zz", 7, 2, 2), null), _logic.GetPosition(_tokens, 9));
        Assert.AreEqual(null, _logic.GetPosition(_tokens, 50));
    }

    [Test]
    public void GetPositionExt_EmptyTokens()
    {
        var result = _logic.GetPositionExt(_tokens, 0);
        Assert.AreEqual(TokenCursorPositionExt.Default, result);
    }

    [Test]
    public void GetPositionExt()
    {
        // '  xx  yzz'
        _tokens = new TokenList
        {
            new("xx", 2, 2, 0),
            new("y", 6, 1, 1),
            new("zz", 7, 2, 2),
        };

        var result = _logic.GetPositionExt(_tokens, 0);
        Assert.AreEqual(new TokenCursorPositionExt(TokenCursorPositionKind.StartLine, Token.Default, new("xx", 2, 2, 0)), result);

        result = _logic.GetPositionExt(_tokens, 1);
        Assert.AreEqual(new TokenCursorPositionExt(TokenCursorPositionKind.StartLine, Token.Default, new("xx", 2, 2, 0)), result);

        result = _logic.GetPositionExt(_tokens, 2);
        Assert.AreEqual(new TokenCursorPositionExt(TokenCursorPositionKind.StartLine, Token.Default, new("xx", 2, 2, 0)), result);

        result = _logic.GetPositionExt(_tokens, 3);
        Assert.AreEqual(new TokenCursorPositionExt(TokenCursorPositionKind.InToken, new("xx", 2, 2, 0), new("xx", 2, 2, 0)), result);

        result = _logic.GetPositionExt(_tokens, 4);
        Assert.AreEqual(new TokenCursorPositionExt(TokenCursorPositionKind.BetweenTokens, new("xx", 2, 2, 0), new("y", 6, 1, 1)), result);

        result = _logic.GetPositionExt(_tokens, 5);
        Assert.AreEqual(new TokenCursorPositionExt(TokenCursorPositionKind.BetweenTokens, new("xx", 2, 2, 0), new("y", 6, 1, 1)), result);

        result = _logic.GetPositionExt(_tokens, 6);
        Assert.AreEqual(new TokenCursorPositionExt(TokenCursorPositionKind.BetweenTokens, new("xx", 2, 2, 0), new("y", 6, 1, 1)), result);

        result = _logic.GetPositionExt(_tokens, 7);
        Assert.AreEqual(new TokenCursorPositionExt(TokenCursorPositionKind.BetweenTokens, new("y", 6, 1, 1), new("zz", 7, 2, 2)), result);

        result = _logic.GetPositionExt(_tokens, 8);
        Assert.AreEqual(new TokenCursorPositionExt(TokenCursorPositionKind.InToken, new("zz", 7, 2, 2), new("zz", 7, 2, 2)), result);

        result = _logic.GetPositionExt(_tokens, 9);
        Assert.AreEqual(new TokenCursorPositionExt(TokenCursorPositionKind.EndLine, new("zz", 7, 2, 2), Token.Default), result);

        result = _logic.GetPositionExt(_tokens, 10);
        Assert.AreEqual(new TokenCursorPositionExt(TokenCursorPositionKind.EndLine, new("zz", 7, 2, 2), Token.Default), result);
    }
}
