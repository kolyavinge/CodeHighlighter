using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CodeHighlighter.Model;

internal class TextSelectionRect
{
    public IEnumerable<Rect> GetCalculatedRects(IEnumerable<TextSelectionLine> selectedLines, TextMeasures textMeasures, IViewportContext viewportContext)
    {
        var selectedLinesList = selectedLines.ToList();
        if (!selectedLinesList.Any()) yield break;

        double leftColumnPos, rightColumnPos;
        TextSelectionLine line;

        for (int i = 0; i < selectedLinesList.Count - 1; i++)
        {
            line = selectedLinesList[i];
            leftColumnPos = line.LeftColumnIndex * textMeasures.LetterWidth - viewportContext.HorizontalScrollBarValue;
            rightColumnPos = line.RightColumnIndex * textMeasures.LetterWidth - viewportContext.HorizontalScrollBarValue;

            yield return new Rect(
                leftColumnPos,
                line.LineIndex * textMeasures.LineHeight - viewportContext.VerticalScrollBarValue,
                rightColumnPos - leftColumnPos + textMeasures.LetterWidth,
                textMeasures.LineHeight);
        }

        line = selectedLinesList.Last();
        leftColumnPos = line.LeftColumnIndex * textMeasures.LetterWidth - viewportContext.HorizontalScrollBarValue;
        rightColumnPos = line.RightColumnIndex * textMeasures.LetterWidth - viewportContext.HorizontalScrollBarValue;

        yield return new Rect(
            leftColumnPos,
            line.LineIndex * textMeasures.LineHeight - viewportContext.VerticalScrollBarValue,
            rightColumnPos - leftColumnPos,
            textMeasures.LineHeight);
    }
}
