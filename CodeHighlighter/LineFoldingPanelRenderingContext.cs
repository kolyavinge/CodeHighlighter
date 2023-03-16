using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using CodeHighlighter.Rendering;

namespace CodeHighlighter;

internal class LineFoldingPanelRenderingContext : ILineFoldingPanelRenderingContext
{
    private DrawingContext? _context;
    private readonly LineFoldingPanel _panel;

    public LineFoldingPanelRenderingContext(LineFoldingPanel panel)
    {
        _panel = panel;
    }

    public void SetContext(DrawingContext context)
    {
        _context = context;
    }

    public void DrawPolygon(object platformColor, IEnumerable<Common.Point> points)
    {
        var pointsArray = points.ToArray();
        var drawingPen = new Pen((Brush)platformColor, 1.5);
        for (int i = 1; i < pointsArray.Length; i++)
        {
            var p1 = new Point(pointsArray[i - 1].X, pointsArray[i - 1].Y);
            var p2 = new Point(pointsArray[i].X, pointsArray[i].Y);
            _context!.DrawLine(drawingPen, p1, p2);
        }
    }
}
