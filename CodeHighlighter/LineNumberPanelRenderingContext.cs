using System.Globalization;
using System.Windows;
using System.Windows.Media;
using CodeHighlighter.Rendering;

namespace CodeHighlighter;

internal class LineNumberPanelRenderingContext : ILineNumberPanelRenderingContext
{
    private readonly LineNumberPanel _lineNumberPanel;
    private DrawingContext? _context;

    public LineNumberPanelRenderingContext(LineNumberPanel lineNumberPanel)
    {
        _lineNumberPanel = lineNumberPanel;
    }

    public void SetContext(DrawingContext context)
    {
        _context = context;
    }

    public void Render(double lineOffsetY, string lineNumber, double controlWidth, Common.Point value, TextAlign align)
    {
        var typeface = new Typeface(_lineNumberPanel.FontFamily, _lineNumberPanel.FontStyle, _lineNumberPanel.FontWeight, _lineNumberPanel.FontStretch);
        var formattedText = new FormattedText(lineNumber, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, _lineNumberPanel.FontSize, _lineNumberPanel.Foreground, 1.0);
        if (align == TextAlign.Left)
        {
            _context!.DrawText(formattedText, new(0, lineOffsetY));
        }
        else if (align == TextAlign.Right)
        {
            _context!.DrawText(formattedText, new(_lineNumberPanel.ActualWidth - formattedText.Width, lineOffsetY));
        }
    }
}
