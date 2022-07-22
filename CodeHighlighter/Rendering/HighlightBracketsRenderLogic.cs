using System.Windows;
using System.Windows.Media;
using CodeHighlighter.Model;
using static CodeHighlighter.Model.BracketsHighlighter;

namespace CodeHighlighter.Rendering;

internal interface IHighlightBracketsRenderLogic
{
    void DrawHighlightedBrackets(DrawingContext context, Brush highlightingBrush, Brush noPairBrush);
}

internal class HighlightBracketsRenderLogic : IHighlightBracketsRenderLogic
{
    private readonly BracketsHighlighter _bracketsHighlighter;
    private readonly TextMeasures _textMeasures;
    private readonly IViewportContext _viewportContext;

    public HighlightBracketsRenderLogic(BracketsHighlighter bracketsHighlighter, TextMeasures textMeasures, IViewportContext viewportContext)
    {
        _bracketsHighlighter = bracketsHighlighter;
        _textMeasures = textMeasures;
        _viewportContext = viewportContext;
    }

    public void DrawHighlightedBrackets(DrawingContext context, Brush highlightingBrush, Brush noPairBrush)
    {
        var brackets = _bracketsHighlighter.GetHighlightedBrackets();
        if (brackets.Kind == HighlightKind.NoHighlight) return;
        if (brackets.Kind == HighlightKind.Highlighted)
        {
            context.DrawRectangle(highlightingBrush, null, GetBracketRect(brackets.Open));
            context.DrawRectangle(highlightingBrush, null, GetBracketRect(brackets.Close));
        }
        else // NoPair
        {
            context.DrawRectangle(noPairBrush, null, GetBracketRect(brackets.Open));
        }
    }

    private Rect GetBracketRect(BracketPosition bracketPosition)
    {
        return new(
            bracketPosition.ColumnIndex * _textMeasures.LetterWidth - _viewportContext.HorizontalScrollBarValue,
            bracketPosition.LineIndex * _textMeasures.LineHeight - _viewportContext.VerticalScrollBarValue,
            _textMeasures.LetterWidth,
            _textMeasures.LineHeight);
    }
}

internal class DummyHighlightBracketsRenderLogic : IHighlightBracketsRenderLogic
{
    public void DrawHighlightedBrackets(DrawingContext context, Brush highlightingBrush, Brush noPairBrush)
    {
    }
}
