using System.Collections.Generic;
using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering;

public interface ILineFoldsRendering
{
    void Render(object platformColor, double textLineHeight, IEnumerable<ILineFoldingPanelModel.LineFoldWithOffsetY> folds);
}

internal class LineFoldsRendering : ILineFoldsRendering
{
    private const double _delta = 4.0;

    private readonly ILineFoldingPanelRenderingContext _context;

    public LineFoldsRendering(ILineFoldingPanelRenderingContext context)
    {
        _context = context;
    }

    public void Render(object platformColor, double textLineHeight, IEnumerable<ILineFoldingPanelModel.LineFoldWithOffsetY> folds)
    {
        foreach (var fold in folds)
        {
            if (fold.IsActive)
            {
                _context.DrawPolygon(
                    platformColor,
                    new Common.Point[]
                    {
                        new(_delta, fold.OffsetY + _delta),
                        new(textLineHeight - _delta, fold.OffsetY + textLineHeight / 2.0),
                        new(_delta, fold.OffsetY + textLineHeight - _delta)
                    },
                    1.5);
            }
            else
            {
                _context.DrawPolygon(
                    platformColor,
                    new Common.Point[]
                    {
                        new(_delta, fold.OffsetY + _delta),
                        new(textLineHeight / 2.0, fold.OffsetY + textLineHeight - _delta),
                        new(textLineHeight - _delta, fold.OffsetY + _delta)
                    },
                    1.5);
            }
        }
    }
}
