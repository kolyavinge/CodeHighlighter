using CodeHighlighter.Utils;

namespace CodeHighlighter.Model;

public interface ILineNumberGapCollection
{
    bool AnyItems { get; }
    LineNumberGap? this[int lineIndex] { get; set; }
    void Clear();
}

public class LineNumberGapCollection : SpreadCollection<LineNumberGap>, ILineNumberGapCollection
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
