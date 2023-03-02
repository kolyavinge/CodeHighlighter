using System.Collections.Generic;

namespace CodeHighlighter.Model;

internal class LineNumberPanelModel : ILineNumberPanelModel
{
    private readonly IExtendedLineNumberGenerator _lineNumberGenerator;

    public ILineGapCollection Gaps { get; }

    public LineNumberPanelModel(
        IExtendedLineNumberGenerator lineNumberGenerator,
        ILineGapCollection gaps)
    {
        _lineNumberGenerator = lineNumberGenerator;
        Gaps = gaps;
    }

    public void AttachLineNumberPanel(ILineNumberPanel panel)
    {
    }

    public IEnumerable<LineNumber> GetLines(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount)
    {
        return _lineNumberGenerator.GetLineNumbers(controlHeight, verticalScrollBarValue, textLineHeight, textLinesCount);
    }
}
