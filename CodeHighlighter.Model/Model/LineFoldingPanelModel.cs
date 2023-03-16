using System.Collections.Generic;
using System.Linq;
using static CodeHighlighter.Model.ILineFoldingPanelModel;

namespace CodeHighlighter.Model;

public interface ILineFoldingPanelModel
{
    public readonly struct LineFoldWithOffsetY
    {
        public readonly int LineIndex;
        public readonly double OffsetY;
        public readonly bool IsActive;

        public LineFoldWithOffsetY(int lineIndex, double offsetY, bool isActive)
        {
            LineIndex = lineIndex;
            OffsetY = offsetY;
            IsActive = isActive;
        }
    }

    ILineFolds Folds { get; }
    void AttachLineFoldingPanel(ILineFoldingPanel panel);
    IEnumerable<LineFoldWithOffsetY> GetFolds(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount);
}

internal class LineFoldingPanelModel : ILineFoldingPanelModel
{
    private readonly IExtendedLineNumberGenerator _lineNumberGenerator;

    public ILineFolds Folds { get; }

    public LineFoldingPanelModel(ILineFolds folds, IExtendedLineNumberGenerator lineNumberGenerator)
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
        var lineNumbers = _lineNumberGenerator.GetLineNumbers(controlHeight, verticalScrollBarValue, textLineHeight, textLinesCount);
        var foldsWithOffset = from line in lineNumbers
                              join fold in Folds.Items
                              on line.LineIndex equals fold.LineIndex
                              select new LineFoldWithOffsetY(line.LineIndex, line.OffsetY, fold.IsActive);

        return foldsWithOffset;
    }
}
