using System.Collections.Generic;

namespace CodeHighlighter.Model;

public readonly struct LineNumber
{
    public readonly int LineIndex;
    public readonly double OffsetY;

    public LineNumber(int lineIndex, double offsetY)
    {
        LineIndex = lineIndex;
        OffsetY = offsetY;
    }
}

public interface ILineNumberGenerator
{
    IEnumerable<LineNumber> GetLineNumbers(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount);
}

internal class LineNumberGenerator : ILineNumberGenerator
{
    public IEnumerable<LineNumber> GetLineNumbers(double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount)
    {
        var startLine = (int)(verticalScrollBarValue / textLineHeight);
        var linesCount = (int)Math.Ceiling(controlHeight / textLineHeight);
        if (verticalScrollBarValue % textLineHeight != 0) linesCount++;
        var endLine = Math.Min(startLine + linesCount, textLinesCount);
        var offsetY = -(verticalScrollBarValue % textLineHeight);
        for (var lineIndex = startLine; lineIndex < endLine; lineIndex++)
        {
            yield return new(lineIndex, offsetY);
            offsetY += textLineHeight;
        }
    }
}
