using System.Collections.Generic;
using CodeHighlighter.Ancillary;

namespace CodeHighlighter.Rendering;

public interface ILineFoldsRendering
{
    void Render(object platformColor, double textLineHeight, IEnumerable<LineFoldWithOffsetY> folds);
}

internal class LineFoldsRendering : ILineFoldsRendering
{
    private const double _delta = 4.0;

    private readonly ILineFoldingPanelRenderingContext _context;

    public LineFoldsRendering(ILineFoldingPanelRenderingContext context)
    {
        _context = context;
    }

    public void Render(object platformColor, double textLineHeight, IEnumerable<LineFoldWithOffsetY> folds)
    {
        foreach (var fold in folds)
        {
            if (fold.IsActive)
            {
                var points = new Common.Point[]
                {
                    new(_delta, fold.OffsetY + _delta),
                    new(textLineHeight - _delta, fold.OffsetY + textLineHeight / 2.0),
                    new(_delta, fold.OffsetY + textLineHeight - _delta)
                };

                _context.DrawPolygon(platformColor, points, 1.5);
            }
            else
            {
                var points = new Common.Point[]
                {
                    new(_delta, fold.OffsetY + _delta),
                    new(textLineHeight / 2.0, fold.OffsetY + textLineHeight - _delta),
                    new(textLineHeight - _delta, fold.OffsetY + _delta)
                };

                _context.DrawPolygon(platformColor, points, 1.5);
            }
        }
    }
}
