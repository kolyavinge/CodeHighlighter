using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Common;

namespace CodeHighlighter.Model;

public class TextSelectionRect
{
    public IEnumerable<Rect> GetCalculatedRects(
        IEnumerable<TextSelectionLine> selectedLines,
        TextMeasures textMeasures,
        double horizontalScrollBarValue,
        double verticalScrollBarValue)
    {
        var selectedLinesList = selectedLines.ToList();
        if (!selectedLinesList.Any()) yield break;

        double leftColumnPos, rightColumnPos;
        TextSelectionLine line;

        for (int i = 0; i < selectedLinesList.Count - 1; i++)
        {
            line = selectedLinesList[i];
            leftColumnPos = line.LeftColumnIndex * textMeasures.LetterWidth - horizontalScrollBarValue;
            rightColumnPos = line.RightColumnIndex * textMeasures.LetterWidth - horizontalScrollBarValue;

            yield return new Rect(
                leftColumnPos,
                line.LineIndex * textMeasures.LineHeight - verticalScrollBarValue,
                rightColumnPos - leftColumnPos + textMeasures.LetterWidth,
                textMeasures.LineHeight);
        }

        line = selectedLinesList.Last();
        leftColumnPos = line.LeftColumnIndex * textMeasures.LetterWidth - horizontalScrollBarValue;
        rightColumnPos = line.RightColumnIndex * textMeasures.LetterWidth - horizontalScrollBarValue;

        yield return new Rect(
            leftColumnPos,
            line.LineIndex * textMeasures.LineHeight - verticalScrollBarValue,
            rightColumnPos - leftColumnPos,
            textMeasures.LineHeight);
    }
}
