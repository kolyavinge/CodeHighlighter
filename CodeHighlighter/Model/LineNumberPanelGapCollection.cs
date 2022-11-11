using CodeHighlighter.Utils;

namespace CodeHighlighter.Model;

public class LineNumberPanelGapCollection : SpreadCollection<LineNumberPanelGap>
{
}

public class LineNumberPanelGap
{
    public int CountBefore;

    public LineNumberPanelGap(int countBeforeLine)
    {
        CountBefore = countBeforeLine;
    }
}
