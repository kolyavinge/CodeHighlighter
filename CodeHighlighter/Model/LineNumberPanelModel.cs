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
        var absoluteOffsetY = 0.0;
        foreach (var line in LineNumber.GetLineNumbers(controlHeight + verticalScrollBarValue, 0, textLineHeight, textLinesCount))
        {
            var gap = GapCollection[line.Index];
            if (gap != null) absoluteOffsetY += gap.CountBefore * textLineHeight;
            if (absoluteOffsetY - verticalScrollBarValue >= controlHeight) yield break;
            else if (absoluteOffsetY + textLineHeight > verticalScrollBarValue) yield return new(line.Index, absoluteOffsetY - verticalScrollBarValue);
            absoluteOffsetY += textLineHeight;
        }
    }
}
