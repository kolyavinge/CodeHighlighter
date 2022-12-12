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

        Assert.That(lines.First(), Is.EqualTo(new LineNumber(0, 0 * TextLineHeight)));
        Assert.That(lines.Last(), Is.EqualTo(new LineNumber(9, 9 * TextLineHeight)));
    }

    [Test]
    public void GetLineNumbersModified()
    {
        _model.Gaps[0] = new(2);
        _model.Gaps[2] = new(3);

        var lines = _model.GetLines(100, 0, TextLineHeight, 3).ToList();

        Assert.That(lines, Has.Count.EqualTo(3));

        Assert.That(lines[0], Is.EqualTo(new LineNumber(0, 2 * TextLineHeight)));
        Assert.That(lines[1], Is.EqualTo(new LineNumber(1, 3 * TextLineHeight)));
        Assert.That(lines[2], Is.EqualTo(new LineNumber(2, 7 * TextLineHeight)));
    }

    [Test]
    public void GetLineNumbersModified_LinesOutControl()
    {
        _model.Gaps[0] = new(2);
        _model.Gaps[2] = new(3);

        var lines = _model.GetLines(100, 0, TextLineHeight, 100).ToList();

        Assert.That(lines, Has.Count.EqualTo(5));

        Assert.That(lines[0], Is.EqualTo(new LineNumber(0, 2 * TextLineHeight)));
        Assert.That(lines[1], Is.EqualTo(new LineNumber(1, 3 * TextLineHeight)));
        Assert.That(lines[2], Is.EqualTo(new LineNumber(2, 7 * TextLineHeight)));
        Assert.That(lines[3], Is.EqualTo(new LineNumber(3, 8 * TextLineHeight)));
        Assert.That(lines[4], Is.EqualTo(new LineNumber(4, 9 * TextLineHeight)));
    }

    [Test]
    public void GetLineNumbersModified_VerticalScrollEven()
    {
        _model.Gaps[0] = new(2);
        _model.Gaps[2] = new(3);

        var lines = _model.GetLines(100, 30, TextLineHeight, 100).ToList();

        Assert.That(lines, Has.Count.EqualTo(7));

        Assert.That(lines[0], Is.EqualTo(new LineNumber(1, 0)));
        Assert.That(lines[1], Is.EqualTo(new LineNumber(2, 4 * TextLineHeight)));
        Assert.That(lines[2], Is.EqualTo(new LineNumber(3, 5 * TextLineHeight)));
        Assert.That(lines[3], Is.EqualTo(new LineNumber(4, 6 * TextLineHeight)));
        Assert.That(lines[4], Is.EqualTo(new LineNumber(5, 7 * TextLineHeight)));
        Assert.That(lines[5], Is.EqualTo(new LineNumber(6, 8 * TextLineHeight)));
        Assert.That(lines[6], Is.EqualTo(new LineNumber(7, 9 * TextLineHeight)));
    }

    [Test]
    public void GetLineNumbersModified_VerticalScrollEven_2()
    {
        _model.Gaps[0] = new(2);
        _model.Gaps[2] = new(3);

        var lines = _model.GetLines(100, 50, TextLineHeight, 100).ToList();

        Assert.That(lines, Has.Count.EqualTo(8));

        Assert.That(lines[0], Is.EqualTo(new LineNumber(2, 2 * TextLineHeight)));
        Assert.That(lines[1], Is.EqualTo(new LineNumber(3, 3 * TextLineHeight)));
        Assert.That(lines[2], Is.EqualTo(new LineNumber(4, 4 * TextLineHeight)));
        Assert.That(lines[3], Is.EqualTo(new LineNumber(5, 5 * TextLineHeight)));
        Assert.That(lines[4], Is.EqualTo(new LineNumber(6, 6 * TextLineHeight)));
        Assert.That(lines[5], Is.EqualTo(new LineNumber(7, 7 * TextLineHeight)));
        Assert.That(lines[6], Is.EqualTo(new LineNumber(8, 8 * TextLineHeight)));
        Assert.That(lines[7], Is.EqualTo(new LineNumber(9, 9 * TextLineHeight)));
    }

    [Test]
    public void GetLineNumbersModified_VerticalScrollOdd()
    {
        _model.Gaps[0] = new(2);
        _model.Gaps[2] = new(3);

        var lines = _model.GetLines(100, 45, TextLineHeight, 100).ToList();

        Assert.That(lines, Has.Count.EqualTo(8));

        Assert.That(lines[0], Is.EqualTo(new LineNumber(2, 2 * TextLineHeight + 5)));
        Assert.That(lines[1], Is.EqualTo(new LineNumber(3, 3 * TextLineHeight + 5)));
        Assert.That(lines[2], Is.EqualTo(new LineNumber(4, 4 * TextLineHeight + 5)));
        Assert.That(lines[3], Is.EqualTo(new LineNumber(5, 5 * TextLineHeight + 5)));
        Assert.That(lines[4], Is.EqualTo(new LineNumber(6, 6 * TextLineHeight + 5)));
        Assert.That(lines[5], Is.EqualTo(new LineNumber(7, 7 * TextLineHeight + 5)));
        Assert.That(lines[6], Is.EqualTo(new LineNumber(8, 8 * TextLineHeight + 5)));
        Assert.That(lines[7], Is.EqualTo(new LineNumber(9, 9 * TextLineHeight + 5)));
    }

    [Test]
    public void GetLineNumbersModified_VerticalScrollOdd_2()
    {
        _model.Gaps[0] = new(2);
        _model.Gaps[2] = new(3);

        var lines = _model.GetLines(100, 25, TextLineHeight, 100).ToList();

        Assert.That(lines, Has.Count.EqualTo(8));

        Assert.That(lines[0], Is.EqualTo(new LineNumber(0, -5)));
        Assert.That(lines[1], Is.EqualTo(new LineNumber(1, 5)));
        Assert.That(lines[2], Is.EqualTo(new LineNumber(2, 4 * TextLineHeight + 5)));
        Assert.That(lines[3], Is.EqualTo(new LineNumber(3, 5 * TextLineHeight + 5)));
        Assert.That(lines[4], Is.EqualTo(new LineNumber(4, 6 * TextLineHeight + 5)));
        Assert.That(lines[5], Is.EqualTo(new LineNumber(5, 7 * TextLineHeight + 5)));
        Assert.That(lines[6], Is.EqualTo(new LineNumber(6, 8 * TextLineHeight + 5)));
        Assert.That(lines[7], Is.EqualTo(new LineNumber(7, 9 * TextLineHeight + 5)));
    }
}
