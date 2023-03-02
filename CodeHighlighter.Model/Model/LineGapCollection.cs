using System.Collections.Generic;
using CodeHighlighter.Utils;

namespace CodeHighlighter.Model;

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
