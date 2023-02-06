using CodeHighlighter.Common;
using CodeHighlighter.Model;
using static CodeHighlighter.Model.IBracketsHighlighter;

namespace CodeHighlighter.Rendering;

public interface IHighlightBracketsRendering
{
    void Render(object platformHighlightingBrush, object platformNoPairBrush);
}

internal class HighlightBracketsRendering : IHighlightBracketsRendering
{
    private readonly ICodeTextBoxModel _model;
    private readonly IRenderingContext _renderingContext;
    private readonly IExtendedLineNumberGenerator _lineNumberGenerator;

    public HighlightBracketsRendering(
        ICodeTextBoxModel model, IRenderingContext renderingContext, IExtendedLineNumberGenerator lineNumberGenerator)
    {
        _model = model;
        _renderingContext = renderingContext;
        _lineNumberGenerator = lineNumberGenerator;
    }

    public void Render(object platformHighlightingBrush, object platformNoPairBrush)
    {
        var brackets = _model.BracketsHighlighter.GetHighlightedBrackets(_model.CursorPosition);
        if (brackets.Kind == HighlightKind.NoHighlight) return;
        if (brackets.Kind == HighlightKind.Highlighted)
        {
            _renderingContext.DrawRectangle(platformHighlightingBrush, GetBracketRect(_model, brackets.Open));
            _renderingContext.DrawRectangle(platformHighlightingBrush, GetBracketRect(_model, brackets.Close));
        }
        else // NoPair
        {
            _renderingContext.DrawRectangle(platformNoPairBrush, GetBracketRect(_model, brackets.Open));
        }
    }

    private Rect GetBracketRect(ICodeTextBoxModel model, BracketPosition bracketPosition)
    {
        var offsetY = _lineNumberGenerator.GetLineOffsetY(bracketPosition.LineIndex, model.TextMeasures.LineHeight);

        return new(
            bracketPosition.ColumnIndex * model.TextMeasures.LetterWidth - model.Viewport.HorizontalScrollBarValue,
            offsetY - model.Viewport.VerticalScrollBarValue,
            model.TextMeasures.LetterWidth,
            model.TextMeasures.LineHeight);
    }
}
