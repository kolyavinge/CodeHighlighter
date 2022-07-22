using System.Windows;
using System.Windows.Media;
using CodeHighlighter.Model;
using static CodeHighlighter.Model.BracketsHighlighter;

namespace CodeHighlighter.Rendering;

internal class HighlightBracketsRenderLogic
{
    public void DrawHighlightedBrackets(CodeTextBoxModel model, DrawingContext context, Brush highlightingBrush, Brush noPairBrush)
    {
        var brackets = model.BracketsHighlighter.GetHighlightedBrackets();
        if (brackets.Kind == HighlightKind.NoHighlight) return;
        if (brackets.Kind == HighlightKind.Highlighted)
        {
            context.DrawRectangle(highlightingBrush, null, GetBracketRect(model, brackets.Open));
            context.DrawRectangle(highlightingBrush, null, GetBracketRect(model, brackets.Close));
        }
        else // NoPair
        {
            context.DrawRectangle(noPairBrush, null, GetBracketRect(model, brackets.Open));
        }
    }

    private Rect GetBracketRect(CodeTextBoxModel model, BracketPosition bracketPosition)
    {
        var textMeasures = model.TextMeasures;
        var viewportContext = model.ViewportContext;

        return new(
            bracketPosition.ColumnIndex * textMeasures.LetterWidth - viewportContext.HorizontalScrollBarValue,
            bracketPosition.LineIndex * textMeasures.LineHeight - viewportContext.VerticalScrollBarValue,
            textMeasures.LetterWidth,
            textMeasures.LineHeight);
    }
}
