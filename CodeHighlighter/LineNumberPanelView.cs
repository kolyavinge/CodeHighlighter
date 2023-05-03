using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CodeHighlighter.Model;
using CodeHighlighter.Rendering;

namespace CodeHighlighter;

public class LineNumberPanelView : Control, ILineNumberPanelView
{
    private readonly RenderingContext _context;
    private readonly INumberRendering? _numberRendering;

    #region Model
    public ILineNumberPanel Model
    {
        get => (ILineNumberPanel)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    public static readonly DependencyProperty ModelProperty =
        DependencyProperty.Register("Model", typeof(ILineNumberPanel), typeof(LineNumberPanelView), new PropertyMetadata(LineNumberPanelFactory.MakeModel(), ModelChangedCallback));

    private static void ModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var model = (ILineNumberPanel)e.NewValue;
        var panel = (LineNumberPanelView)d;
        model.AttachLineNumberPanel(panel);
        panel.InvalidateVisual();
    }
    #endregion

    #region VerticalScrollBarValue
    public double VerticalScrollBarValue
    {
        get => (double)GetValue(VerticalScrollBarValueProperty);
        set => SetValue(VerticalScrollBarValueProperty, value);
    }

    public static readonly DependencyProperty VerticalScrollBarValueProperty =
        DependencyProperty.Register("VerticalScrollBarValue", typeof(double), typeof(LineNumberPanelView), new PropertyMetadata(0.0, VerticalScrollBarValueChangedCallback));

    private static void VerticalScrollBarValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var panel = (LineNumberPanelView)d;
        if ((double)e.NewValue < 0) panel.VerticalScrollBarValue = 0.0;
        panel.InvalidateVisual();
    }
    #endregion

    #region TextLinesCount
    public int TextLinesCount
    {
        get => (int)GetValue(TextLinesCountProperty);
        set => SetValue(TextLinesCountProperty, value);
    }

    public static readonly DependencyProperty TextLinesCountProperty =
        DependencyProperty.Register("TextLinesCount", typeof(int), typeof(LineNumberPanelView), new PropertyMetadata(TextLinesCountChangedCallback));

    private static void TextLinesCountChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var panel = (LineNumberPanelView)d;
        panel.InvalidateVisual();
    }
    #endregion

    #region TextLineHeight
    public double TextLineHeight
    {
        get => (double)GetValue(TextLineHeightProperty);
        set => SetValue(TextLineHeightProperty, value);
    }

    public static readonly DependencyProperty TextLineHeightProperty =
        DependencyProperty.Register("TextLineHeight", typeof(double), typeof(LineNumberPanelView), new PropertyMetadata(1.0, TextLineHeightChangedCallback));

    private static void TextLineHeightChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var panel = (LineNumberPanelView)d;
        panel.InvalidateVisual();
    }
    #endregion

    public LineNumberPanelView()
    {
        _context = new RenderingContext(this);
        _numberRendering = RenderingModelFactory.MakeNumberRendering(_context);
    }

    protected override void OnRender(DrawingContext context)
    {
        if (_numberRendering == null) return;
        _context.SetContext(context);
        context.PushClip(new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight)));
        context.DrawRectangle(Background ?? Brushes.White, null, new Rect(0, 0, ActualWidth, ActualHeight));
        var lines = Model.GetLines(ActualHeight, VerticalScrollBarValue, TextLineHeight, TextLinesCount).ToList();
        _numberRendering.Render(lines, ActualWidth);
        context.Pop();
        Width = lines.LastOrDefault().LineIndex.ToString().Length * GetLetterWidth();
    }

    private double GetLetterWidth()
    {
        var typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
        return new FormattedText("A", CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, FontSize, Foreground, 1.0).Width;
    }
}
