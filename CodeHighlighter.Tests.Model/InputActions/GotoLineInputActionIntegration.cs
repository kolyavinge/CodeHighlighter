using System;
using CodeHighlighter.InputActions;
using NUnit.Framework;

namespace CodeHighlighter.Tests.InputActions;

internal class GotoLineInputActionIntegration : BaseInputActionIntegration
{
    [Test]
    public void GotoFirstLine()
    {
        var lineIndex = GotoLineInputAction.Instance.CalculateLineIndex(0, 100);
        Assert.AreEqual(0, lineIndex);

        var offsetLine = GotoLineInputAction.Instance.CalculateOffsetLine(lineIndex, 20);
        Assert.AreEqual(0, offsetLine);

        var scrollBarValue = GotoLineInputAction.Instance.CalculateVerticalScrollBarValue(offsetLine, 3);
        Assert.AreEqual(0, scrollBarValue);
    }

    [Test]
    public void GotoLastLine()
    {
        var lineIndex = GotoLineInputAction.Instance.CalculateLineIndex(99, 100);
        Assert.AreEqual(99, lineIndex);

        var offsetLine = GotoLineInputAction.Instance.CalculateOffsetLine(lineIndex, 20);
        Assert.AreEqual(89, offsetLine);

        var scrollBarValue = GotoLineInputAction.Instance.CalculateVerticalScrollBarValue(offsetLine, 3);
        Assert.AreEqual(89 * 3, scrollBarValue);
    }

    [Test]
    public void GotoFarAway()
    {
        var lineIndex = GotoLineInputAction.Instance.CalculateLineIndex(10000, 100);
        Assert.AreEqual(100, lineIndex);

        var offsetLine = GotoLineInputAction.Instance.CalculateOffsetLine(lineIndex, 20);
        Assert.AreEqual(90, offsetLine);

        var scrollBarValue = GotoLineInputAction.Instance.CalculateVerticalScrollBarValue(offsetLine, 3);
        Assert.AreEqual(90 * 3, scrollBarValue);
    }

    [Test]
    public void LineIndexNegative_Error()
    {
        try
        {
            GotoLineInputAction.Instance.Do(null, -1);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.AreEqual("lineIndex", e.Message);
        }
    }
}
