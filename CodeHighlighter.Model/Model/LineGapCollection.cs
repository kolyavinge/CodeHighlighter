using CodeHighlighter.Utils;

namespace CodeHighlighter.Model;

public interface ILineGapCollection
{
    bool AnyItems { get; }
    LineGap? this[int lineIndex] { get; set; }
    void Clear();
}

public class LineGapCollection : SpreadCollection<LineGap>, ILineGapCollection
{
}

public class LineGap
{
    public uint CountBefore { get; set; }

    public LineGap(uint countBefore)
    {
        CountBefore = countBefore;
    }
}
