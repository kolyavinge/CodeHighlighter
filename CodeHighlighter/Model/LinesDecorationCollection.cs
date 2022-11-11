using System.Windows.Media;
using CodeHighlighter.Utils;

namespace CodeHighlighter.Model;

public class LinesDecorationCollection : SpreadCollection<LineDecoration>
{
}

public class LineDecoration
{
    public Brush Background { get; set; }

    public LineDecoration()
    {
        Background = Brushes.Transparent;
    }
}
