using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Common;

namespace CodeHighlighter.Model;

public interface ITextSelectionRect
{
    IEnumerable<Rect> GetCalculatedRects(
        IEnumerable<TextSelectionLine> selectedLines,
        ITextMeasures textMeasures,
        double controlHeight,
        double horizontalScrollBarValue,
        double verticalScrollBarValue);
}

internal class TextSelectionRect : ITextSelectionRect
{
    private readonly IExtendedLineNumberGenerator _lineNumberGenerator;

    public TextSelectionRect(IExtendedLineNumberGenerator lineNumberGenerator)
    {
        _lineNumberGenerator = lineNumberGenerator;
    }

    public IEnumerable<Rect> GetCalculatedRects(
        IEnumerable<TextSelectionLine> selectedLines,
        ITextMeasures textMeasures,
        double controlHeight,
        double horizontalScrollBarValue,
        double verticalScrollBarValue)
    {
        var selectedLinesList = selectedLines.ToList();
        if (!selectedLinesList.Any()) yield break;

        var lineNumbers = _lineNumberGenerator.GetLineNumbers(
            controlHeight, verticalScrollBarValue, textMeasures.LineHeight, selectedLinesList.Last().LineIndex + 1);

        var selectedLinesInViewport =
            (from l in selectedLinesList
             join n in lineNumbers on l.LineIndex equals n.LineIndex
             select (l, n)).ToList();

        if (!selectedLinesInViewport.Any()) yield break;

        double leftColumnPos, rightColumnPos;

        for (int i = 0; i < selectedLinesInViewport.Count - 1; i++)
        {
            var (line, number) = selectedLinesInViewport[i];
            leftColumnPos = line.LeftColumnIndex * textMeasures.LetterWidth - horizontalScrollBarValue;
            rightColumnPos = line.RightColumnIndex * textMeasures.LetterWidth - horizontalScrollBarValue;

            yield return new Rect(
                leftColumnPos,
                number.OffsetY,
                rightColumnPos - leftColumnPos + textMeasures.LetterWidth,
                textMeasures.LineHeight);
        }

        var (lastLine, lastNumber) = selectedLinesInViewport.Last();
        leftColumnPos = lastLine.LeftColumnIndex * textMeasures.LetterWidth - horizontalScrollBarValue;
        rightColumnPos = lastLine.RightColumnIndex * textMeasures.LetterWidth - horizontalScrollBarValue;

        yield return new Rect(
            leftColumnPos,
            lastNumber.OffsetY,
            rightColumnPos - leftColumnPos,
            textMeasures.LineHeight);
    }
}
