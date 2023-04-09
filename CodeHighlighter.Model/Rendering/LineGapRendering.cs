using System.Collections.Generic;
using CodeHighlighter.Common;
using CodeHighlighter.Core;
using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering;

public interface ILineGapRendering
{
    void Render(object platformBrush);
}

internal class LineGapRendering : ILineGapRendering
{
    private readonly ICodeTextBoxModel _model;
    private readonly ICodeTextBoxRenderingContext _renderingContext;
    private readonly IExtendedLineNumberGenerator _lineNumberGenerator;

    public LineGapRendering(
        ICodeTextBoxModel model,
        ICodeTextBoxRenderingContext renderingContext,
        IExtendedLineNumberGenerator lineNumberGenerator)
    {
        _model = model;
        _renderingContext = renderingContext;
        _lineNumberGenerator = lineNumberGenerator;
    }

    public void Render(object platformColor)
    {
        if (!_model.Gaps.AnyItems) return;
        var textMeasures = _model.TextMeasures;
        var viewport = _model.Viewport;
        foreach (var line in _lineNumberGenerator.GetLineNumbers(viewport.ActualHeight, viewport.VerticalScrollBarValue, textMeasures.LineHeight, _model.TextLines.Count))
        {
            var gap = _model.Gaps[line.LineIndex];
            if (gap == null) continue;
            for (int i = 1; i <= gap.CountBefore; i++)
            {
                var y = line.OffsetY - i * textMeasures.LineHeight;
                _renderingContext.DrawPolygon(platformColor, GetPolygon1(y), 1.0);
                _renderingContext.DrawPolygon(platformColor, GetPolygon2(y), 1.0);
            }
        }
    }

    private IEnumerable<Point> GetPolygon1(double y)
    {
        yield return new Point(0, y);
        for (double x = 0; x < _model.Viewport.ActualWidth;)
        {
            x += _model.TextMeasures.LineHeight;
            yield return new Point(x, y + _model.TextMeasures.LineHeight);

            x += _model.TextMeasures.LineHeight;
            yield return new Point(x, y);
        }
    }

    private IEnumerable<Point> GetPolygon2(double y)
    {
        yield return new Point(0, y + _model.TextMeasures.LineHeight);
        for (double x = 0; x < _model.Viewport.ActualWidth;)
        {
            x += _model.TextMeasures.LineHeight;
            yield return new Point(x, y);

            x += _model.TextMeasures.LineHeight;
            yield return new Point(x, y + _model.TextMeasures.LineHeight);
        }
    }
}
