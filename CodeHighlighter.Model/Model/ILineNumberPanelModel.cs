using System.Collections.Generic;

namespace CodeHighlighter.Model;

public interface ILineNumberPanelModel
{
    ILineGapCollection Gaps { get; }

    void AttachLineNumberPanel(ILineNumberPanel panel);

    IEnumerable<LineNumber> GetLines(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount);
}
