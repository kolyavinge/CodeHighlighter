using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model;

public class LineNumberPanelGapCollection
{
    public readonly Dictionary<int, LineNumberPanelGap> _gaps = new();

    public bool AnyItems => _gaps.Any();

    public LineNumberPanelGap? this[int lineIndex]
    {
        get
        {
            _gaps.TryGetValue(lineIndex, out LineNumberPanelGap result);
            return result;
        }
        set
        {
            if (value != null)
            {
                if (_gaps.ContainsKey(lineIndex))
                {
                    _gaps.Remove(lineIndex);
                }
                _gaps.Add(lineIndex, value.Value);
            }
            else
            {
                if (_gaps.ContainsKey(lineIndex))
                {
                    _gaps.Remove(lineIndex);
                }
            }
        }
    }

    public void Clear()
    {
        _gaps.Clear();
    }
}

public readonly struct LineNumberPanelGap
{
    public readonly int CountBeforeLine;

    public LineNumberPanelGap(int countBeforeLine)
    {
        CountBeforeLine = countBeforeLine;
    }
}
