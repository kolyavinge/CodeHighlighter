using System.Collections.Generic;
using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class TokenNavigatorTest
{
    private Mock<IText> _text;
    private Mock<ITokens> _tokens;
    private TokenNavigator _navigator;

    [SetUp]
    public void Setup()
    {
        _text = new Mock<IText>();
        _tokens = new Mock<ITokens>();
        _navigator = new TokenNavigator();
    }

    [Test]
    public void MoveRight()
    {
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("  xx  yzz  "));
        _text.SetupGet(x => x.LinesCount).Returns(1);
        var tokens = new List<LineToken>
        {
            new(2, 2, 0), // x
            new(6, 1, 1), // y
            new(7, 2, 2), // z
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
    public void MoveRight_NextLine()
    {
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("  xx  yzz  "));
        _text.Setup(x => x.GetLine(1)).Returns(new TextLine("  xx  yzz  "));
        _text.SetupGet(x => x.LinesCount).Returns(2);
        var tokens1 = new List<LineToken>
        {
            new(2, 2, 0), // x
            new(6, 1, 1), // y
            new(7, 2, 2), // z
        };
        var tokens2 = new List<LineToken>
        {
            new(2, 2, 0), // x
            new(6, 1, 1), // y
            new(7, 2, 2), // z
        };
        _tokens.Setup(x => x.GetTokens(0)).Returns(tokens1);
        _tokens.Setup(x => x.GetTokens(0)).Returns(tokens2);
        _tokens.SetupGet(x => x.LinesCount).Returns(2);

        var result = _navigator.MoveRight(_text.Object, _tokens.Object, 0, 11);
        Assert.AreEqual(1, result.LineIndex);
        Assert.AreEqual(0, result.ColumnIndex);
    }

    [Test]
    public void MoveLeft()
    {
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("  xx  yzz  "));
        _text.SetupGet(x => x.LinesCount).Returns(1);
        var tokens = new List<LineToken>
        {
            new(2, 2, 0), // x
            new(6, 1, 1), // y
            new(7, 2, 2), // z
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
    public void MoveLeft_PrevLine()
    {
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("  xx  yzz  "));
        _text.Setup(x => x.GetLine(1)).Returns(new TextLine("  xx  yzz  "));
        _text.SetupGet(x => x.LinesCount).Returns(2);
        var tokens1 = new List<LineToken>
        {
            new(2, 2, 0), // x
            new(6, 1, 1), // y
            new(7, 2, 2), // z
        };
        var tokens2 = new List<LineToken>
        {
            new(2, 2, 0), // x
            new(6, 1, 1), // y
            new(7, 2, 2), // z
        };
        _tokens.Setup(x => x.GetTokens(0)).Returns(tokens1);
        _tokens.Setup(x => x.GetTokens(0)).Returns(tokens2);
        _tokens.SetupGet(x => x.LinesCount).Returns(2);

        var result = _navigator.MoveLeft(_text.Object, _tokens.Object, 1, 0);
        Assert.AreEqual(0, result.LineIndex);
        Assert.AreEqual(11, result.ColumnIndex);
    }
}
