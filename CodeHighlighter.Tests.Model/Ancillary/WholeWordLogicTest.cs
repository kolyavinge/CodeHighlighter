using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Ancillary;
using CodeHighlighter.Core;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Ancillary;

internal class WholeWordLogicTest
{
    private Mock<ITextLines> _textLines;
    private List<TextPosition> _positions;
    private List<TextPosition> _result;
    private WholeWordLogic _logic;

    [SetUp]
    public void Setup()
    {
        _textLines = new Mock<ITextLines>();
        _positions = new List<TextPosition>();
        _logic = new WholeWordLogic(_textLines.Object);
    }

    [Test]
    public void Empty()
    {
        GetResult();

        Assert.That(_result, Has.Count.EqualTo(0));
    }

    [Test]
    public void WholeLine()
    {
        _textLines.Setup(x => x.GetLine(0)).Returns(new TextLine("123"));
        _positions.Add(new TextPosition(0, 0, 0, 3));

        GetResult();

        Assert.That(_result[0], Is.EqualTo(new TextPosition(0, 0, 0, 3)));
    }

    [Test]
    public void TerminalSymbols()
    {
        _textLines.Setup(x => x.GetLine(0)).Returns(new TextLine("+123+"));
        _positions.Add(new TextPosition(0, 1, 0, 4));

        GetResult();

        Assert.That(_result[0], Is.EqualTo(new TextPosition(0, 1, 0, 4)));
    }

    [Test]
    public void StartTerminalSymbol()
    {
        _textLines.Setup(x => x.GetLine(0)).Returns(new TextLine("+123"));
        _positions.Add(new TextPosition(0, 1, 0, 4));

        GetResult();

        Assert.That(_result[0], Is.EqualTo(new TextPosition(0, 1, 0, 4)));
    }

    [Test]
    public void EndTerminalSymbol()
    {
        _textLines.Setup(x => x.GetLine(0)).Returns(new TextLine("123+"));
        _positions.Add(new TextPosition(0, 0, 0, 3));

        GetResult();

        Assert.That(_result[0], Is.EqualTo(new TextPosition(0, 0, 0, 3)));
    }

    [Test]
    public void StartNonTerminalSymbol()
    {
        _textLines.Setup(x => x.GetLine(0)).Returns(new TextLine("_123"));
        _positions.Add(new TextPosition(0, 1, 0, 4));

        GetResult();

        Assert.That(_result, Has.Count.EqualTo(0));
    }

    [Test]
    public void EndNonTerminalSymbol()
    {
        _textLines.Setup(x => x.GetLine(0)).Returns(new TextLine("123_"));
        _positions.Add(new TextPosition(0, 0, 0, 3));

        GetResult();

        Assert.That(_result, Has.Count.EqualTo(0));
    }

    [Test]
    public void IsTerminal()
    {
        var terminals = "!@#$%^&*()+-=`~,.<>[]{};:'\"\\|/?";
        Assert.True(terminals.All(_logic.IsTerminal));
    }

    [Test]
    public void NonTerminals()
    {
        var terminals = "_1234567890qwertyuiopasdfghjklzxcvbnm";
        Assert.False(terminals.All(_logic.IsTerminal));
    }

    private void GetResult()
    {
        _result = _logic.GetResult(_positions).ToList();
    }
}
