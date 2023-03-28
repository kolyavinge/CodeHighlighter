using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CodeHighlighter.Rendering;

namespace CodeHighlighter;

internal class RenderingContext : ICodeTextBoxRenderingContext, ILineNumberPanelRenderingContext, ILineFoldingPanelRenderingContext
{
    private readonly Control _control;
    private readonly Dictionary<Common.Color, SolidColorBrush> _textBrushes;
    private Typeface? _typeface;
    private DrawingContext? _context;

    public RenderingContext(Control control)
    {
        _control = control;
        _textBrushes = new Dictionary<Common.Color, SolidColorBrush>();
        _typeface = new Typeface(_control.FontFamily, _control.FontStyle, _control.FontWeight, _control.FontStretch);
    }

    public RenderingContext(CodeTextBox codeTextBox) : this((Control)codeTextBox)
    {
        codeTextBox.FontSettingsChanged += (s, e) =>
        {
            _typeface = new Typeface(_control.FontFamily, _control.FontStyle, _control.FontWeight, _control.FontStretch);
        };
    }

    public void SetContext(DrawingContext context)
    {
        _context = context;
    }

    public void SetColorsForText(IEnumerable<Common.Color> colors)
    {
        _textBrushes.Clear();
        foreach (var color in colors.Distinct())
        {
            _textBrushes.Add(color, new SolidColorBrush(new() { R = color.R, G = color.G, B = color.B, A = color.A }));
        }
    }

    public void DrawText(string text, Common.Point position, Common.Color? foreground)
    {
        var brush = foreground != null ? _textBrushes[foreground.Value] : _control.Foreground;
        var formattedText = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, _typeface, _control.FontSize, brush, 1.0);
        _context!.DrawText(formattedText, new(position.X, position.Y));
    }

    public void DrawNumber(double offsetY, int number, TextAlign align)
    {
        var typeface = new Typeface(_control.FontFamily, _control.FontStyle, _control.FontWeight, _control.FontStretch);
        var formattedText = new FormattedText(number.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, _control.FontSize, _control.Foreground, 1.0);
        if (align == TextAlign.Left)
        {
            _context!.DrawText(formattedText, new(0, offsetY));
        }
        else if (align == TextAlign.Right)
        {
            _context!.DrawText(formattedText, new(_control.ActualWidth - formattedText.Width, offsetY));
        }
    }

    public void DrawRectangle(object platformColor, Common.Rect rect)
    {
        _context!.DrawRectangle((Brush)platformColor, null, new(rect.X, rect.Y, rect.Width, rect.Height));
    }

    public void DrawRectangle(Common.Color color, Common.Rect rect)
    {
        var brush = new SolidColorBrush(new() { R = color.R, G = color.G, B = color.B, A = color.A });
        _context!.DrawRectangle(brush, null, new(rect.X, rect.Y, rect.Width, rect.Height));
    }

    public void DrawPolygon(object platformColor, IEnumerable<Common.Point> points, double thickness)
    {
        var pointsArray = points.ToArray();
        var drawingPen = new Pen((Brush)platformColor, thickness);
        for (int i = 1; i < pointsArray.Length; i++)
        {
            var p1 = new Point(pointsArray[i - 1].X, pointsArray[i - 1].Y);
            var p2 = new Point(pointsArray[i].X, pointsArray[i].Y);
            _context!.DrawLine(drawingPen, p1, p2);
        }
    }
}
