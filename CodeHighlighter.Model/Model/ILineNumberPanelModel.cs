using System.Collections.Generic;
using CodeHighlighter.Ancillary;

namespace CodeHighlighter.Model;

public interface ILineNumberPanelModel
{
    ILineGapCollection Gaps { get; }

    void AttachLineNumberPanel(ILineNumberPanel panel);

    IEnumerable<LineNumber> GetLines(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount);
}
