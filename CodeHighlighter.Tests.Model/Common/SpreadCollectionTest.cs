using CodeHighlighter.Common;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Common;

internal class SpreadCollectionTest
{
    class Item { }

    private SpreadCollection<Item> _collection;

    [SetUp]
    public void Setup()
    {
        _collection = new SpreadCollection<Item>();
    }

    [Test]
    public void Indexer()
    {
        Assert.Null(_collection[0]);

        var item = new Item();
        _collection[0] = item;
        Assert.That(_collection[0], Is.EqualTo(item));

        Assert.NotNull(_collection[0]);
    }

    [Test]
    public void Indexer_UpdateItem()
    {
        _collection[0] = new Item();
        var item = new Item();
        _collection[0] = item;
        Assert.That(_collection[0], Is.EqualTo(item));

        Assert.NotNull(_collection[0]);
    }

    [Test]
    public void Indexer_Null()
    {
        _collection[0] = new Item();
        _collection[0] = null;

        Assert.Null(_collection[0]);
    }

    [Test]
    public void AnyItems()
    {
        Assert.False(_collection.AnyItems);

        _collection[0] = new Item();

        Assert.True(_collection.AnyItems);
    }

    [Test]
    public void Clear()
    {
        _collection[0] = new Item();
        _collection.Clear();

        Assert.False(_collection.AnyItems);
    }
}
