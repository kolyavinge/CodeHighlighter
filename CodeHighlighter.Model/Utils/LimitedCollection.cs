using System.Collections;
using System.Collections.Generic;

namespace CodeHighlighter.Utils;

internal class LimitedCollection<T> : IReadOnlyList<T>
{
    private readonly List<T> _items = new();
    private readonly int _limit;

    public int Count => _items.Count;

    public bool HasLimit => _items.Count == _limit;

    public LimitedCollection(int limit)
    {
        _limit = limit;
    }

    public T this[int index]
    {
        get => _items[index];
        set => _items[index] = value;
    }

    public void Add(T item)
    {
        _items.Add(item);
        if (_items.Count > _limit)
        {
            _items.RemoveAt(0);
        }
    }

    public void RemoveRange(int index, int count)
    {
        _items.RemoveRange(index, count);
    }

    public void Remove(T item)
    {
        _items.Remove(item);
    }

    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
}
