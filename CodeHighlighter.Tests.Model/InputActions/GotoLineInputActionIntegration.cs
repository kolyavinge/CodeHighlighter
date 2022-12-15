using System;
using CodeHighlighter.InputActions;
using NUnit.Framework;

namespace CodeHighlighter.Tests.InputActions;

internal class GotoLineInputActionIntegration : BaseInputActionIntegration
{
    private GotoLineInputAction _action;

    [SetUp]
    public void Setup()
    {
        _action = new GotoLineInputAction(_inputActionFactory);
    }

    [Test]
    public void GotoFirstLine()
    {
        var lineIndex = _action.CalculateLineIndex(0, 100);
        Assert.AreEqual(0, lineIndex);

        var offsetLine = _action.CalculateOffsetLine(lineIndex, 20);
        Assert.AreEqual(0, offsetLine);

        var scrollBarValue = _action.CalculateVerticalScrollBarValue(offsetLine, 3);
        Assert.AreEqual(0, scrollBarValue);
    }

    [Test]
    public void GotoLastLine()
    {
        var lineIndex = _action.CalculateLineIndex(99, 100);
        Assert.AreEqual(99, lineIndex);

        var offsetLine = _action.CalculateOffsetLine(lineIndex, 20);
        Assert.AreEqual(89, offsetLine);

        var scrollBarValue = _action.CalculateVerticalScrollBarValue(offsetLine, 3);
        Assert.AreEqual(89 * 3, scrollBarValue);
    }

    [Test]
    public void GotoFarAway()
    {
        var lineIndex = _action.CalculateLineIndex(10000, 100);
        Assert.AreEqual(100, lineIndex);

        var offsetLine = _action.CalculateOffsetLine(lineIndex, 20);
        Assert.AreEqual(90, offsetLine);

        var scrollBarValue = _action.CalculateVerticalScrollBarValue(offsetLine, 3);
        Assert.AreEqual(90 * 3, scrollBarValue);
    }

    [Test]
    public void LineIndexNegative_Error()
    {
        try
        {
            _action.Do(null, -1);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.AreEqual("lineIndex", e.Message);
        }
    }
}
