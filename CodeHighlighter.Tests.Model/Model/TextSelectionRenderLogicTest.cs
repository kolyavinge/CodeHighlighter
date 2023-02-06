using System;
using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Common;
using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class TextSelectionRenderLogicTest
{
    private TextMeasures _textMeasures;
    private double _horizontalScrollBarValue;
    private double _verticalScrollBarValue;
    private double _controlHeight;
    private Mock<IExtendedLineNumberGenerator> _lineNumberGenerator;
    private TextSelectionRect _textSelectionRect;

    [SetUp]
    public void Setup()
    {
        _textMeasures = new TextMeasures();
        _textMeasures.UpdateMeasures(5, 2);
        _horizontalScrollBarValue = 7;
        _verticalScrollBarValue = 9;
        _controlHeight = 50;
        _lineNumberGenerator = new Mock<IExtendedLineNumberGenerator>();
        _textSelectionRect = new TextSelectionRect(_lineNumberGenerator.Object);
    }

    [Test]
    public void GetCalculatedRects_Empty()
    {
        var result = GetCalculatedRects(new List<TextSelectionLine>());
        Assert.AreEqual(0, result.Count);
        _lineNumberGenerator.Verify(x => x.GetLineNumbers(_controlHeight, _verticalScrollBarValue, 5, 1), Times.Never());
    }

    [Test]
    public void GetCalculatedRects_OneLine()
    {
        var selectedLines = new List<TextSelectionLine>
        {
            new(3, 1, 5)
        };
        _lineNumberGenerator.Setup(x => x.GetLineNumbers(_controlHeight, _verticalScrollBarValue, 5, 4)).Returns(new LineNumber[]
        {
            new(3, 3 * 5 - 9)
        });
        var result = GetCalculatedRects(selectedLines);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(new Rect(2 - 7, 3 * 5 - 9, 2 * 4, 5), result[0]);
        _lineNumberGenerator.Verify(x => x.GetLineNumbers(_controlHeight, _verticalScrollBarValue, 5, 4), Times.Once());
    }

    [Test]
    public void GetCalculatedRects_OneLineOutViewport()
    {
        var selectedLines = new List<TextSelectionLine>
        {
            new(1, 1, 5)
        };
        _lineNumberGenerator.Setup(x => x.GetLineNumbers(_controlHeight, _verticalScrollBarValue, 5, 2)).Returns(Array.Empty<LineNumber>());
        var result = GetCalculatedRects(selectedLines);
        Assert.AreEqual(0, result.Count);
        _lineNumberGenerator.Verify(x => x.GetLineNumbers(_controlHeight, _verticalScrollBarValue, 5, 2), Times.Once());
    }

    [Test]
    public void GetCalculatedRects_TwoLines()
    {
        var selectedLines = new List<TextSelectionLine>
        {
            new(3, 1, 5),
            new(4, 1, 5),
        };
        _lineNumberGenerator.Setup(x => x.GetLineNumbers(_controlHeight, _verticalScrollBarValue, 5, 5)).Returns(new LineNumber[]
        {
            new(3, 3 * 5 - 9),
            new(4, 4 * 5 - 9),
        });
        var result = GetCalculatedRects(selectedLines);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(new Rect(2 - 7, 3 * 5 - 9, 2 * 5, 5), result[0]);
        Assert.AreEqual(new Rect(2 - 7, 4 * 5 - 9, 2 * 4, 5), result[1]);
        _lineNumberGenerator.Verify(x => x.GetLineNumbers(_controlHeight, _verticalScrollBarValue, 5, 5), Times.Once());
    }

    [Test]
    public void GetCalculatedRects_TwoLinesWithGap()
    {
        var selectedLines = new List<TextSelectionLine>
        {
            new(3, 1, 5),
            new(4, 1, 5),
        };
        _lineNumberGenerator.Setup(x => x.GetLineNumbers(_controlHeight, _verticalScrollBarValue, 5, 5)).Returns(new LineNumber[]
        {
            new(3, 3 * 5 - 9),
            new(4, 6 * 5 - 9),
        });
        var result = GetCalculatedRects(selectedLines);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(new Rect(2 - 7, 3 * 5 - 9, 2 * 5, 5), result[0]);
        Assert.AreEqual(new Rect(2 - 7, 6 * 5 - 9, 2 * 4, 5), result[1]);
        _lineNumberGenerator.Verify(x => x.GetLineNumbers(_controlHeight, _verticalScrollBarValue, 5, 5), Times.Once());
    }

    private List<Rect> GetCalculatedRects(List<TextSelectionLine> selectedLines)
    {
        return _textSelectionRect.GetCalculatedRects(selectedLines, _textMeasures, _controlHeight, _horizontalScrollBarValue, _verticalScrollBarValue).ToList();
    }
}
