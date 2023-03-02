using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CodeHighlighter.Model;
using CodeHighlighter.Rendering;

namespace CodeHighlighter;

public class LineNumberPanel : Control, ILineNumberPanel
{
    private readonly LineNumberPanelRenderingContext _context;
    private INumberRendering? _numberRendering;

    #region Model
    public ILineNumberPanelModel Model
    {
        get => (ILineNumberPanelModel)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    public static readonly DependencyProperty ModelProperty =
        DependencyProperty.Register("Model", typeof(ILineNumberPanelModel), typeof(LineNumberPanel), new PropertyMetadata(LineNumberPanelModelFactory.MakeModel(), ModelChangedCallback));

    private static void ModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var model = (ILineNumberPanelModel)e.NewValue;
        var panel = (LineNumberPanel)d;
        model.AttachLineNumberPanel(panel);
        panel._numberRendering = RenderingModelFactory.MakeNumberRendering(panel._context);
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
        get => (int)GetValue(TextLinesCountProperty);
        set => SetValue(TextLinesCountProperty, value);
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
        get => (double)GetValue(TextLineHeightProperty);
        set => SetValue(TextLineHeightProperty, value);
    }

    public static readonly DependencyProperty TextLineHeightProperty =
        DependencyProperty.Register("TextLineHeight", typeof(double), typeof(LineNumberPanel), new PropertyMetadata(1.0, TextLineHeightChangedCallback));

    private static void TextLineHeightChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var panel = (LineNumberPanel)d;
        panel.InvalidateVisual();
    }
    #endregion

    public LineNumberPanel()
    {
        _context = new LineNumberPanelRenderingContext(this);
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
