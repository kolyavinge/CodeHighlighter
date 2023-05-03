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

public class CodeTextBoxView : Control, ICodeTextBoxView, INotifyPropertyChanged
{
    private IKeyboardController? _keyboardController;
    private IMouseController? _mouseController;
    private ICodeTextBoxRendering? _renderingModel;
    private RenderingContext? _renderingContext;
    private readonly MouseSettings _mouseSettings;
    private readonly CursorRenderLogic _cursorRenderLogic;

    public event EventHandler<FontSettingsChangedEventArgs>? FontSettingsChanged;
    public event EventHandler? ViewportSizeChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    #region Model
    public ICodeTextBox? Model
    {
        get => (ICodeTextBox?)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    public static readonly DependencyProperty ModelProperty =
        DependencyProperty.Register("Model", typeof(ICodeTextBox), typeof(CodeTextBoxView), new PropertyMetadata(ModelPropertyChangedCallback));

    private static void ModelPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var model = (ICodeTextBox)e.NewValue ?? throw new ArgumentNullException(nameof(Model));
        var codeTextBox = (CodeTextBoxView)d;
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
        DependencyProperty.Register("SelectionBrush", typeof(Brush), typeof(CodeTextBoxView), new PropertyMetadata(new SolidColorBrush(new Color { R = 40, G = 80, B = 120, A = 100 })));
    #endregion

    #region HighlightBracketsBrush
    public Brush HighlightPairBracketsBrush
    {
        get => (Brush)GetValue(HighlightPairBracketsBrushProperty);
        set => SetValue(HighlightPairBracketsBrushProperty, value);
    }

    public static readonly DependencyProperty HighlightPairBracketsBrushProperty =
        DependencyProperty.Register("HighlightPairBracketsBrush", typeof(Brush), typeof(CodeTextBoxView));
    #endregion

    #region HighlightNoPairBracketBrush
    public Brush HighlightNoPairBracketBrush
    {
        get => (Brush)GetValue(HighlightNoPairBracketBrushProperty);
        set => SetValue(HighlightNoPairBracketBrushProperty, value);
    }

    public static readonly DependencyProperty HighlightNoPairBracketBrushProperty =
        DependencyProperty.Register("HighlightNoPairBracketBrush", typeof(Brush), typeof(CodeTextBoxView));
    #endregion

    #region CursorLineHighlightingBrush
    public Brush CursorLineHighlightingBrush
    {
        get => (Brush)GetValue(CursorLineHighlightingBrushProperty);
        set => SetValue(CursorLineHighlightingBrushProperty, value);
    }

    public static readonly DependencyProperty CursorLineHighlightingBrushProperty =
        DependencyProperty.Register("CursorLineHighlightingBrush", typeof(Brush), typeof(CodeTextBoxView));
    #endregion

    #region LineGapBrush
    public Brush LineGapBrush
    {
        get => (Brush)GetValue(LineGapBrushProperty);
        set => SetValue(LineGapBrushProperty, value);
    }

    public static readonly DependencyProperty LineGapBrushProperty =
        DependencyProperty.Register("LineGapBrush", typeof(Brush), typeof(CodeTextBoxView), new PropertyMetadata(Brushes.Gray));
    #endregion

    #region ActivatedFoldBrush
    public Brush ActivatedFoldBrush
    {
        get { return (Brush)GetValue(ActivatedFoldBrushProperty); }
        set { SetValue(ActivatedFoldBrushProperty, value); }
    }

    public static readonly DependencyProperty ActivatedFoldBrushProperty =
        DependencyProperty.Register("ActivatedFoldBrush", typeof(Brush), typeof(CodeTextBoxView), new PropertyMetadata(Brushes.Gray));
    #endregion

    #region VerticalScrollBarValue
    public double VerticalScrollBarValue
    {
        get => (double)GetValue(VerticalScrollBarValueProperty);
        set => SetValue(VerticalScrollBarValueProperty, value);
    }

    public static readonly DependencyProperty VerticalScrollBarValueProperty =
        DependencyProperty.Register("VerticalScrollBarValue", typeof(double), typeof(CodeTextBoxView), new PropertyMetadata(0.0, VerticalScrollBarValueChangedCallback));

    private static void VerticalScrollBarValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var codeTextBox = (CodeTextBoxView)d;
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
        DependencyProperty.Register("VerticalScrollBarMaximum", typeof(double), typeof(CodeTextBoxView), new PropertyMetadata(0.0, ScrollBarChangedCallback));
    #endregion

    #region HorizontalScrollBarValue
    public double HorizontalScrollBarValue
    {
        get => (double)GetValue(HorizontalScrollBarValueProperty);
        set => SetValue(HorizontalScrollBarValueProperty, value);
    }

    public static readonly DependencyProperty HorizontalScrollBarValueProperty =
        DependencyProperty.Register("HorizontalScrollBarValue", typeof(double), typeof(CodeTextBoxView), new PropertyMetadata(0.0, HorizontalScrollBarValueChangedCallback));

    private static void HorizontalScrollBarValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var codeTextBox = (CodeTextBoxView)d;
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
        DependencyProperty.Register("HorizontalScrollBarMaximum", typeof(double), typeof(CodeTextBoxView), new PropertyMetadata(0.0, ScrollBarChangedCallback));
    #endregion

    #region ViewportWidth
    public double ViewportWidth
    {
        get => (double)GetValue(ViewportWidthProperty);
        set => SetValue(ViewportWidthProperty, value);
    }

    public static readonly DependencyProperty ViewportWidthProperty =
        DependencyProperty.Register("ViewportWidth", typeof(double), typeof(CodeTextBoxView), new PropertyMetadata(0.0, ScrollBarChangedCallback));
    #endregion

    #region ViewportHeight
    public double ViewportHeight
    {
        get => (double)GetValue(ViewportHeightProperty);
        set => SetValue(ViewportHeightProperty, value);
    }

    public static readonly DependencyProperty ViewportHeightProperty =
        DependencyProperty.Register("ViewportHeight", typeof(double), typeof(CodeTextBoxView), new PropertyMetadata(0.0, ScrollBarChangedCallback));
    #endregion

    private static void InitModel(CodeTextBoxView codeTextBox, ICodeTextBox model)
    {
        model.AttachCodeTextBox(codeTextBox);
        model.TextMeasuresEvents.LetterWidthChanged += (s, e) => { codeTextBox.TextLetterWidth = e.LetterWidth; };
        model.TextMeasuresEvents.LineHeightChanged += (s, e) => { codeTextBox.TextLineHeight = e.LineHeight; };
        model.TextEvents.LinesCountChanged += (s, e) => { codeTextBox.TextLinesCount = e.LinesCount; };
        codeTextBox.TextLinesCount = model.TextLines.Count;
        UpdateFontSettings(codeTextBox, codeTextBox.FontSettings);
        codeTextBox.ViewportHeight = codeTextBox.ActualHeight;
        codeTextBox.ViewportWidth = codeTextBox.ActualWidth;
        codeTextBox.ViewportSizeChanged?.Invoke(codeTextBox, EventArgs.Empty);
        codeTextBox._keyboardController = ControllerFactory.MakeKeyboardController(model);
        codeTextBox._mouseController = ControllerFactory.MakeMouseController(codeTextBox, model);
        codeTextBox._renderingContext = new RenderingContext(codeTextBox);
        codeTextBox._renderingContext.SetColorsForText(model.TokensColors);
        codeTextBox._renderingModel = RenderingModelFactory.MakeModel(model, codeTextBox._renderingContext);
    }

    private static void ScrollBarChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var codeTextBox = (CodeTextBoxView)d;
        codeTextBox.InvalidateVisual();
    }

