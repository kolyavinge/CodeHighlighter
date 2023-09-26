using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Common;

public class SpreadCollection<T> : IEnumerable<T>
{
    public readonly Dictionary<int, T> _items = new();

    public bool AnyItems => _items.Any();

    public T? this[int lineIndex]
    {
        get
        {
            _items.TryGetValue(lineIndex, out T? result);
            return result;
        }
        set
        {
            if (value is not null)
            {
                if (_items.ContainsKey(lineIndex))
                {
                    _items.Remove(lineIndex);
                }
                _items.Add(lineIndex, value);
            }
            else
            {
                if (_items.ContainsKey(lineIndex))
                {
                    _items.Remove(lineIndex);
                }
            }
        }
    }

    public void Clear()
    {
        _items.Clear();
    }

    public IEnumerator<T> GetEnumerator() => _items.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _items.Values.GetEnumerator();
}
