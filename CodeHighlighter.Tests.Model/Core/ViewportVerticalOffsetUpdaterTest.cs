using CodeHighlighter.Core;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Core;

internal class ViewportVerticalOffsetUpdaterTest
{
    private const double _lineHeight = 10;
    private const double _verticalMaximum = 10 * _lineHeight;

    private ViewportVerticalOffsetUpdater _updater;

    [SetUp]
    public void Setup()
    {
        _updater = new ViewportVerticalOffsetUpdater();
    }

    [Test]
    public void GetVerticalOffsetAfterScrollLineUp()
    {
        Assert.AreEqual(0, _updater.GetVerticalOffsetAfterScrollLineUp(0, _lineHeight));
        Assert.AreEqual(0, _updater.GetVerticalOffsetAfterScrollLineUp(0, _lineHeight));
        Assert.AreEqual(2 * _lineHeight, _updater.GetVerticalOffsetAfterScrollLineUp(3 * _lineHeight, _lineHeight));
        Assert.AreEqual(_lineHeight, _updater.GetVerticalOffsetAfterScrollLineUp(2 * _lineHeight, _lineHeight));
        Assert.AreEqual(0, _updater.GetVerticalOffsetAfterScrollLineUp(_lineHeight, _lineHeight));
        Assert.AreEqual(0, _updater.GetVerticalOffsetAfterScrollLineUp(0, _lineHeight));
    }

    [Test]
    public void GetVerticalOffsetAfterScrollLineUp_HalfLine()
    {
        Assert.AreEqual(2 * _lineHeight, _updater.GetVerticalOffsetAfterScrollLineUp(2.5 * _lineHeight, _lineHeight));
        Assert.AreEqual(_lineHeight, _updater.GetVerticalOffsetAfterScrollLineUp(2 * _lineHeight, _lineHeight));

        Assert.AreEqual(0, _updater.GetVerticalOffsetAfterScrollLineUp(0.5 * _lineHeight, _lineHeight));
        Assert.AreEqual(0, _updater.GetVerticalOffsetAfterScrollLineUp(0, _lineHeight));
    }

    [Test]
    public void GetVerticalOffsetAfterScrollLineDown()
    {
        Assert.AreEqual(_lineHeight, _updater.GetVerticalOffsetAfterScrollLineDown(0, _verticalMaximum, _lineHeight));
        Assert.AreEqual(2 * _lineHeight, _updater.GetVerticalOffsetAfterScrollLineDown(_lineHeight, _verticalMaximum, _lineHeight));
        Assert.AreEqual(10 * _lineHeight, _updater.GetVerticalOffsetAfterScrollLineDown(9 * _lineHeight, _verticalMaximum, _lineHeight));
        Assert.AreEqual(10 * _lineHeight, _updater.GetVerticalOffsetAfterScrollLineDown(10 * _lineHeight, _verticalMaximum, _lineHeight));
    }

    [Test]
    public void GetVerticalOffsetAfterScrollLineDown_HalfLine()
    {
        Assert.AreEqual(3 * _lineHeight, _updater.GetVerticalOffsetAfterScrollLineDown(2.5 * _lineHeight, _verticalMaximum, _lineHeight));
        Assert.AreEqual(4 * _lineHeight, _updater.GetVerticalOffsetAfterScrollLineDown(3 * _lineHeight, _verticalMaximum, _lineHeight));

        Assert.AreEqual(10 * _lineHeight, _updater.GetVerticalOffsetAfterScrollLineDown(9.5 * _lineHeight, _verticalMaximum, _lineHeight));
        Assert.AreEqual(10 * _lineHeight, _updater.GetVerticalOffsetAfterScrollLineDown(10 * _lineHeight, _verticalMaximum, _lineHeight));
    }
}
