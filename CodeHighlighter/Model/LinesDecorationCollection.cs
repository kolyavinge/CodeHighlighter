using CodeHighlighter.Common;
using CodeHighlighter.Utils;

namespace CodeHighlighter.Model;

public class LinesDecorationCollection : SpreadCollection<LineDecoration>
{
}

public class LineDecoration
{
    public Color Background { get; set; }

    public LineDecoration()
    {
        Background = default;
    }
}
