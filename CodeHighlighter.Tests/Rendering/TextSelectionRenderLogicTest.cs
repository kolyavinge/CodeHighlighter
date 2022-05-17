using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CodeHighlighter.Model;
using CodeHighlighter.Rendering;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Rendering;

public class TextSelectionRenderLogicTest
{
    private TextMeasures _textMeasures;
    private Mock<IViewportContext> _viewportContext;
    private TextSelectionRenderLogic _logic;

    [SetUp]
    public void Setup()
    {
        _textMeasures = new TextMeasures(5, 2);

        _viewportContext = new Mock<IViewportContext>();
        _viewportContext.SetupGet(x => x.HorizontalScrollBarValue).Returns(7);
        _viewportContext.SetupGet(x => x.VerticalScrollBarValue).Returns(9);

        _logic = new TextSelectionRenderLogic();
    }

    [Test]
    public void GetCalculatedRects_Empty()
    {
        var selectedLines = new List<TextSelectionLine>();
        var result = _logic.GetCalculatedRects(selectedLines, _textMeasures, _viewportContext.Object).ToList();
        Assert.AreEqual(0, result.Count);
    }

    [Test]
    public void GetCalculatedRects_OneLine()
    {
        var selectedLines = new List<TextSelectionLine>
        {
            new TextSelectionLine(1, 1, 5)
        };
        var result = _logic.GetCalculatedRects(selectedLines, _textMeasures, _viewportContext.Object).ToList();
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(new Rect(2 - 7, 5 - 9, 2 * 4, 5), result[0]);
    }

    [Test]
    public void GetCalculatedRects_TwoLines()
    {
        var selectedLines = new List<TextSelectionLine>
        {
            new TextSelectionLine(1, 1, 5),
            new TextSelectionLine(1, 1, 5),
        };
        var result = _logic.GetCalculatedRects(selectedLines, _textMeasures, _viewportContext.Object).ToList();
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(new Rect(2 - 7, 5 - 9, 2 * 5, 5), result[0]);
        Assert.AreEqual(new Rect(2 - 7, 5 - 9, 2 * 4, 5), result[1]);
    }
}
