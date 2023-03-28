using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CodeHighlighter.Controllers;
using CodeHighlighter.Model;
using CodeHighlighter.Rendering;

namespace CodeHighlighter;

public class LineFoldingPanel : Control, ILineFoldingPanel
{
    private readonly RenderingContext _context;
    private ILineFoldsRendering? _lineFoldsRendering;
    private ILineFoldingPanelMouseController? _mouseController;

    #region Model
    public ILineFoldingPanelModel Model
    {
        get => (ILineFoldingPanelModel)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    public static readonly DependencyProperty ModelProperty =
        DependencyProperty.Register("Model", typeof(ILineFoldingPanelModel), typeof(LineFoldingPanel), new PropertyMetadata(ModelChangedCallback));

    private static void ModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var model = (ILineFoldingPanelModel)e.NewValue;
        var panel = (LineFoldingPanel)d;
        model.AttachLineFoldingPanel(panel);
        panel._lineFoldsRendering = RenderingModelFactory.MakeLineFoldsRendering(panel._context);
        panel._mouseController = ControllerFactory.MakeMouseController(panel, model);
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
        DependencyProperty.Register("VerticalScrollBarValue", typeof(double), typeof(LineFoldingPanel), new PropertyMetadata(0.0, VerticalScrollBarValueChangedCallback));

    private static void VerticalScrollBarValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var panel = (LineFoldingPanel)d;
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
        DependencyProperty.Register("TextLinesCount", typeof(int), typeof(LineFoldingPanel), new PropertyMetadata(TextLinesCountChangedCallback));

    private static void TextLinesCountChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var panel = (LineFoldingPanel)d;
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
        DependencyProperty.Register("TextLineHeight", typeof(double), typeof(LineFoldingPanel), new PropertyMetadata(1.0, TextLineHeightChangedCallback));

    private static void TextLineHeightChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var panel = (LineFoldingPanel)d;
        panel.InvalidateVisual();
    }
    #endregion

    public LineFoldingPanel()
    {
        _context = new RenderingContext(this);
    }

    protected override void OnRender(DrawingContext context)
    {
        if (_lineFoldsRendering == null) return;
        _context.SetContext(context);
        context.PushClip(new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight)));
        context.DrawRectangle(Background ?? Brushes.White, null, new Rect(0, 0, ActualWidth, ActualHeight));
        var folds = Model.GetFolds(ActualHeight, VerticalScrollBarValue, TextLineHeight, TextLinesCount).ToList();
        _lineFoldsRendering.Render(Foreground, TextLineHeight, folds);
        context.Pop();
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        if (_mouseController == null) return;
        var positionInControl = e.GetPosition(this);
        _mouseController.LeftButtonDown(new(positionInControl.X, positionInControl.Y));
    }
}
