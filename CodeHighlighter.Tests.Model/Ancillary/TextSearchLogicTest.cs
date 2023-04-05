using System;
using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Ancillary;
using CodeHighlighter.Core;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Ancillary;

internal class TextSearchLogicTest
{
    private Mock<IText> _text;
    private string _pattern;
    private SearchOptions _options;
    private List<TextPosition> _result;
    private TextSearchLogic _logic;

    [SetUp]
    public void Setup()
    {
        _text = new Mock<IText>();
        _options = new SearchOptions();
        _logic = new TextSearchLogic();
    }

    [Test]
    public void EmptyTextEmptyPattern()
    {
        _text.SetupGet(x => x.LinesCount).Returns(0);
        _pattern = "";

        DoSearch();

        Assert.IsEmpty(_result);
    }

    [Test]
    public void PatternMultiline_1_Error()
    {
        _text.SetupGet(x => x.LinesCount).Returns(0);
        _pattern = "123\n123";

        try
        {
            DoSearch();
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.That(e.Message, Is.EqualTo("String pattern cannot be multiline."));
        }
    }

    [Test]
    public void PatternMultiline_2_Error()
    {
        _text.SetupGet(x => x.LinesCount).Returns(0);
        _pattern = "123\r123";

        try
        {
            DoSearch();
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.That(e.Message, Is.EqualTo("String pattern cannot be multiline."));
        }
    }

    [Test]
    public void EmptyPattern()
    {
        _text.SetupGet(x => x.LinesCount).Returns(1);
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("123"));
        _pattern = "";

        DoSearch();

        Assert.IsEmpty(_result);
    }

    [Test]
    public void Case_WholeTextInPattern()
    {
        _text.SetupGet(x => x.LinesCount).Returns(1);
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("abcd"));
        _pattern = "abcd";

        DoSearch();

        Assert.That(_result, Has.Count.EqualTo(1));
        Assert.That(_result.First(), Is.EqualTo(new TextPosition(0, 0, 0, 4)));
    }

    [Test]
    public void Case_WrongFirst()
    {
        _text.SetupGet(x => x.LinesCount).Returns(1);
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("abcd"));
        _pattern = "_bcd";

        DoSearch();

        Assert.IsEmpty(_result);
    }

    [Test]
    public void Case_WrongLast()
    {
        _text.SetupGet(x => x.LinesCount).Returns(1);
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("abcd"));
        _pattern = "abc_";

        DoSearch();

        Assert.IsEmpty(_result);
    }

    [Test]
    public void Case_WrongMiddle_1()
    {
        _text.SetupGet(x => x.LinesCount).Returns(1);
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("abcd"));
        _pattern = "ab_d";

        DoSearch();

        Assert.IsEmpty(_result);
    }

    [Test]
    public void Case_WrongMiddle_2()
    {
        _text.SetupGet(x => x.LinesCount).Returns(1);
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("abcd"));
        _pattern = "a_cd";

        DoSearch();

        Assert.IsEmpty(_result);
    }

    [Test]
    public void Case_PatternLastPosition()
    {
        _text.SetupGet(x => x.LinesCount).Returns(1);
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("__abcd"));
        _pattern = "abcd";

        DoSearch();

        Assert.That(_result, Has.Count.EqualTo(1));
        Assert.That(_result.First(), Is.EqualTo(new TextPosition(0, 2, 0, 6)));
    }

    [Test]
    public void IgnoreCase_WholeTextInPattern()
    {
        _text.SetupGet(x => x.LinesCount).Returns(1);
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("ABCD"));
        _pattern = "abcd";
        _options.IgnoreCase = true;

        DoSearch();

        Assert.That(_result, Has.Count.EqualTo(1));
        Assert.That(_result.First(), Is.EqualTo(new TextPosition(0, 0, 0, 4)));
    }

    [Test]
    public void IgnoreCase_WrongFirst()
    {
        _text.SetupGet(x => x.LinesCount).Returns(1);
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("ABCD"));
        _pattern = "_bcd";
        _options.IgnoreCase = true;

        DoSearch();

        Assert.IsEmpty(_result);
    }

    [Test]
    public void IgnoreCase_WrongLast()
    {
        _text.SetupGet(x => x.LinesCount).Returns(1);
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("ABCD"));
        _pattern = "abc_";
        _options.IgnoreCase = true;

        DoSearch();

        Assert.IsEmpty(_result);
    }

    [Test]
    public void IgnoreCase_WrongMiddle_1()
    {
        _text.SetupGet(x => x.LinesCount).Returns(1);
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("ABCD"));
        _pattern = "ab_d";
        _options.IgnoreCase = true;

        DoSearch();

        Assert.IsEmpty(_result);
    }

    [Test]
    public void IgnoreCase_WrongMiddle_2()
    {
        _text.SetupGet(x => x.LinesCount).Returns(1);
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("ABCD"));
        _pattern = "a_cd";
        _options.IgnoreCase = true;

        DoSearch();

        Assert.IsEmpty(_result);
    }

    [Test]
    public void IgnoreCase_PatternLastPosition()
    {
        _text.SetupGet(x => x.LinesCount).Returns(1);
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("__ABCD"));
        _pattern = "abcd";
        _options.IgnoreCase = true;

        DoSearch();

        Assert.That(_result, Has.Count.EqualTo(1));
        Assert.That(_result.First(), Is.EqualTo(new TextPosition(0, 2, 0, 6)));
    }

    [Test]
    public void MultiLines()
    {
        _text.SetupGet(x => x.LinesCount).Returns(3);
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("__abc"));
        _text.Setup(x => x.GetLine(1)).Returns(new TextLine("abc"));
        _text.Setup(x => x.GetLine(2)).Returns(new TextLine("_abc_"));
        _pattern = "abc";

        DoSearch();

        Assert.That(_result, Has.Count.EqualTo(3));
        Assert.That(_result[0], Is.EqualTo(new TextPosition(0, 2, 0, 5)));
        Assert.That(_result[1], Is.EqualTo(new TextPosition(1, 0, 1, 3)));
        Assert.That(_result[2], Is.EqualTo(new TextPosition(2, 1, 2, 4)));
    }

    [Test]
    public void LongPattern()
    {
        _text.SetupGet(x => x.LinesCount).Returns(1);
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("12345"));
        _pattern = "1234567890";

        DoSearch();

        Assert.IsEmpty(_result);
    }

    private void DoSearch()
    {
        _result = _logic.DoSearch(_text.Object, _pattern, _options).ToList();
    }
}
