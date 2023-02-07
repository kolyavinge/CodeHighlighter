using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model;

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

    public ExtendedLineNumberGenerator(
        ILineNumberGenerator lineNumberGenerator,
        ILineGapCollection gaps)
    {
        _lineNumberGenerator = lineNumberGenerator;
        _gaps = gaps;
    }

    public IEnumerable<LineNumber> GetLineNumbers(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount)
    {
        if (_gaps.AnyItems)
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
        foreach (var line in _lineNumberGenerator.GetLineNumbers(controlHeight + verticalScrollBarValue, 0, textLineHeight, textLinesCount))
        {
            var gap = _gaps[line.LineIndex];
            if (gap != null) absoluteOffsetY += gap.CountBefore * textLineHeight;
            if (absoluteOffsetY - verticalScrollBarValue >= controlHeight) yield break;
            else if (absoluteOffsetY + textLineHeight > verticalScrollBarValue) yield return new(line.LineIndex, absoluteOffsetY - verticalScrollBarValue);
            absoluteOffsetY += textLineHeight;
        }
    }

    public double GetLineOffsetY(int lineIndex, double textLineHeight)
    {
        if (_gaps.AnyItems)
        {
            lineIndex += (int)(Enumerable.Range(0, lineIndex + 1).Sum(i => _gaps[i]?.CountBefore) ?? 0);
        }

        return lineIndex * textLineHeight;
    }

    public int GetLineIndex(double mouseY, double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount)
    {
        if (_gaps.AnyItems)
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
