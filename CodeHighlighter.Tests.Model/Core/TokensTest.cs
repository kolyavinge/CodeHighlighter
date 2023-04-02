using System.Collections.Generic;
using CodeHighlighter.Core;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Core;

internal class TokensTest
{
    private Tokens _tokens;

    [SetUp]
    public void Setup()
    {
        _tokens = new Tokens();
    }

    [Test]
    public void Constructor()
    {
        Assert.AreEqual(1, _tokens.LinesCount);
    }

    [Test]
    public void SetTokens_Init()
    {
        var tokens = new List<CodeHighlighter.CodeProvidering.Token>
        {
            new("xx", 0, 0, 0),
            new("x", 0, 2, 1),
            new("x", 2, 0, 0),
            new("xx", 2, 1, 1),
        };
        _tokens.SetTokens(tokens, 0, 3);

        Assert.AreEqual(3, _tokens.LinesCount);
        Assert.AreEqual(2, _tokens.GetTokens(0).Count);
        Assert.AreEqual(0, _tokens.GetTokens(1).Count);
        Assert.AreEqual(2, _tokens.GetTokens(2).Count);
    }

    [Test]
    public void SetTokens_Replace()
    {
        var tokens = new List<CodeHighlighter.CodeProvidering.Token>
        {
            new("xx", 0, 0, 0),
            new("x", 0, 2, 1),
            new("x", 2, 0, 0),
            new("xx", 2, 1, 1),
        };
        _tokens.SetTokens(tokens, 0, 3);

        tokens = new List<CodeHighlighter.CodeProvidering.Token>
        {
            new("xxx", 0, 0, 0),
            new("x", 1, 0, 0),
            new("xx", 1, 1, 1),
        };
        _tokens.SetTokens(tokens, 0, 2);

        Assert.AreEqual(3, _tokens.LinesCount);
        Assert.AreEqual(1, _tokens.GetTokens(0).Count);
        Assert.AreEqual(2, _tokens.GetTokens(1).Count);
        Assert.AreEqual(2, _tokens.GetTokens(2).Count);
    }

    [Test]
    public void DeleteLine()
    {
        Assert.AreEqual(1, _tokens.LinesCount);
        _tokens.DeleteLine(0);
        Assert.AreEqual(1, _tokens.LinesCount);
    }

    [Test]
    public void ReplaceLines()
    {
        var tokens = new List<CodeHighlighter.CodeProvidering.Token>
        {
            new("xx", 0, 0, 0),
            new("x", 1, 2, 1),
            new("x", 1, 3, 1),
            new("x", 2, 0, 0),
        };
        _tokens.SetTokens(tokens, 0, 3);

        _tokens.ReplaceLines(0, 2);

        Assert.AreEqual(new[] { new Token("x", 2, 1), new Token("x", 3, 1) }, _tokens.GetTokens(0));
        Assert.AreEqual(new[] { new Token("x", 0, 0) }, _tokens.GetTokens(1));
        Assert.AreEqual(new[] { new Token("xx", 0, 0) }, _tokens.GetTokens(2));
    }
}
