using System.Collections.Generic;
using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class LineFoldsItemsUpdaterTest
{
    private List<LineFold> _oldItems;
    private List<LineFold> _newItems;
    private Mock<ILineFolds> _folds;
    private LineFoldsItemsUpdater _updater;

    [SetUp]
    public void Setup()
    {
        _oldItems = new List<LineFold>();
        _newItems = new List<LineFold>();
        _folds = new Mock<ILineFolds>();
        _updater = new LineFoldsItemsUpdater();
    }

    [Test]
    public void Update_EmptyOldItems()
    {
        _newItems.Add(new(1, 2));
        _newItems.Add(new(5, 3));

        Update();

        _folds.VerifySet(x => x.Items = _newItems, Times.Once());
    }

    [Test]
    public void Update_EqualItems()
    {
        _oldItems.Add(new(1, 2));
        _oldItems.Add(new(5, 3));
        _newItems.Add(new(1, 2));
        _newItems.Add(new(5, 3));

        Update();

        _folds.VerifySet(x => x.Items = _newItems, Times.Never());
    }

    [Test]
    public void Update_OneOldOneNew()
    {
        var lf = new LineFold(5, 3);
        _oldItems.Add(new(1, 2));
        _oldItems.Add(lf);
        _newItems.Add(new(5, 3));

        Update();

        _folds.VerifySet(x => x.Items = new List<LineFold> { lf }, Times.Once());
    }

    [Test]
    public void Update_DifferentLinesCount()
    {
        _oldItems.Add(new(1, 2));
        _newItems.Add(new(1, 3));

        Update();

        _folds.VerifySet(x => x.Items = _newItems, Times.Once());
    }

    [Test]
    public void Update_AllNew()
    {
        _oldItems.Add(new(1, 2));
        _oldItems.Add(new(5, 3));
        _newItems.Add(new(10, 2));
        _newItems.Add(new(50, 3));

        Update();

        _folds.VerifySet(x => x.Items = _newItems, Times.Once());
    }

    [Test]
    public void Update_EmptyNew()
    {
        _oldItems.Add(new(1, 2));
        _oldItems.Add(new(5, 3));

        Update();

        _folds.VerifySet(x => x.Items = _newItems, Times.Once());
    }

    private void Update()
    {
        _folds.SetupGet(x => x.Items).Returns(_oldItems);
        _updater.Update(_folds.Object, _newItems);
    }
}
