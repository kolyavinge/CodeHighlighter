using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Utils;

public class SpreadCollection<T>
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
            if (value != null)
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
}
