using System.Linq;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class LineNumberTest
{
    private readonly double TextLineHeight = 10;

    [Test]
    public void GetLineNumbers_LinesInControl()
    {
        var lines = LineNumber.GetLineNumbers(100, 0, TextLineHeight, 3).ToList();

        Assert.That(lines.Count, Is.EqualTo(3));

        Assert.That(lines[0], Is.EqualTo(new LineNumber(0, 0)));
        Assert.That(lines[1], Is.EqualTo(new LineNumber(1, 10)));
        Assert.That(lines[2], Is.EqualTo(new LineNumber(2, 20)));
    }

    [Test]
    public void GetLineNumbers_LinesOutControl()
    {
        var lines = LineNumber.GetLineNumbers(100, 0, TextLineHeight, 100).ToList();

        Assert.That(lines.Count, Is.EqualTo(10));

        Assert.That(lines.First(), Is.EqualTo(new LineNumber(0, 0)));
        Assert.That(lines.Last(), Is.EqualTo(new LineNumber(9, 90)));
    }

    [Test]
    public void GetLineNumbers_LinesInControlWithVerticalScroll_Even()
    {
        var lines = LineNumber.GetLineNumbers(100, 10, TextLineHeight, 10).ToList();

        Assert.That(lines.Count, Is.EqualTo(9));

        Assert.That(lines.First(), Is.EqualTo(new LineNumber(1, 0)));
        Assert.That(lines.Last(), Is.EqualTo(new LineNumber(9, 80)));
    }

    [Test]
    public void GetLineNumbers_LinesInControlWithVerticalScroll_Odd()
    {
        var lines = LineNumber.GetLineNumbers(100, 15, TextLineHeight, 10).ToList();

        Assert.That(lines.Count, Is.EqualTo(9));

        Assert.That(lines.First(), Is.EqualTo(new LineNumber(1, -5)));
        Assert.That(lines.Last(), Is.EqualTo(new LineNumber(9, 75)));
    }

    [Test]
    public void GetLineNumbers_LinesOutControlWithVerticalScroll_Even()
    {
        var lines = LineNumber.GetLineNumbers(100, 10, TextLineHeight, 100).ToList();

        Assert.That(lines.Count, Is.EqualTo(10));

        Assert.That(lines.First(), Is.EqualTo(new LineNumber(1, 0)));
        Assert.That(lines.Last(), Is.EqualTo(new LineNumber(10, 90)));
    }

    [Test]
    public void GetLineNumbers_LinesOutControlWithVerticalScroll_Odd()
    {
        var lines = LineNumber.GetLineNumbers(100, 15, TextLineHeight, 100).ToList();

        Assert.That(lines.Count, Is.EqualTo(11));

        Assert.That(lines.First(), Is.EqualTo(new LineNumber(1, -5)));
        Assert.That(lines.Last(), Is.EqualTo(new LineNumber(11, 95)));
    }

    [Test]
    public void GetLineNumbers_LinesOutControl_OddControlHeight()
    {
        var lines = LineNumber.GetLineNumbers(91, 0, TextLineHeight, 100).ToList();

        Assert.That(lines.Count, Is.EqualTo(10));

        Assert.That(lines.First(), Is.EqualTo(new LineNumber(0, 0)));
        Assert.That(lines.Last(), Is.EqualTo(new LineNumber(9, 90)));
    }
}