    private static void OnFontSettingsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var codeTextBox = (CodeTextBoxView)d;
        if (codeTextBox.Model == null) return;
        UpdateFontSettings(codeTextBox, codeTextBox.FontSettings);
    }

    private static void UpdateFontSettings(CodeTextBoxView codeTextBox, FontSettings fontSettings)
    {
        fontSettings.FontFamily = codeTextBox.FontFamily;
        fontSettings.FontSize = codeTextBox.FontSize;
        fontSettings.FontStretch = codeTextBox.FontStretch;
        fontSettings.FontStyle = codeTextBox.FontStyle;
        fontSettings.FontWeight = codeTextBox.FontWeight;
        codeTextBox.FontSettingsChanged?.Invoke(codeTextBox, new(fontSettings.LineHeight, fontSettings.LetterWidth));
    }

    static CodeTextBoxView()
    {
        FontSizeProperty.OverrideMetadata(typeof(CodeTextBoxView), new FrameworkPropertyMetadata(OnFontSettingsChanged));
        FontFamilyProperty.OverrideMetadata(typeof(CodeTextBoxView), new FrameworkPropertyMetadata(OnFontSettingsChanged));
        FontStyleProperty.OverrideMetadata(typeof(CodeTextBoxView), new FrameworkPropertyMetadata(OnFontSettingsChanged));
        FontWeightProperty.OverrideMetadata(typeof(CodeTextBoxView), new FrameworkPropertyMetadata(OnFontSettingsChanged));
        FontStretchProperty.OverrideMetadata(typeof(CodeTextBoxView), new FrameworkPropertyMetadata(OnFontSettingsChanged));
    }

    internal FontSettings FontSettings { get; private set; }

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

    private bool _isHorizontalScrollBarVisible;
    public bool IsHorizontalScrollBarVisible
    {
        get => _isHorizontalScrollBarVisible;
        set { _isHorizontalScrollBarVisible = value; PropertyChanged?.Invoke(this, new(nameof(IsHorizontalScrollBarVisible))); }
    }

    public CodeTextBoxView()
    {
        _cursorRenderLogic = new CursorRenderLogic();
        _mouseSettings = new MouseSettings();
        FontSettings = new FontSettings();
        Cursor = Cursors.IBeam;
        FocusVisualStyle = null;
        var template = new ControlTemplate(typeof(CodeTextBoxView));
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
        if (_renderingModel == null) return;
        _renderingContext!.SetContext(context);
        context.PushClip(new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight)));
        context.DrawRectangle(Background ?? Brushes.White, null, new Rect(0, 0, ActualWidth, ActualHeight));
        _renderingModel.ActivatedLineFolds.RenderActivatedFoldLines(ActivatedFoldBrush);
        if (IsFocused)
        {
            _cursorRenderLogic.DrawHighlightedCursorLine(Model, context, CursorLineHighlightingBrush, ActualWidth);
        }
        _renderingModel.LinesDecoration.Render();
        _renderingModel.TextHighlight.Render();
        _renderingModel.TextSelection.Render(SelectionBrush);
        _renderingModel.HighlightBrackets.Render(HighlightPairBracketsBrush, HighlightNoPairBracketBrush);
        _renderingModel.Text.Render();
        _renderingModel.LineGap.Render(LineGapBrush);
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
        if (_mouseController == null) return;
        var positionInControl = e.GetPosition(this);
        if (e.ChangedButton == MouseButton.Left)
        {
            _mouseController.LeftButtonDown(new(positionInControl.X, positionInControl.Y));
            Mouse.Capture(this);
        }
        else if (e.ChangedButton == MouseButton.Right)
        {
            _mouseController.RightButtonDown(new(positionInControl.X, positionInControl.Y));
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (_mouseController == null) return;
        var positionInControl = e.GetPosition(this);
        _mouseController.Move(new(positionInControl.X, positionInControl.Y));
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        if (_mouseController == null) return;
        if (e.ChangedButton == MouseButton.Left)
        {
            _mouseController.LeftButtonUp();
            Mouse.Capture(null);
        }
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        if (_mouseController == null) return;
        _mouseController.ScrollWheel(_mouseSettings.VerticalScrollLinesCount, e.Delta > 0);
    }

    protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
    {
        if (_mouseController == null) return;
        var positionInControl = e.GetPosition(this);
        _mouseController.LeftButtonDoubleClick(new(positionInControl.X, positionInControl.Y));
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (_keyboardController == null) return;
        var controlPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
        var shiftPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
        var key = e.Key == System.Windows.Input.Key.System ? e.SystemKey : e.Key;
        e.Handled = _keyboardController.KeyDown((Controllers.Key)key, controlPressed, shiftPressed);
        _cursorRenderLogic.ResetAnimation();
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        if (_keyboardController == null) return;
        var shiftPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
        e.Handled = _keyboardController.KeyUp(shiftPressed);
    }

    protected override void OnTextInput(TextCompositionEventArgs e)
    {
        if (_keyboardController == null) return;
        _keyboardController.TextInput(e.Text);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);
        _cursorRenderLogic.HideCursor();
        InvalidateVisual();
    }

    public void ClipboardSetText(string text)
    {
        Clipboard.SetText(text);
    }

    public string ClipboardGetText()
    {
        return Clipboard.GetText();
    }
}
