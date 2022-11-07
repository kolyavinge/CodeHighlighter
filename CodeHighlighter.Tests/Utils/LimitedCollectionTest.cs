using System.Linq;
using CodeHighlighter.Utils;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Utils;

internal class LimitedCollectionTest
{
    private LimitedCollection<int> _collection;

    [Test]
    public void Add()
    {
        _collection = new LimitedCollection<int>(100);
        for (int i = 0; i < 100; i++)
        {
            _collection.Add(i);
        }

        for (int i = 0; i < 100; i++)
        {
            Assert.That(_collection[i], Is.EqualTo(i));
        }
    }

    [Test]
    public void Add_Limit()
    {
        _collection = new LimitedCollection<int>(100);
        for (int i = 0; i < 100; i++)
        {
            _collection.Add(i);
        }

        _collection.Add(100);
        Assert.That(_collection.First(), Is.EqualTo(1));
        Assert.That(_collection.Last(), Is.EqualTo(100));

        _collection.Add(101);
        Assert.That(_collection.First(), Is.EqualTo(2));
        Assert.That(_collection.Last(), Is.EqualTo(101));
    }
}
