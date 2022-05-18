using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

public class ViewportTest
{
    private const double _lineHeight = 10;
    private const double _verticalMaximum = 10 * _lineHeight;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetVerticalOffsetAfterScrollLineUp()
    {
        Assert.AreEqual(0, Viewport.GetVerticalOffsetAfterScrollLineUp(0, _lineHeight));
        Assert.AreEqual(0, Viewport.GetVerticalOffsetAfterScrollLineUp(0, _lineHeight));
        Assert.AreEqual(2 * _lineHeight, Viewport.GetVerticalOffsetAfterScrollLineUp(3 * _lineHeight, _lineHeight));
        Assert.AreEqual(_lineHeight, Viewport.GetVerticalOffsetAfterScrollLineUp(2 * _lineHeight, _lineHeight));
        Assert.AreEqual(0, Viewport.GetVerticalOffsetAfterScrollLineUp(_lineHeight, _lineHeight));
        Assert.AreEqual(0, Viewport.GetVerticalOffsetAfterScrollLineUp(0, _lineHeight));
    }

    [Test]
    public void GetVerticalOffsetAfterScrollLineUp_HalfLine()
    {
        Assert.AreEqual(2 * _lineHeight, Viewport.GetVerticalOffsetAfterScrollLineUp(2.5 * _lineHeight, _lineHeight));
        Assert.AreEqual(_lineHeight, Viewport.GetVerticalOffsetAfterScrollLineUp(2 * _lineHeight, _lineHeight));

        Assert.AreEqual(0, Viewport.GetVerticalOffsetAfterScrollLineUp(0.5 * _lineHeight, _lineHeight));
        Assert.AreEqual(0, Viewport.GetVerticalOffsetAfterScrollLineUp(0, _lineHeight));
    }

    [Test]
    public void GetVerticalOffsetAfterScrollLineDown()
    {
        Assert.AreEqual(_lineHeight, Viewport.GetVerticalOffsetAfterScrollLineDown(0, _verticalMaximum, _lineHeight));
        Assert.AreEqual(2 * _lineHeight, Viewport.GetVerticalOffsetAfterScrollLineDown(_lineHeight, _verticalMaximum, _lineHeight));
        Assert.AreEqual(10 * _lineHeight, Viewport.GetVerticalOffsetAfterScrollLineDown(9 * _lineHeight, _verticalMaximum, _lineHeight));
        Assert.AreEqual(10 * _lineHeight, Viewport.GetVerticalOffsetAfterScrollLineDown(10 * _lineHeight, _verticalMaximum, _lineHeight));
    }

    [Test]
    public void GetVerticalOffsetAfterScrollLineDown_HalfLine()
    {
        Assert.AreEqual(3 * _lineHeight, Viewport.GetVerticalOffsetAfterScrollLineDown(2.5 * _lineHeight, _verticalMaximum, _lineHeight));
        Assert.AreEqual(4 * _lineHeight, Viewport.GetVerticalOffsetAfterScrollLineDown(3 * _lineHeight, _verticalMaximum, _lineHeight));

        Assert.AreEqual(10 * _lineHeight, Viewport.GetVerticalOffsetAfterScrollLineDown(9.5 * _lineHeight, _verticalMaximum, _lineHeight));
        Assert.AreEqual(10 * _lineHeight, Viewport.GetVerticalOffsetAfterScrollLineDown(10 * _lineHeight, _verticalMaximum, _lineHeight));
    }
}
