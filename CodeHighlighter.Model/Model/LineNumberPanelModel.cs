using System.Collections.Generic;

namespace CodeHighlighter.Model;

internal class LineNumberPanelModel : ILineNumberPanelModel
{
    private readonly IExtendedLineNumberGenerator _lineNumberGenerator;

    public ILineNumberGapCollection Gaps { get; }

    public LineNumberPanelModel(
        IExtendedLineNumberGenerator lineNumberGenerator,
        ILineNumberGapCollection gaps)
    {
        _lineNumberGenerator = lineNumberGenerator;
        Gaps = gaps;
    }

    public IEnumerable<LineNumber> GetLines(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount)
    {
        return _lineNumberGenerator.GetLineNumbers(controlHeight, verticalScrollBarValue, textLineHeight, textLinesCount);
    }
}
