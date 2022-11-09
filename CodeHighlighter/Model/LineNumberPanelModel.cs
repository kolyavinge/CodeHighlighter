using System.Collections.Generic;

namespace CodeHighlighter.Model;

public class LineNumberPanelModel
{
    public LineNumberPanelGapCollection GapCollection { get; } = new();

    public IEnumerable<LineNumber> GetLines(
        double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount)
    {
        if (GapCollection.AnyItems)
        {
            return GetLineNumbersModified(controlHeight, verticalScrollBarValue, textLineHeight, textLinesCount);
        }
        else
        {
            return LineNumber.GetLineNumbers(controlHeight, verticalScrollBarValue, textLineHeight, textLinesCount);
        }
    }

    private IEnumerable<LineNumber> GetLineNumbersModified(
        double controlHeight, double verticalScrollBarValue, double textLineHeight, int textLinesCount)
    {
        var gapYOffset = 0.0;
        foreach (var line in LineNumber.GetLineNumbers(controlHeight, verticalScrollBarValue, textLineHeight, textLinesCount))
        {
            var gap = GapCollection[line.Index];
            if (gap != null)
            {
                gapYOffset += gap.Value.CountBeforeLine * textLineHeight;
            }
            var newLine = new LineNumber(line.Index, line.OffsetY + gapYOffset);
            if (newLine.OffsetY > controlHeight) yield break;
            yield return newLine;
        }
    }
}
