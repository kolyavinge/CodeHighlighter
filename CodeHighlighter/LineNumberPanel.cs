using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CodeHighlighter;

public class LineNumberPanel : Control
{
    #region VerticalScrollBarValue
    public double VerticalScrollBarValue
    {
        get { return (double)GetValue(VerticalScrollBarValueProperty); }
        set { SetValue(VerticalScrollBarValueProperty, value); }
    }

    public static readonly DependencyProperty VerticalScrollBarValueProperty =
        DependencyProperty.Register("VerticalScrollBarValue", typeof(double), typeof(LineNumberPanel), new PropertyMetadata(0.0, VerticalScrollBarValueChangedCallback));

    private static void VerticalScrollBarValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var panel = (LineNumberPanel)d;
        if ((double)e.NewValue < 0) panel.VerticalScrollBarValue = 0.0;
        panel.InvalidateVisual();
    }
    #endregion

    #region TextLinesCount
    public int TextLinesCount
    {
        get { return (int)GetValue(TextLinesCountProperty); }
        set { SetValue(TextLinesCountProperty, value); }
    }

    public static readonly DependencyProperty TextLinesCountProperty =
        DependencyProperty.Register("TextLinesCount", typeof(int), typeof(LineNumberPanel), new PropertyMetadata(TextLinesCountChangedCallback));

    private static void TextLinesCountChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var panel = (LineNumberPanel)d;
        panel.InvalidateVisual();
    }
    #endregion

    #region TextLineHeight
    public double TextLineHeight
    {
        get { return (double)GetValue(TextLineHeightProperty); }
        set { SetValue(TextLineHeightProperty, value); }
    }

    public static readonly DependencyProperty TextLineHeightProperty =
        DependencyProperty.Register("TextLineHeight", typeof(double), typeof(LineNumberPanel), new PropertyMetadata(TextLineHeightChangedCallback));

    private static void TextLineHeightChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var panel = (LineNumberPanel)d;
        panel.InvalidateVisual();
    }
    #endregion

    protected override void OnRender(DrawingContext context)
    {
        if (TextLineHeight == 0.0) return;
        context.PushClip(new RectangleGeometry(new Rect(-1, -1, ActualWidth + 1, ActualHeight + 1)));
        context.DrawRectangle(Background ?? Brushes.White, null, new Rect(0, 0, ActualWidth, ActualHeight));
        // draw numbers
        var typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
        var startLine = (int)(VerticalScrollBarValue / TextLineHeight);
        var linesCount = (int)(ActualHeight / TextLineHeight) + 1;
        var endLine = Math.Min(startLine + linesCount, TextLinesCount);
        var offsetY = -(VerticalScrollBarValue % TextLineHeight);
        for (var lineIndex = startLine; lineIndex < endLine; lineIndex++)
        {
            var lineNumber = (lineIndex + 1).ToString();
            var formattedText = new FormattedText(lineNumber, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, FontSize, Foreground, 1.0);
            context.DrawText(formattedText, new Point(ActualWidth - formattedText.Width, offsetY));
            offsetY += TextLineHeight;
        }
        context.Pop();
    }
}
