using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using CodeHighlighter.Model;
using CodeHighlighter.Utils;

namespace CodeHighlighter.Rendering
{
    internal class TextSelectionRenderLogic
    {
        public void DrawSelectedLines(
            DrawingContext context, Brush brush, IEnumerable<TextSelectionLine> selectedLines, ITextMeasures textMeasures, IViewportContext viewportContext)
        {
            GetCalculatedRects(selectedLines, textMeasures, viewportContext).Each(rect => context.DrawRectangle(brush, null, rect));
        }

        public IEnumerable<Rect> GetCalculatedRects(IEnumerable<TextSelectionLine> selectedLines, ITextMeasures textMeasures, IViewportContext viewportContext)
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
}
