﻿using System.Globalization;
using System.Windows;
using System.Windows.Media;
using CodeHighlighter.Rendering;

namespace CodeHighlighter;

internal class RenderingContext : IRenderingContext
{
    private DrawingContext? _context;
    private readonly CodeTextBox _codeTextBox;

    public RenderingContext(CodeTextBox codeTextBox)
    {
        _codeTextBox = codeTextBox;
    }

    public void SetContext(DrawingContext context)
    {
        _context = context;
    }

    public void DrawText(string text, Common.Point position, Common.Color? foreground)
    {
        var fontSettings = _codeTextBox.FontSettings;
        var typeface = new Typeface(fontSettings.FontFamily, fontSettings.FontStyle, fontSettings.FontWeight, fontSettings.FontStretch);

        var brush = foreground != null
            ? new SolidColorBrush(new() { R = foreground.Value.R, G = foreground.Value.G, B = foreground.Value.B, A = foreground.Value.A })
            : _codeTextBox.Foreground;

        var formattedText = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSettings.FontSize, brush, 1.0);

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
}
