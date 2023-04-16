using CodeHighlighter.Ancillary;
using CodeHighlighter.Common;
using CodeHighlighter.Core;
using CodeHighlighter.Model;
using static CodeHighlighter.Ancillary.IBracketsHighlighter;

namespace CodeHighlighter.Rendering;

public interface IHighlightBracketsRendering
{
    void Render(object platformHighlightingBrush, object platformNoPairBrush);
}

internal class HighlightBracketsRendering : IHighlightBracketsRendering
{
    private readonly ICodeTextBoxModel _model;
    private readonly ICodeTextBoxRenderingContext _renderingContext;
    private readonly IExtendedLineNumberGenerator _lineNumberGenerator;
    private readonly ILineFolds _folds;

    public HighlightBracketsRendering(
        ICodeTextBoxModel model,
        ICodeTextBoxRenderingContext renderingContext,
        IExtendedLineNumberGenerator lineNumberGenerator,
        ILineFolds folds)
    {
        _model = model;
        _renderingContext = renderingContext;
        _lineNumberGenerator = lineNumberGenerator;
        _folds = folds;
    }

    public void Render(object platformHighlightingBrush, object platformNoPairBrush)
    {
        var brackets = _model.BracketsHighlighter.GetHighlightedBrackets(_model.CursorPosition);
        if (brackets.Kind == HighlightKind.NoHighlight) return;
        if (_folds.IsFolded(brackets.Open.LineIndex) || _folds.IsFolded(brackets.Close.LineIndex)) return;
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
