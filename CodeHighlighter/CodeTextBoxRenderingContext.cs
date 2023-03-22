using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using CodeHighlighter.Rendering;

namespace CodeHighlighter;

internal class CodeTextBoxRenderingContext : ICodeTextBoxRenderingContext
{
    private readonly CodeTextBox _codeTextBox;
    private readonly Dictionary<Common.Color, SolidColorBrush> _textBrushes;
    private Typeface? _typeface;
    private DrawingContext? _context;

    public CodeTextBoxRenderingContext(CodeTextBox codeTextBox)
    {
        _codeTextBox = codeTextBox;
        _textBrushes = new Dictionary<Common.Color, SolidColorBrush>();
        _typeface = new Typeface(_codeTextBox.FontSettings.FontFamily, _codeTextBox.FontSettings.FontStyle, _codeTextBox.FontSettings.FontWeight, _codeTextBox.FontSettings.FontStretch);
        _codeTextBox.FontSettingsChanged += (s, e) =>
        {
            _typeface = new Typeface(
                _codeTextBox.FontSettings.FontFamily, _codeTextBox.FontSettings.FontStyle, _codeTextBox.FontSettings.FontWeight, _codeTextBox.FontSettings.FontStretch);
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
        var brush = foreground != null ? _textBrushes[foreground.Value] : _codeTextBox.Foreground;
        var formattedText = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, _typeface, _codeTextBox.FontSettings.FontSize, brush, 1.0);
        _context!.DrawText(formattedText, new(position.X, position.Y));
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

    public void DrawPolygon(object platformColor, IEnumerable<Common.Point> points)
    {
        var pointsArray = points.ToArray();
        var drawingPen = new Pen((Brush)platformColor, 1);
        for (int i = 1; i < pointsArray.Length; i++)
        {
            var p1 = new Point(pointsArray[i - 1].X, pointsArray[i - 1].Y);
            var p2 = new Point(pointsArray[i].X, pointsArray[i].Y);
            _context!.DrawLine(drawingPen, p1, p2);
        }
    }
}
