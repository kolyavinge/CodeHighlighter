using System.Linq;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class LineFoldsTest
{
    private LineFolds _lineFolds;

    [SetUp]
    public void Setup()
    {
        _lineFolds = new LineFolds();
    }

    [Test]
    public void Constructor()
    {
        Assert.False(_lineFolds.AnyItems);
    }

    [Test]
    public void SetItems()
    {
        var items = new LineFold[]
        {
            new(1, 5),
            new(8, 2),
            new(14, 3)
        };
        _lineFolds.SetItems(items);

        Assert.True(_lineFolds.AnyItems);

        var lineFoldsItems = _lineFolds.Items.ToList();
        Assert.That(lineFoldsItems[0].LineIndex, Is.EqualTo(1));
        Assert.That(lineFoldsItems[0].LinesCount, Is.EqualTo(5));
        Assert.False(lineFoldsItems[0].IsActive);

        Assert.That(lineFoldsItems[1].LineIndex, Is.EqualTo(8));
        Assert.That(lineFoldsItems[1].LinesCount, Is.EqualTo(2));
        Assert.False(lineFoldsItems[1].IsActive);

        Assert.That(lineFoldsItems[2].LineIndex, Is.EqualTo(14));
        Assert.That(lineFoldsItems[2].LinesCount, Is.EqualTo(3));
        Assert.False(lineFoldsItems[2].IsActive);
    }

    [Test]
    public void SetItems_RaiseItemsSet()
    {
        var raised = 0;
        _lineFolds.ItemsSet += (s, e) => raised++;
        var items = new LineFold[]
        {
            new(1, 5)
        };
        _lineFolds.SetItems(items);

        Assert.That(raised, Is.EqualTo(1));
    }

    [Test]
    public void Activate_RaiseEvent()
    {
        var raised = 0;
        _lineFolds.Activated += (s, e) => { raised++; };
        var items = new LineFold[] { new(1, 5) };
        _lineFolds.SetItems(items);

        _lineFolds.Activate(new[] { 1 });

        Assert.That(raised, Is.EqualTo(1));
    }

    [Test]
    public void Deactivate_RaiseEvent()
    {
        var raised = 0;
        _lineFolds.Deactivated += (s, e) => { raised++; };
        var items = new LineFold[] { new(1, 5) };
        _lineFolds.SetItems(items);

        _lineFolds.Deactivate(new[] { 1 });

        Assert.That(raised, Is.EqualTo(1));
    }

    [Test]
    public void Activate_WrongLine()
    {
        var items = new LineFold[] { new(1, 5) };
        _lineFolds.SetItems(items);
        _lineFolds.Activate(new[] { 0 });
    }

    [Test]
    public void Deactivate_WrongLine()
    {
        var items = new LineFold[] { new(1, 5) };
        _lineFolds.SetItems(items);
        _lineFolds.Deactivate(new[] { 0 });
    }

    [Test]
    public void IsFolded_ActivateDeactivate()
    {
        var items = new LineFold[] { new(1, 3) };
        _lineFolds.SetItems(items);
        Assert.False(_lineFolds.IsFolded(1));
        Assert.False(_lineFolds.IsFolded(2));
        Assert.False(_lineFolds.IsFolded(3));
        Assert.False(_lineFolds.IsFolded(4));

        _lineFolds.Activate(new[] { 1 });
        Assert.False(_lineFolds.IsFolded(1));
        Assert.True(_lineFolds.IsFolded(2));
        Assert.True(_lineFolds.IsFolded(3));
        Assert.True(_lineFolds.IsFolded(4));

        _lineFolds.Deactivate(new[] { 1 });
        Assert.False(_lineFolds.IsFolded(1));
        Assert.False(_lineFolds.IsFolded(2));
        Assert.False(_lineFolds.IsFolded(3));
        Assert.False(_lineFolds.IsFolded(4));
    }

    [Test]
    public void GetUnfoldedLineIndexUp()
    {
        var items = new LineFold[] { new(1, 3) };
        _lineFolds.SetItems(items);
        _lineFolds.Activate(new[] { 1 });

        Assert.That(_lineFolds.GetUnfoldedLineIndexUp(0), Is.EqualTo(0));
        Assert.That(_lineFolds.GetUnfoldedLineIndexUp(1), Is.EqualTo(1));
        Assert.That(_lineFolds.GetUnfoldedLineIndexUp(2), Is.EqualTo(1));
        Assert.That(_lineFolds.GetUnfoldedLineIndexUp(3), Is.EqualTo(1));
        Assert.That(_lineFolds.GetUnfoldedLineIndexUp(4), Is.EqualTo(1));
        Assert.That(_lineFolds.GetUnfoldedLineIndexUp(5), Is.EqualTo(5));
    }

    [Test]
    public void GetUnfoldedLineIndexUp_NoActivation()
    {
        var items = new LineFold[] { new(1, 3) };
        _lineFolds.SetItems(items);

        Assert.That(_lineFolds.GetUnfoldedLineIndexUp(0), Is.EqualTo(0));
        Assert.That(_lineFolds.GetUnfoldedLineIndexUp(1), Is.EqualTo(1));
        Assert.That(_lineFolds.GetUnfoldedLineIndexUp(2), Is.EqualTo(2));
        Assert.That(_lineFolds.GetUnfoldedLineIndexUp(3), Is.EqualTo(3));
        Assert.That(_lineFolds.GetUnfoldedLineIndexUp(4), Is.EqualTo(4));
        Assert.That(_lineFolds.GetUnfoldedLineIndexUp(5), Is.EqualTo(5));
    }

    [Test]
    public void GetUnfoldedLineIndexDown()
    {
        var items = new LineFold[] { new(1, 3) };
        _lineFolds.SetItems(items);
        _lineFolds.Activate(new[] { 1 });

        Assert.That(_lineFolds.GetUnfoldedLineIndexDown(0), Is.EqualTo(0));
        Assert.That(_lineFolds.GetUnfoldedLineIndexDown(1), Is.EqualTo(1));
        Assert.That(_lineFolds.GetUnfoldedLineIndexDown(2), Is.EqualTo(5));
        Assert.That(_lineFolds.GetUnfoldedLineIndexDown(3), Is.EqualTo(5));
        Assert.That(_lineFolds.GetUnfoldedLineIndexDown(4), Is.EqualTo(5));
        Assert.That(_lineFolds.GetUnfoldedLineIndexDown(5), Is.EqualTo(5));
    }

    [Test]
    public void GetUnfoldedLineIndexDown_NoActivation()
    {
        var items = new LineFold[] { new(1, 3) };
        _lineFolds.SetItems(items);

        Assert.That(_lineFolds.GetUnfoldedLineIndexDown(0), Is.EqualTo(0));
        Assert.That(_lineFolds.GetUnfoldedLineIndexDown(1), Is.EqualTo(1));
        Assert.That(_lineFolds.GetUnfoldedLineIndexDown(2), Is.EqualTo(2));
        Assert.That(_lineFolds.GetUnfoldedLineIndexDown(3), Is.EqualTo(3));
        Assert.That(_lineFolds.GetUnfoldedLineIndexDown(4), Is.EqualTo(4));
        Assert.That(_lineFolds.GetUnfoldedLineIndexDown(5), Is.EqualTo(5));
    }

    [Test]
    public void UpdateAfterLineAdd_1()
    {
        var items = new LineFold[] { new(1, 3), new(5, 3) };
        _lineFolds.SetItems(items);

        _lineFolds.UpdateAfterLineAdd(3, 2);

        var lineFoldsItems = _lineFolds.Items.ToList();
        Assert.That(lineFoldsItems[0].LineIndex, Is.EqualTo(1));
        Assert.That(lineFoldsItems[0].LinesCount, Is.EqualTo(3));
        Assert.False(lineFoldsItems[0].IsActive);
        Assert.That(lineFoldsItems[1].LineIndex, Is.EqualTo(7));
        Assert.That(lineFoldsItems[1].LinesCount, Is.EqualTo(3));
        Assert.False(lineFoldsItems[1].IsActive);
    }

    [Test]
    public void UpdateAfterLineAdd_2()
    {
        var items = new LineFold[] { new(0, 3) };
        _lineFolds.SetItems(items);

        _lineFolds.UpdateAfterLineAdd(0, 2);

        var lineFoldsItems = _lineFolds.Items.ToList();
        Assert.That(lineFoldsItems[0].LineIndex, Is.EqualTo(2));
        Assert.That(lineFoldsItems[0].LinesCount, Is.EqualTo(3));
        Assert.False(lineFoldsItems[0].IsActive);
    }

    [Test]
    public void UpdateAfterLineAdd_Activated()
    {
        var items = new LineFold[] { new(1, 3), new(5, 3) };
        _lineFolds.SetItems(items);
        _lineFolds.Activate(new[] { 5 });

        _lineFolds.UpdateAfterLineAdd(3, 2);

        Assert.False(_lineFolds.IsFolded(2));
        Assert.False(_lineFolds.IsFolded(3));
        Assert.False(_lineFolds.IsFolded(4));
        Assert.True(_lineFolds.IsFolded(8));
        Assert.True(_lineFolds.IsFolded(9));
        Assert.True(_lineFolds.IsFolded(10));
    }

    [Test]
    public void UpdateAfterLineDelete()
    {
        var items = new LineFold[] { new(1, 3), new(5, 3) };
        _lineFolds.SetItems(items);

        _lineFolds.UpdateAfterLineDelete(3, 1);

        var lineFoldsItems = _lineFolds.Items.ToList();
        Assert.That(lineFoldsItems[0].LineIndex, Is.EqualTo(1));
        Assert.That(lineFoldsItems[0].LinesCount, Is.EqualTo(3));
        Assert.False(lineFoldsItems[0].IsActive);
        Assert.That(lineFoldsItems[1].LineIndex, Is.EqualTo(4));
        Assert.That(lineFoldsItems[1].LinesCount, Is.EqualTo(3));
        Assert.False(lineFoldsItems[1].IsActive);
    }

    [Test]
    public void UpdateAfterLineDelete_Activated()
    {
        var items = new LineFold[] { new(1, 3), new(5, 3) };
        _lineFolds.SetItems(items);
        _lineFolds.Activate(new[] { 5 });

        _lineFolds.UpdateAfterLineDelete(3, 1);

        Assert.False(_lineFolds.IsFolded(1));
        Assert.False(_lineFolds.IsFolded(2));
        Assert.False(_lineFolds.IsFolded(3));
        Assert.True(_lineFolds.IsFolded(5));
        Assert.True(_lineFolds.IsFolded(6));
        Assert.True(_lineFolds.IsFolded(7));
    }
}
