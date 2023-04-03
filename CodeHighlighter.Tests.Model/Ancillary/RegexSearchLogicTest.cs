using System;
using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Ancillary;
using CodeHighlighter.Core;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Ancillary;

internal class RegexSearchLogicTest
{
    private Mock<IText> _text;
    private string _pattern;
    private SearchOptions _options;
    private List<SearchEntry> _result;
    private RegexSearchLogic _logic;

    [SetUp]
    public void Setup()
    {
        _text = new Mock<IText>();
        _options = new SearchOptions();
        _logic = new RegexSearchLogic();
    }

    [Test]
    public void EmptyTextEmptyPattern()
    {
        _text.Setup(x => x.ToString()).Returns("");
        _pattern = "";

        DoSearch();

        Assert.IsEmpty(_result);
    }

    [Test]
    public void EmptyPattern()
    {
        _text.Setup(x => x.ToString()).Returns("123");
        _pattern = "";

        DoSearch();

        Assert.IsEmpty(_result);
    }

    [Test]
    public void Case()
    {
        _text.Setup(x => x.ToString()).Returns("abcd");
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("abcd"));
        _pattern = "abcd";
        _options.IgnoreCase = false;

        DoSearch();

        Assert.That(_result, Has.Count.EqualTo(1));
        Assert.That(_result.First(), Is.EqualTo(new SearchEntry(new(0, 0), new(0, 4))));
    }

    [Test]
    public void IgnoreCase()
    {
        _text.Setup(x => x.ToString()).Returns("ABCD");
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("ABCD"));
        _pattern = "abcd";
        _options.IgnoreCase = true;

        DoSearch();

        Assert.That(_result, Has.Count.EqualTo(1));
        Assert.That(_result.First(), Is.EqualTo(new SearchEntry(new(0, 0), new(0, 4))));
    }

    [Test]
    public void Begining_1()
    {
        _text.Setup(x => x.ToString()).Returns($"abcd{Environment.NewLine}efgh");
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("abcd"));
        _text.Setup(x => x.GetLine(1)).Returns(new TextLine("efgh"));
        _pattern = "ab";

        DoSearch();

        Assert.That(_result, Has.Count.EqualTo(1));
        Assert.That(_result.First(), Is.EqualTo(new SearchEntry(new(0, 0), new(0, 2))));
    }

    [Test]
    public void Begining_2()
    {
        _text.Setup(x => x.ToString()).Returns($"abcd{Environment.NewLine}efgh");
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("abcd"));
        _text.Setup(x => x.GetLine(1)).Returns(new TextLine("efgh"));
        _pattern = "ef";

        DoSearch();

        Assert.That(_result, Has.Count.EqualTo(1));
        Assert.That(_result.First(), Is.EqualTo(new SearchEntry(new(1, 0), new(1, 2))));
    }

    [Test]
    public void Middle_1()
    {
        _text.Setup(x => x.ToString()).Returns($"abcd{Environment.NewLine}efgh");
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("abcd"));
        _text.Setup(x => x.GetLine(1)).Returns(new TextLine("efgh"));
        _pattern = "bc";

        DoSearch();

        Assert.That(_result, Has.Count.EqualTo(1));
        Assert.That(_result.First(), Is.EqualTo(new SearchEntry(new(0, 1), new(0, 3))));
    }

    [Test]
    public void Middle_2()
    {
        _text.Setup(x => x.ToString()).Returns($"abcd{Environment.NewLine}efgh");
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("abcd"));
        _text.Setup(x => x.GetLine(1)).Returns(new TextLine("efgh"));
        _pattern = "fg";

        DoSearch();

        Assert.That(_result, Has.Count.EqualTo(1));
        Assert.That(_result.First(), Is.EqualTo(new SearchEntry(new(1, 1), new(1, 3))));
    }

    [Test]
    public void Ending_1()
    {
        _text.Setup(x => x.ToString()).Returns($"abcd{Environment.NewLine}efgh");
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("abcd"));
        _text.Setup(x => x.GetLine(1)).Returns(new TextLine("efgh"));
        _pattern = "cd";

        DoSearch();

        Assert.That(_result, Has.Count.EqualTo(1));
        Assert.That(_result.First(), Is.EqualTo(new SearchEntry(new(0, 2), new(0, 4))));
    }

    [Test]
    public void Ending_2()
    {
        _text.Setup(x => x.ToString()).Returns($"abcd{Environment.NewLine}efgh");
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("abcd"));
        _text.Setup(x => x.GetLine(1)).Returns(new TextLine("efgh"));
        _pattern = "gh";

        DoSearch();

        Assert.That(_result, Has.Count.EqualTo(1));
        Assert.That(_result.First(), Is.EqualTo(new SearchEntry(new(1, 2), new(1, 4))));
    }

    [Test]
    public void TwoLinesMatch()
    {
        _text.Setup(x => x.ToString()).Returns($"abcd{Environment.NewLine}efgh");
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("abcd"));
        _text.Setup(x => x.GetLine(1)).Returns(new TextLine("efgh"));
        _pattern = $"cd{Environment.NewLine}ef";

        DoSearch();

        Assert.That(_result, Has.Count.EqualTo(1));
        Assert.That(_result.First(), Is.EqualTo(new SearchEntry(new(0, 2), new(1, 2))));
    }

    [Test]
    public void LastSymbol()
    {
        _text.Setup(x => x.ToString()).Returns($"abcd{Environment.NewLine}efgh");
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("abcd"));
        _text.Setup(x => x.GetLine(1)).Returns(new TextLine("efgh"));
        _pattern = "h";

        DoSearch();

        Assert.That(_result, Has.Count.EqualTo(1));
        Assert.That(_result.First(), Is.EqualTo(new SearchEntry(new(1, 3), new(1, 4))));
    }

    private void DoSearch()
    {
        _result = _logic.DoSearch(_text.Object, _pattern, _options).ToList();
    }
}
