using System.Linq;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class LineNumberPanelModelTest
{
    private readonly double TextLineHeight = 10;
    private LineNumberPanelModel _model;

    [SetUp]
    public void Setup()
    {
        _model = new LineNumberPanelModel();
    }

    [Test]
    public void GetLineNumbers_LinesOutControl()
    {
        var lines = _model.GetLines(100, 0, TextLineHeight, 100).ToList();

        Assert.That(lines.Count, Is.EqualTo(10));

        Assert.That(lines.First(), Is.EqualTo(new LineNumber(0, 0)));
        Assert.That(lines.Last(), Is.EqualTo(new LineNumber(9, 90)));
    }

    [Test]
    public void GetLineNumbersModified()
    {
        _model.GapCollection[0] = new(2);
        _model.GapCollection[2] = new(3);

        var lines = _model.GetLines(100, 0, TextLineHeight, 3).ToList();

        Assert.That(lines, Has.Count.EqualTo(3));

        Assert.That(lines[0], Is.EqualTo(new LineNumber(0, 2 * TextLineHeight)));
        Assert.That(lines[1], Is.EqualTo(new LineNumber(1, 3 * TextLineHeight)));
        Assert.That(lines[2], Is.EqualTo(new LineNumber(2, 7 * TextLineHeight)));
    }

    [Test]
    public void GetLineNumbersModified_LinesOutControl()
    {
        _model.GapCollection[0] = new(2);
        _model.GapCollection[2] = new(3);

        var lines = _model.GetLines(100, 0, TextLineHeight, 100).ToList();

        Assert.That(lines, Has.Count.EqualTo(6));
    }
}
