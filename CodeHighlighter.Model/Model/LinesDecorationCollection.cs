using CodeHighlighter.Common;

namespace CodeHighlighter.Model;

public interface ILinesDecorationCollection
{
    bool AnyItems { get; }
    LineDecoration? this[int lineIndex] { get; set; }
    void Clear();
}

public class LinesDecorationCollection : SpreadCollection<LineDecoration>, ILinesDecorationCollection
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
