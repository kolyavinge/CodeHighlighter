﻿using System.Linq;
using CodeHighlighter.Ancillary;
using CodeHighlighter.Core;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Core;

internal class ExtendedLineNumberGeneratorTest
{
    private readonly double TextLineHeight = 10;
    private Mock<ILineGapCollection> _gaps;
    private Mock<ILineFolds> _folds;
    private ExtendedLineNumberGenerator _generator;

    [SetUp]
    public void Setup()
    {
        _gaps = new Mock<ILineGapCollection>();
        _folds = new Mock<ILineFolds>();
        _generator = new ExtendedLineNumberGenerator(new LineNumberGenerator(), _gaps.Object, _folds.Object);
    }

    [Test]
    public void GetLineNumbers_LinesOutControl()
    {
        var lines = _generator.GetLineNumbers(100, 0, TextLineHeight, 100).ToList();

        Assert.That(lines.Count, Is.EqualTo(10));

        Assert.That(lines.First(), Is.EqualTo(new LineNumber(0, 0 * TextLineHeight)));
        Assert.That(lines.Last(), Is.EqualTo(new LineNumber(9, 9 * TextLineHeight)));
    }

    [Test]
    public void GetLineNumbersModified()
    {
        _gaps.SetupGet(x => x.AnyItems).Returns(true);
        _gaps.SetupGet(x => x[0]).Returns(new LineGap(2));
        _gaps.SetupGet(x => x[2]).Returns(new LineGap(3));

        var lines = _generator.GetLineNumbers(100, 0, TextLineHeight, 3).ToList();

        Assert.That(lines, Has.Count.EqualTo(3));

        Assert.That(lines[0], Is.EqualTo(new LineNumber(0, 2 * TextLineHeight)));
        Assert.That(lines[1], Is.EqualTo(new LineNumber(1, 3 * TextLineHeight)));
        Assert.That(lines[2], Is.EqualTo(new LineNumber(2, 7 * TextLineHeight)));
    }

    [Test]
    public void GetLineNumbersModified_LinesOutControl()
    {
        _gaps.SetupGet(x => x.AnyItems).Returns(true);
        _gaps.SetupGet(x => x[0]).Returns(new LineGap(2));
        _gaps.SetupGet(x => x[2]).Returns(new LineGap(3));

        var lines = _generator.GetLineNumbers(100, 0, TextLineHeight, 100).ToList();

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
        _gaps.SetupGet(x => x.AnyItems).Returns(true);
        _gaps.SetupGet(x => x[0]).Returns(new LineGap(2));
        _gaps.SetupGet(x => x[2]).Returns(new LineGap(3));

        var lines = _generator.GetLineNumbers(100, 30, TextLineHeight, 100).ToList();

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
        _gaps.SetupGet(x => x.AnyItems).Returns(true);
        _gaps.SetupGet(x => x[0]).Returns(new LineGap(2));
        _gaps.SetupGet(x => x[2]).Returns(new LineGap(3));

        var lines = _generator.GetLineNumbers(100, 50, TextLineHeight, 100).ToList();

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
        _gaps.SetupGet(x => x.AnyItems).Returns(true);
        _gaps.SetupGet(x => x[0]).Returns(new LineGap(2));
        _gaps.SetupGet(x => x[2]).Returns(new LineGap(3));

        var lines = _generator.GetLineNumbers(100, 45, TextLineHeight, 100).ToList();

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
        _gaps.SetupGet(x => x.AnyItems).Returns(true);
        _gaps.SetupGet(x => x[0]).Returns(new LineGap(2));
        _gaps.SetupGet(x => x[2]).Returns(new LineGap(3));

        var lines = _generator.GetLineNumbers(100, 25, TextLineHeight, 100).ToList();

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

    [Test]
    public void GetLineOffsetY_Gaps()
    {
        _gaps.SetupGet(x => x.AnyItems).Returns(true);
        _gaps.SetupGet(x => x[0]).Returns(new LineGap(2));
        _gaps.SetupGet(x => x[2]).Returns(new LineGap(3));

        var result = _generator.GetLineOffsetY(7, TextLineHeight);

        Assert.That(result, Is.EqualTo((7 + 5) * TextLineHeight));
    }

    [Test]
    public void GetLineOffsetY_Folds()
    {
        _folds.SetupGet(x => x.AnyFoldedItems).Returns(true);
        _folds.Setup(x => x.IsFolded(1)).Returns(true);
        _folds.Setup(x => x.IsFolded(2)).Returns(true);
        _folds.Setup(x => x.IsFolded(3)).Returns(true);

        var result = _generator.GetLineOffsetY(7, TextLineHeight);

        Assert.That(result, Is.EqualTo((7 - 3) * TextLineHeight));
    }

    [Test]
    public void GetLineIndex()
    {
        Assert.That(_generator.GetLineIndex(20, 100, 10, 10, 1000), Is.EqualTo(3));
    }

    [Test]
    public void GetLineIndexWithGaps_CursorBelowGap()
    {
        _gaps.SetupGet(x => x.AnyItems).Returns(true);
        _gaps.SetupGet(x => x[0]).Returns(new LineGap(2));

        Assert.That(_generator.GetLineIndex(50, 100, 10, 10, 1000), Is.EqualTo(3));
    }

    [Test]
    public void GetLineIndexWithGaps_CursorInGapFirstLine()
    {
        _gaps.SetupGet(x => x.AnyItems).Returns(true);
        _gaps.SetupGet(x => x[0]).Returns(new LineGap(2));

        Assert.That(_generator.GetLineIndex(10, 100, 0, 10, 1000), Is.EqualTo(0));
    }

    [Test]
    public void GetLineIndexWithGaps_CursorInGap()
    {
        _gaps.SetupGet(x => x.AnyItems).Returns(true);
        _gaps.SetupGet(x => x[1]).Returns(new LineGap(2));

        Assert.That(_generator.GetLineIndex(25, 100, 0, 10, 1000), Is.EqualTo(0));
    }
}
