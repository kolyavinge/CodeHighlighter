using System.Collections.Generic;

namespace CodeHighlighter.Model;

public interface ILineNumberPanelModel
{
    ILineNumberGapCollection Gaps { get; }

    IEnumerable<LineNumber> GetLines(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount);
}
