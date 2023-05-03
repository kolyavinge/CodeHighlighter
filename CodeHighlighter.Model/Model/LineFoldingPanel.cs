using System.Collections.Generic;
using CodeHighlighter.Ancillary;

namespace CodeHighlighter.Model;

public interface ILineFoldingPanelView
{
    double ActualHeight { get; }
    double VerticalScrollBarValue { get; }
    int TextLinesCount { get; }
    double TextLineHeight { get; }
    void InvalidateVisual();
}

public interface ILineFoldingPanel
{
    ILineFolds Folds { get; }
    void AttachLineFoldingPanel(ILineFoldingPanelView panel);
    IEnumerable<LineFoldWithOffsetY> GetFolds(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount);
}

internal class LineFoldingPanel : ILineFoldingPanel
{
    private readonly ILineFoldsNumberGenerator _lineNumberGenerator;

    public ILineFolds Folds { get; }

    public LineFoldingPanel(ILineFolds folds, ILineFoldsNumberGenerator lineNumberGenerator)
    {
        Folds = folds;
        _lineNumberGenerator = lineNumberGenerator;
    }

    public void AttachLineFoldingPanel(ILineFoldingPanelView panel)
    {
        Folds.ItemsSet += (s, e) => panel.InvalidateVisual();
        Folds.Activated += (s, e) => panel.InvalidateVisual();
        Folds.Deactivated += (s, e) => panel.InvalidateVisual();
    }

    public IEnumerable<LineFoldWithOffsetY> GetFolds(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount)
    {
        return _lineNumberGenerator.GetFolds(controlHeight, verticalScrollBarValue, textLineHeight, textLinesCount);
    }
}
