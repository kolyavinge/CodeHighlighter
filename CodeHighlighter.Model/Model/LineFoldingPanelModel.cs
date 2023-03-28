using System.Collections.Generic;

namespace CodeHighlighter.Model;

public interface ILineFoldingPanelModel
{
    ILineFolds Folds { get; }
    void AttachLineFoldingPanel(ILineFoldingPanel panel);
    IEnumerable<LineFoldWithOffsetY> GetFolds(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount);
}

internal class LineFoldingPanelModel : ILineFoldingPanelModel
{
    private readonly ILineFoldsNumberGenerator _lineNumberGenerator;

    public ILineFolds Folds { get; }

    public LineFoldingPanelModel(ILineFolds folds, ILineFoldsNumberGenerator lineNumberGenerator)
    {
        Folds = folds;
        _lineNumberGenerator = lineNumberGenerator;
    }

    public void AttachLineFoldingPanel(ILineFoldingPanel panel)
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
