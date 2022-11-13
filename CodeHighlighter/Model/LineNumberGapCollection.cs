using CodeHighlighter.Utils;

namespace CodeHighlighter.Model;

public class LineNumberGapCollection : SpreadCollection<LineNumberGap>
{
}

public class LineNumberGap
{
    public uint CountBefore { get; set; }

    public LineNumberGap(uint countBefore)
    {
        CountBefore = countBefore;
    }
}
