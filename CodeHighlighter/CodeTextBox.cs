using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CodeHighlighter.Controllers;
using CodeHighlighter.Model;
using CodeHighlighter.Rendering;

namespace CodeHighlighter;

public class CodeTextBox : Control, ICodeTextBox, INotifyPropertyChanged
{
    private readonly TextRenderLogic _textRenderLogic;
    private readonly TextSelectionRenderLogic _textSelectionRenderLogic;
    private readonly CursorRenderLogic _cursorRenderLogic;
    private readonly KeyboardController _keyboardController;
    private readonly MouseController _mouseController;
    private readonly HighlightBracketsRenderLogic _highlightBracketsRenderLogic;
    private readonly LineRenderLogic _lineRenderLogic;
    private readonly MouseSettings _mouseSettings;
    private readonly FontSettings _fontSettings;

    public event EventHandler<FontSettingsChangedEventArgs>? FontSettingsChanged;
    public event EventHandler? ViewportSizeChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    #region Model
    public ICodeTextBoxModel? Model
    {
        get => (ICodeTextBoxModel?)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    public static readonly DependencyProperty ModelProperty =
        DependencyProperty.Register("Model", typeof(ICodeTextBoxModel), typeof(CodeTextBox), new PropertyMetadata(ModelPropertyChangedCallback));

    private static void ModelPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var model = (ICodeTextBoxModel)e.NewValue ?? throw new ArgumentNullException(nameof(Model));
        var codeTextBox = (CodeTextBox)d;
        InitModel(codeTextBox, model);
    }
    #endregion

    #region SelectionBrush
    public Brush SelectionBrush
    {
        get => (Brush)GetValue(SelectionBrushProperty);
        set => SetValue(SelectionBrushProperty, value);
    }

    public static readonly DependencyProperty SelectionBrushProperty =
        DependencyProperty.Register("SelectionBrush", typeof(Brush), typeof(CodeTextBox), new PropertyMetadata(new SolidColorBrush(new Color { R = 40, G = 80, B = 120, A = 100 })));
    #endregion

    #region HighlightBracketsBrush
    public Brush HighlightPairBracketsBrush
    {
        get => (Brush)GetValue(HighlightPairBracketsBrushProperty);
        set => SetValue(HighlightPairBracketsBrushProperty, value);
    }

    public static readonly DependencyProperty HighlightPairBracketsBrushProperty =
        DependencyProperty.Register("HighlightPairBracketsBrush", typeof(Brush), typeof(CodeTextBox));
    #endregion

    #region HighlightNoPairBracketBrush
    public Brush HighlightNoPairBracketBrush
    {
        get => (Brush)GetValue(HighlightNoPairBracketBrushProperty);
        set => SetValue(HighlightNoPairBracketBrushProperty, value);
    }

    public static readonly DependencyProperty HighlightNoPairBracketBrushProperty =
        DependencyProperty.Register("HighlightNoPairBracketBrush", typeof(Brush), typeof(CodeTextBox));
    #endregion

    #region CursorLineHighlightingBrush
    public Brush CursorLineHighlightingBrush
    {
        get => (Brush)GetValue(CursorLineHighlightingBrushProperty);
        set => SetValue(CursorLineHighlightingBrushProperty, value);
    }

    public static readonly DependencyProperty CursorLineHighlightingBrushProperty =
        DependencyProperty.Register("CursorLineHighlightingBrush", typeof(Brush), typeof(CodeTextBox));
    #endregion

    #region VerticalScrollBarValue
    public double VerticalScrollBarValue
    {
        get => (double)GetValue(VerticalScrollBarValueProperty);
        set => SetValue(VerticalScrollBarValueProperty, value);
    }

    public static readonly DependencyProperty VerticalScrollBarValueProperty =
        DependencyProperty.Register("VerticalScrollBarValue", typeof(double), typeof(CodeTextBox), new PropertyMetadata(0.0, VerticalScrollBarValueChangedCallback));

