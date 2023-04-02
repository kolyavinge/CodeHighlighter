﻿using CodeHighlighter.Core;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Core;

internal class TokenNavigatorTest
{
    private Mock<IText> _text;
    private Mock<ITokensInternal> _tokens;
    private TokenNavigator _navigator;

    [SetUp]
    public void Setup()
    {
        _text = new Mock<IText>();
        _tokens = new Mock<ITokensInternal>();
        _navigator = new TokenNavigator();
    }

    [Test]
    public void MoveRight_FromStartLine()
    {
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("  xx  yzz  "));
        _text.SetupGet(x => x.LinesCount).Returns(1);
        var tokens = new TokenList
        {
            new("xx", 2, 0),
            new("y", 6, 1),
            new("zz", 7, 2)
        };
        _tokens.SetupGet(x => x.LinesCount).Returns(1);
        _tokens.Setup(x => x.GetTokens(0)).Returns(tokens);

        var result = _navigator.MoveRight(_text.Object, _tokens.Object, 0, 0);
        Assert.AreEqual(0, result.LineIndex);
        Assert.AreEqual(2, result.ColumnIndex);

        result = _navigator.MoveRight(_text.Object, _tokens.Object, 0, 2);
        Assert.AreEqual(0, result.LineIndex);
        Assert.AreEqual(6, result.ColumnIndex);

        result = _navigator.MoveRight(_text.Object, _tokens.Object, 0, 6);
        Assert.AreEqual(0, result.LineIndex);
        Assert.AreEqual(7, result.ColumnIndex);

        result = _navigator.MoveRight(_text.Object, _tokens.Object, 0, 7);
        Assert.AreEqual(0, result.LineIndex);
        Assert.AreEqual(11, result.ColumnIndex);

