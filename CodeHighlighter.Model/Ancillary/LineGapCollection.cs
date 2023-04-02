using System.Collections.Generic;
using CodeHighlighter.Common;

namespace CodeHighlighter.Ancillary;

public class LineGap
{
    public int CountBefore { get; set; }

    public LineGap(int countBefore)
    {
        CountBefore = countBefore;
    }
}

public interface ILineGapCollection : IEnumerable<LineGap>
{
    bool AnyItems { get; }
    LineGap? this[int lineIndex] { get; set; }
    void Clear();
}

public class LineGapCollection : SpreadCollection<LineGap>, ILineGapCollection
{
}