    private static void VerticalScrollBarValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var codeTextBox = (CodeTextBox)d;
        var value = (double)e.NewValue;
        if (value < 0) codeTextBox.VerticalScrollBarValue = 0.0;
        else if (value > codeTextBox.VerticalScrollBarMaximum) codeTextBox.VerticalScrollBarValue = codeTextBox.VerticalScrollBarMaximum;
        codeTextBox.InvalidateVisual();
    }
    #endregion

    #region VerticalScrollBarMaximum
    public double VerticalScrollBarMaximum
    {
        get => (double)GetValue(VerticalScrollBarMaximumProperty);
        set => SetValue(VerticalScrollBarMaximumProperty, value);
    }

    public static readonly DependencyProperty VerticalScrollBarMaximumProperty =
        DependencyProperty.Register("VerticalScrollBarMaximum", typeof(double), typeof(CodeTextBox), new PropertyMetadata(0.0, ScrollBarChangedCallback));
    #endregion

    #region HorizontalScrollBarValue
    public double HorizontalScrollBarValue
    {
        get => (double)GetValue(HorizontalScrollBarValueProperty);
        set => SetValue(HorizontalScrollBarValueProperty, value);
    }

    public static readonly DependencyProperty HorizontalScrollBarValueProperty =
        DependencyProperty.Register("HorizontalScrollBarValue", typeof(double), typeof(CodeTextBox), new PropertyMetadata(0.0, HorizontalScrollBarValueChangedCallback));

    private static void HorizontalScrollBarValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var codeTextBox = (CodeTextBox)d;
        var value = (double)e.NewValue;
        if (value < 0) codeTextBox.HorizontalScrollBarValue = 0.0;
        else if (value > codeTextBox.HorizontalScrollBarMaximum) codeTextBox.HorizontalScrollBarValue = codeTextBox.HorizontalScrollBarMaximum;
        codeTextBox.InvalidateVisual();
    }
    #endregion

    #region HorizontalScrollBarMaximum
    public double HorizontalScrollBarMaximum
    {
        get => (double)GetValue(HorizontalScrollBarMaximumProperty);
        set => SetValue(HorizontalScrollBarMaximumProperty, value);
    }

    public static readonly DependencyProperty HorizontalScrollBarMaximumProperty =
        DependencyProperty.Register("HorizontalScrollBarMaximum", typeof(double), typeof(CodeTextBox), new PropertyMetadata(0.0, ScrollBarChangedCallback));
    #endregion

    #region ViewportWidth
    public double ViewportWidth
    {
        get => (double)GetValue(ViewportWidthProperty);
        set => SetValue(ViewportWidthProperty, value);
    }

    public static readonly DependencyProperty ViewportWidthProperty =
        DependencyProperty.Register("ViewportWidth", typeof(double), typeof(CodeTextBox), new PropertyMetadata(0.0, ScrollBarChangedCallback));
    #endregion

    #region ViewportHeight
    public double ViewportHeight
    {
        get => (double)GetValue(ViewportHeightProperty);
        set => SetValue(ViewportHeightProperty, value);
    }

    public static readonly DependencyProperty ViewportHeightProperty =
        DependencyProperty.Register("ViewportHeight", typeof(double), typeof(CodeTextBox), new PropertyMetadata(0.0, ScrollBarChangedCallback));
    #endregion

    private static void InitModel(CodeTextBox codeTextBox, ICodeTextBoxModel model)
    {
        model.AttachCodeTextBox(codeTextBox);
        model.TextMeasuresEvents.LetterWidthChanged += (s, e) => { codeTextBox.TextLetterWidth = e.LetterWidth; };
        model.TextMeasuresEvents.LineHeightChanged += (s, e) => { codeTextBox.TextLineHeight = e.LineHeight; };
        model.TextEvents.LinesCountChanged += (s, e) => { codeTextBox.TextLinesCount = e.LinesCount; };
        codeTextBox.TextLinesCount = model.TextLinesCount;
        UpdateFontSettings(codeTextBox, codeTextBox._fontSettings);
        codeTextBox.ViewportHeight = codeTextBox.ActualHeight;
        codeTextBox.ViewportWidth = codeTextBox.ActualWidth;
        codeTextBox.ViewportSizeChanged?.Invoke(codeTextBox, EventArgs.Empty);
    }

    private static void ScrollBarChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var codeTextBox = (CodeTextBox)d;
        codeTextBox.InvalidateVisual();
    }

    private static void OnFontSettingsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var codeTextBox = (CodeTextBox)d;
        if (codeTextBox.Model == null) return;
        UpdateFontSettings(codeTextBox, codeTextBox._fontSettings);
    }

    private static void UpdateFontSettings(CodeTextBox codeTextBox, FontSettings fontSettings)
    {
        fontSettings.FontFamily = codeTextBox.FontFamily;
        fontSettings.FontSize = codeTextBox.FontSize;
        fontSettings.FontStretch = codeTextBox.FontStretch;
        fontSettings.FontStyle = codeTextBox.FontStyle;
        fontSettings.FontWeight = codeTextBox.FontWeight;
        codeTextBox.FontSettingsChanged?.Invoke(codeTextBox, new(fontSettings.LineHeight, fontSettings.LetterWidth));
    }

    static CodeTextBox()
    {
        FontSizeProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(OnFontSettingsChanged));
        FontFamilyProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(OnFontSettingsChanged));
        FontStyleProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(OnFontSettingsChanged));
        FontWeightProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(OnFontSettingsChanged));
        FontStretchProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(OnFontSettingsChanged));
    }

    private int _textLinesCount;
    public int TextLinesCount
    {
        get => _textLinesCount;
        set { _textLinesCount = value; PropertyChanged?.Invoke(this, new(nameof(TextLinesCount))); }
    }

    private double _textLineHeight;
    public double TextLineHeight
    {
        get => _textLineHeight;
        set { _textLineHeight = value; PropertyChanged?.Invoke(this, new(nameof(TextLineHeight))); }
    }

    private double _textLetterWidth;
    public double TextLetterWidth
    {
        get => _textLetterWidth;
        set { _textLetterWidth = value; PropertyChanged?.Invoke(this, new(nameof(TextLetterWidth))); }
    }

    public CodeTextBox()
    {
        _textRenderLogic = new TextRenderLogic();
        _textSelectionRenderLogic = new TextSelectionRenderLogic();
        _cursorRenderLogic = new CursorRenderLogic();
        _keyboardController = new KeyboardController();
        _mouseController = new MouseController();
        _highlightBracketsRenderLogic = new HighlightBracketsRenderLogic();
        _lineRenderLogic = new LineRenderLogic();
        _mouseSettings = new MouseSettings();
        _fontSettings = new FontSettings();
        Cursor = Cursors.IBeam;
        FocusVisualStyle = null;
        var template = new ControlTemplate(typeof(CodeTextBox));
        template.VisualTree = new FrameworkElementFactory(typeof(Grid), "RootLayout");
        template.VisualTree.AppendChild(new FrameworkElementFactory(typeof(Line), "CursorLine"));
        Template = template;
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        var grid = (Grid)Template.FindName("RootLayout", this);
        grid.ClipToBounds = true;
        var cursorLine = (Line)Template.FindName("CursorLine", this);
        _cursorRenderLogic.SetCursor(cursorLine, Foreground);
    }

    protected override void OnRender(DrawingContext context)
    {
        if (Model == null) return;
        context.PushClip(new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight)));
        context.DrawRectangle(Background ?? Brushes.White, null, new Rect(0, 0, ActualWidth, ActualHeight));
        if (IsFocused)
        {
            _cursorRenderLogic.DrawHighlightedCursorLine(Model, context, CursorLineHighlightingBrush, ActualWidth);
        }
        _lineRenderLogic.DrawLines(Model, context, ActualWidth);
        _textSelectionRenderLogic.DrawSelectedLines(Model, context, SelectionBrush);
        _highlightBracketsRenderLogic.DrawHighlightedBrackets(Model, context, HighlightPairBracketsBrush, HighlightNoPairBracketBrush);
        _textRenderLogic.DrawText(Model, _fontSettings, context, Foreground);
        if (IsFocused)
        {
            _cursorRenderLogic.DrawCursor(Model);
        }
        context.Pop();
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        if (Model == null) return;
        ViewportHeight = sizeInfo.NewSize.Height;
        ViewportWidth = sizeInfo.NewSize.Width;
        ViewportSizeChanged?.Invoke(this, EventArgs.Empty);
        InvalidateVisual();
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (Model == null) return;
        var positionInControl = e.GetPosition(this);
        _mouseController.OnMouseDown(this, Model, new(positionInControl.X, positionInControl.Y));
        Mouse.Capture(this);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (Model == null) return;
        var positionInControl = e.GetPosition(this);
        _mouseController.OnMouseMove(this, Model, new(positionInControl.X, positionInControl.Y), e.LeftButton);
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        base.OnMouseUp(e);
        Mouse.Capture(null);
        if (Model == null) return;
        _mouseController.OnMouseUp(Model);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        _mouseController.OnMouseWheel(this, this, Model!.TextMeasures, _mouseSettings.VerticalScrollLinesCount, e.Delta > 0);
    }

    protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
    {
        if (Model == null) return;
        var positionInControl = e.GetPosition(this);
        _mouseController.OnMouseDoubleClick(this, Model, new(positionInControl.X, positionInControl.Y));
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (Model == null) return;
        var controlPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
        //var altPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt;
        var shiftPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
        var key = e.Key == Key.System ? e.SystemKey : e.Key;
        e.Handled = _keyboardController.OnKeyDown(Model, key, controlPressed, shiftPressed);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        if (Model == null) return;
        var shiftPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
        e.Handled = _keyboardController.OnKeyUp(Model, shiftPressed);
    }

    protected override void OnTextInput(TextCompositionEventArgs e)
    {
        if (Model == null) return;
        _keyboardController.OnTextInput(Model, e.Text);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);
        _cursorRenderLogic.HideCursor();
        InvalidateVisual();
    }
}