        result = _navigator.MoveRight(_text.Object, _tokens.Object, 0, 11);
        Assert.AreEqual(0, result.LineIndex);
        Assert.AreEqual(11, result.ColumnIndex);
    }

    [Test]
    public void MoveRight_BetweenTokens()
    {
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("  xx  yzz  "));
        _text.SetupGet(x => x.LinesCount).Returns(1);
        var tokens = new TokenList
        {
            new("xx", 2, 0),
            new("y", 6, 1),
            new("zz", 7, 2)
        };
        _tokens.SetupGet(x => x.LinesCount).Returns(1);
        _tokens.Setup(x => x.GetTokens(0)).Returns(tokens);

        var result = _navigator.MoveRight(_text.Object, _tokens.Object, 0, 5);
        Assert.AreEqual(0, result.LineIndex);
        Assert.AreEqual(6, result.ColumnIndex);
    }

    [Test]
    public void MoveRight_InToken()
    {
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("  xx  yzz  "));
        _text.SetupGet(x => x.LinesCount).Returns(1);
        var tokens = new TokenList
        {
            new("xx", 2, 0),
            new("y", 6, 1),
            new("zz", 7, 2)
        };
        _tokens.SetupGet(x => x.LinesCount).Returns(1);
        _tokens.Setup(x => x.GetTokens(0)).Returns(tokens);

        var result = _navigator.MoveRight(_text.Object, _tokens.Object, 0, 3);
        Assert.AreEqual(0, result.LineIndex);
        Assert.AreEqual(6, result.ColumnIndex);
    }

    [Test]
    public void MoveRight_EndLine()
    {
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("  xx  yzz  "));
        _text.SetupGet(x => x.LinesCount).Returns(1);
        var tokens = new TokenList
        {
            new("xx", 2, 0),
            new("y", 6, 1),
            new("zz", 7, 2)
        };
        _tokens.SetupGet(x => x.LinesCount).Returns(1);
        _tokens.Setup(x => x.GetTokens(0)).Returns(tokens);

        var result = _navigator.MoveRight(_text.Object, _tokens.Object, 0, 10);
        Assert.AreEqual(0, result.LineIndex);
        Assert.AreEqual(11, result.ColumnIndex);
    }

    [Test]
    public void MoveRight_NextLine()
    {
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("  xx  yzz  "));
        _text.Setup(x => x.GetLine(1)).Returns(new TextLine("  xx  yzz  "));
        _text.SetupGet(x => x.LinesCount).Returns(2);
        var tokens1 = new TokenList
        {
            new("xx", 2, 0),
            new("y", 6, 1),
            new("zz", 7, 2)
        };
        var tokens2 = new TokenList
        {
            new("xx", 2, 0),
            new("y", 6, 1),
            new("zz", 7, 2)
        };
        _tokens.Setup(x => x.GetTokens(0)).Returns(tokens1);
        _tokens.Setup(x => x.GetTokens(0)).Returns(tokens2);
        _tokens.SetupGet(x => x.LinesCount).Returns(2);

        var result = _navigator.MoveRight(_text.Object, _tokens.Object, 0, 11);
        Assert.AreEqual(1, result.LineIndex);
        Assert.AreEqual(0, result.ColumnIndex);
    }

    [Test]
    public void MoveLeft_FromEndLine()
    {
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("  xx  yzz  "));
        _text.SetupGet(x => x.LinesCount).Returns(1);
        var tokens = new TokenList
        {
            new("xx", 2, 0),
            new("y", 6, 1),
            new("zz", 7, 2)
        };
        _tokens.SetupGet(x => x.LinesCount).Returns(1);
        _tokens.Setup(x => x.GetTokens(0)).Returns(tokens);

        var result = _navigator.MoveLeft(_text.Object, _tokens.Object, 0, 11);
        Assert.AreEqual(0, result.LineIndex);
        Assert.AreEqual(7, result.ColumnIndex);

        result = _navigator.MoveLeft(_text.Object, _tokens.Object, 0, 7);
        Assert.AreEqual(0, result.LineIndex);
        Assert.AreEqual(6, result.ColumnIndex);

        result = _navigator.MoveLeft(_text.Object, _tokens.Object, 0, 6);
        Assert.AreEqual(0, result.LineIndex);
        Assert.AreEqual(2, result.ColumnIndex);

        result = _navigator.MoveLeft(_text.Object, _tokens.Object, 0, 2);
        Assert.AreEqual(0, result.LineIndex);
        Assert.AreEqual(0, result.ColumnIndex);

        result = _navigator.MoveLeft(_text.Object, _tokens.Object, 0, 0);
        Assert.AreEqual(0, result.LineIndex);
        Assert.AreEqual(0, result.ColumnIndex);
    }

    [Test]
    public void MoveLeft_InToken()
    {
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("  xx  yzz  "));
        _text.SetupGet(x => x.LinesCount).Returns(1);
        var tokens = new TokenList
        {
            new("xx", 2, 0),
            new("y", 6, 1),
            new("zz", 7, 2)
        };
        _tokens.SetupGet(x => x.LinesCount).Returns(1);
        _tokens.Setup(x => x.GetTokens(0)).Returns(tokens);

        var result = _navigator.MoveLeft(_text.Object, _tokens.Object, 0, 8);
        Assert.AreEqual(0, result.LineIndex);
        Assert.AreEqual(7, result.ColumnIndex);
    }

    [Test]
    public void MoveLeft_PrevLine()
    {
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("  xx  yzz  "));
        _text.Setup(x => x.GetLine(1)).Returns(new TextLine("  xx  yzz  "));
        _text.SetupGet(x => x.LinesCount).Returns(2);
        var tokens1 = new TokenList
        {
            new("xx", 2, 0),
            new("y", 6, 1),
            new("zz", 7, 2)
        };
        var tokens2 = new TokenList
        {
            new("xx", 2, 0),
            new("y", 6, 1),
            new("zz", 7, 2)
        };
        _tokens.Setup(x => x.GetTokens(0)).Returns(tokens1);
        _tokens.Setup(x => x.GetTokens(0)).Returns(tokens2);
        _tokens.SetupGet(x => x.LinesCount).Returns(2);

        var result = _navigator.MoveLeft(_text.Object, _tokens.Object, 1, 0);
        Assert.AreEqual(0, result.LineIndex);
        Assert.AreEqual(11, result.ColumnIndex);
    }
}
