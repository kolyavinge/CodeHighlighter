using System.Collections.Generic;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

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
            new("", 0, 0, 2, 0),
            new("", 0, 2, 1, 1),
            new("", 2, 0, 1, 0),
            new("", 2, 1, 2, 1),
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
            new("", 0, 0, 2, 0),
            new("", 0, 2, 1, 1),
            new("", 2, 0, 1, 0),
            new("", 2, 1, 2, 1),
        };
        _tokens.SetTokens(tokens, 0, 3);

        tokens = new List<CodeHighlighter.CodeProvidering.Token>
        {
            new("", 0, 0, 3, 0),
            new("", 1, 0, 1, 0),
            new("", 1, 1, 2, 1),
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
            new("", 0, 0, 2, 0),
            new("", 1, 2, 1, 1),
            new("", 1, 3, 1, 1),
            new("", 2, 0, 1, 0),
        };
        _tokens.SetTokens(tokens, 0, 3);

        _tokens.ReplaceLines(0, 2);

        Assert.AreEqual(new[] { new Token("", 2, 1, 1), new Token("", 3, 1, 1) }, _tokens.GetTokens(0));
        Assert.AreEqual(new[] { new Token("", 0, 1, 0) }, _tokens.GetTokens(1));
        Assert.AreEqual(new[] { new Token("", 0, 2, 0) }, _tokens.GetTokens(2));
    }
}
