using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace CodeHighlighter.Rendering;

public class LinesDecorationCollection
{
    private readonly Dictionary<int, LineDecoration> _lineDecorations = new();

    public bool AnyLines => _lineDecorations.Any();

    public LineDecoration? this[int lineInex]
    {
        get
        {
            _lineDecorations.TryGetValue(lineInex, out LineDecoration? result);
            return result;
        }
        set
        {
            if (value != null)
            {
                if (_lineDecorations.ContainsKey(lineInex))
                {
                    _lineDecorations.Remove(lineInex);
                }
                _lineDecorations.Add(lineInex, value);
            }
            else
            {
                if (_lineDecorations.ContainsKey(lineInex))
                {
                    _lineDecorations.Remove(lineInex);
                }
            }
        }
    }

    public void Clear()
    {
        _lineDecorations.Clear();
    }
}

public class LineDecoration
{
    public Brush Background { get; set; }

    public LineDecoration()
    {
        Background = Brushes.Transparent;
    }
}
