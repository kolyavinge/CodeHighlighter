using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model;

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

public interface ILineFoldsNumberGenerator
{
    IEnumerable<LineFoldWithOffsetY> GetFolds(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount);
}

internal class LineFoldsNumberGenerator : ILineFoldsNumberGenerator
{
    private readonly ILineFolds _folds;
    private readonly IExtendedLineNumberGenerator _lineNumberGenerator;

    public LineFoldsNumberGenerator(
        ILineFolds folds,
        IExtendedLineNumberGenerator lineNumberGenerator)
    {
        _folds = folds;
        _lineNumberGenerator = lineNumberGenerator;
    }

    public IEnumerable<LineFoldWithOffsetY> GetFolds(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount)
    {
        var lineNumbers = _lineNumberGenerator.GetLineNumbers(controlHeight, verticalScrollBarValue, textLineHeight, textLinesCount);
        var foldsWithOffset = from line in lineNumbers
                              join fold in _folds.Items
                              on line.LineIndex equals fold.LineIndex
                              select new LineFoldWithOffsetY(line.LineIndex, line.OffsetY, fold.IsActive);

        return foldsWithOffset;
    }
}
