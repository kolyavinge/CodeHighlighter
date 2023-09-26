using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Ancillary;

namespace CodeHighlighter.Core;

public interface IExtendedLineNumberGenerator
{
    IEnumerable<LineNumber> GetLineNumbers(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount);
    double GetLineOffsetY(int lineIndex, double textLineHeight);
    int GetLineIndex(double mouseY, double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount);
}

internal class ExtendedLineNumberGenerator : IExtendedLineNumberGenerator
{
    private readonly ILineNumberGenerator _lineNumberGenerator;
    private readonly ILineGapCollection _gaps;
    private readonly ILineFolds _folds;

    public ExtendedLineNumberGenerator(
        ILineNumberGenerator lineNumberGenerator,
        ILineGapCollection gaps,
        ILineFolds folds)
    {
        _lineNumberGenerator = lineNumberGenerator;
        _gaps = gaps;
        _folds = folds;
    }

    public IEnumerable<LineNumber> GetLineNumbers(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount)
    {
        if (_gaps.AnyItems || _folds.AnyFoldedItems)
        {
            return GetLineNumbersModified(controlHeight, verticalScrollBarValue, textLineHeight, textLinesCount);
        }
        else
        {
            return _lineNumberGenerator.GetLineNumbers(controlHeight, verticalScrollBarValue, textLineHeight, textLinesCount);
        }
    }

    private IEnumerable<LineNumber> GetLineNumbersModified(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount)
    {
        var absoluteOffsetY = 0.0;
        var lines = _lineNumberGenerator.GetLineNumbers(
              controlHeight + verticalScrollBarValue + textLineHeight * _folds.FoldedLinesCount,  // TODO improve it
              0,
              textLineHeight,
              textLinesCount);
        foreach (var line in lines)
        {
            if (_folds.IsFolded(line.LineIndex)) continue;

            var gap = _gaps[line.LineIndex];
            if (gap is not null) absoluteOffsetY += gap.CountBefore * textLineHeight;

            if (absoluteOffsetY - verticalScrollBarValue >= controlHeight) yield break;
            else if (absoluteOffsetY + textLineHeight > verticalScrollBarValue) yield return new(line.LineIndex, absoluteOffsetY - verticalScrollBarValue);

            absoluteOffsetY += textLineHeight;
        }
    }

    public double GetLineOffsetY(int lineIndex, double textLineHeight)
    {
        if (_gaps.AnyItems)
        {
            lineIndex += Enumerable.Range(0, lineIndex + 1).Sum(i => _gaps[i]?.CountBefore) ?? 0;
        }
        if (_folds.AnyFoldedItems)
        {
            lineIndex -= Enumerable.Range(0, lineIndex + 1).Where(_folds.IsFolded).Count();
        }

        return lineIndex * textLineHeight;
    }

    public int GetLineIndex(double mouseY, double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount)
    {
        if (_gaps.AnyItems || _folds.AnyFoldedItems)
        {
            return
                GetLineNumbersModified(controlHeight, verticalScrollBarValue, textLineHeight, textLinesCount)
                .LastOrDefault(line => line.OffsetY < mouseY)
                .LineIndex;
        }
        else
        {
            return (int)((mouseY + verticalScrollBarValue) / textLineHeight);
        }
    }
}
