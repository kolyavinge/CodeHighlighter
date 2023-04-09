using System.Linq;
using CodeHighlighter.Ancillary;
using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering;

public interface IActivatedLineFoldsRendering
{
    void RenderActivatedFoldLines(object platformColor);
}

public class ActivatedLineFoldsRendering : IActivatedLineFoldsRendering
{
    private readonly ICodeTextBoxModel _model;
    private readonly ICodeTextBoxRenderingContext _context;
    private readonly ILineFoldsNumberGenerator _lineNumberGenerator;

    public ActivatedLineFoldsRendering(
        ICodeTextBoxModel model,
        ICodeTextBoxRenderingContext context,
        ILineFoldsNumberGenerator lineNumberGenerator)
    {
        _model = model;
        _context = context;
        _lineNumberGenerator = lineNumberGenerator;
    }

    public void RenderActivatedFoldLines(object platformColor)
    {
        var folds = _lineNumberGenerator
            .GetFolds(_model.Viewport.ActualHeight, _model.Viewport.VerticalScrollBarValue, _model.TextMeasures.LineHeight, _model.TextLines.Count)
            .Where(x => x.IsActive);

        foreach (var fold in folds)
        {
            _context.DrawRectangle(platformColor, new(0, fold.OffsetY, _model.Viewport.ActualWidth, _model.TextMeasures.LineHeight));
        }
    }
}
