using System.Windows;
using System.Windows.Media;
using CodeHighlighter.Model;
using static CodeHighlighter.Model.BracketsHighlighter;

namespace CodeHighlighter.Rendering;

internal class HighlightBracketsRenderLogic
{
    public void DrawHighlightedBrackets(CodeTextBoxModel model, DrawingContext context, Brush highlightingBrush, Brush noPairBrush)
    {
        var brackets = model.BracketsHighlighter.GetHighlightedBrackets(model.Text, model.TextCursor.Position);
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
        return new(
            bracketPosition.ColumnIndex * model.TextMeasures.LetterWidth - model.Viewport.HorizontalScrollBarValue,
            bracketPosition.LineIndex * model.TextMeasures.LineHeight - model.Viewport.VerticalScrollBarValue,
            model.TextMeasures.LetterWidth,
            model.TextMeasures.LineHeight);
    }
}
