using System.Collections.Generic;
using CodeHighlighter.Ancillary;

namespace CodeHighlighter.Model;

internal class LineNumberPanelModel : ILineNumberPanelModel
{
    private readonly IExtendedLineNumberGenerator _lineNumberGenerator;
    private readonly ILineFolds _folds;

    public ILineGapCollection Gaps { get; }

    public LineNumberPanelModel(
        IExtendedLineNumberGenerator lineNumberGenerator,
        ILineGapCollection gaps,
        ILineFolds folds)
    {
        _lineNumberGenerator = lineNumberGenerator;
        _folds = folds;
        Gaps = gaps;
    }

    public void AttachLineNumberPanel(ILineNumberPanel panel)
    {
        _folds.ItemsSet += (s, e) => panel.InvalidateVisual();
        _folds.Activated += (s, e) => panel.InvalidateVisual();
        _folds.Deactivated += (s, e) => panel.InvalidateVisual();
    }

    public IEnumerable<LineNumber> GetLines(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount)
    {
        return _lineNumberGenerator.GetLineNumbers(controlHeight, verticalScrollBarValue, textLineHeight, textLinesCount);
    }
}
