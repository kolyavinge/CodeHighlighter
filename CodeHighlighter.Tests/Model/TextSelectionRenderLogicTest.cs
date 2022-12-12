using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class TextSelectionRenderLogicTest
{
    private TextMeasures _textMeasures;
    private double _horizontalScrollBarValue;
    private double _verticalScrollBarValue;
    private TextSelectionRect _textSelectionRect;

    [SetUp]
    public void Setup()
    {
        _textMeasures = new TextMeasures();
        _textMeasures.UpdateMeasures(5, 2);
        _horizontalScrollBarValue = 7;
        _verticalScrollBarValue = 9;
        _textSelectionRect = new TextSelectionRect();
    }

    [Test]
    public void GetCalculatedRects_Empty()
    {
        var result = _textSelectionRect.GetCalculatedRects(new List<TextSelectionLine>(), _textMeasures, _horizontalScrollBarValue, _verticalScrollBarValue).ToList();
        Assert.AreEqual(0, result.Count);
    }

    [Test]
    public void GetCalculatedRects_OneLine()
    {
        var selectedLines = new List<TextSelectionLine>
        {
            new(1, 1, 5)
        };
        var result = _textSelectionRect.GetCalculatedRects(selectedLines, _textMeasures, _horizontalScrollBarValue, _verticalScrollBarValue).ToList();
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(new Rect(2 - 7, 5 - 9, 2 * 4, 5), result[0]);
    }

    [Test]
    public void GetCalculatedRects_TwoLines()
    {
        var selectedLines = new List<TextSelectionLine>
        {
            new(1, 1, 5),
            new(1, 1, 5),
        };
        var result = _textSelectionRect.GetCalculatedRects(selectedLines, _textMeasures, _horizontalScrollBarValue, _verticalScrollBarValue).ToList();
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(new Rect(2 - 7, 5 - 9, 2 * 5, 5), result[0]);
        Assert.AreEqual(new Rect(2 - 7, 5 - 9, 2 * 4, 5), result[1]);
    }
}
