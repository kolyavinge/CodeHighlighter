using System.Collections.Generic;
using CodeHighlighter.Ancillary;
using CodeHighlighter.Core;

namespace CodeHighlighter.Model;

public interface ILineNumberPanelView
{
    void InvalidateVisual();
}

public interface ILineNumberPanel
{
    ILineGapCollection Gaps { get; }

    void AttachLineNumberPanel(ILineNumberPanelView panel);

    IEnumerable<LineNumber> GetLines(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount);
}

internal class LineNumberPanel : ILineNumberPanel
{
    private readonly IExtendedLineNumberGenerator _lineNumberGenerator;
    private readonly ILineFolds _folds;

    public ILineGapCollection Gaps { get; }

    public LineNumberPanel(
        IExtendedLineNumberGenerator lineNumberGenerator,
        ILineGapCollection gaps,
        ILineFolds folds)
    {
        _lineNumberGenerator = lineNumberGenerator;
        _folds = folds;
        Gaps = gaps;
    }

    public void AttachLineNumberPanel(ILineNumberPanelView panel)
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
