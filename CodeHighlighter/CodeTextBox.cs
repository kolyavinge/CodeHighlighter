using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CodeHighlighter.Controllers;
using CodeHighlighter.Input;
using CodeHighlighter.Model;
using CodeHighlighter.Rendering;

namespace CodeHighlighter;

public interface ICodeTextBox
{
    bool Focus();
    void InvalidateVisual();
}

public class CodeTextBox : Control, ICodeTextBox, IViewportContext, INotifyPropertyChanged
{
    private readonly CodeTextBoxModel _model;
    private readonly Viewport _viewport;
    private readonly FontSettings _fontSettings;
    private readonly TextMeasures _textMeasures;
    private readonly TextRenderLogic _textRenderLogic;
    private readonly TextSelectionRenderLogic _textSelectionRenderLogic;
    private readonly TokenKindUpdater _tokenKindUpdater;
    private readonly CursorRenderLogic _cursorRenderLogic;
    private readonly KeyboardController _keyboardController;
    private readonly MouseController _mouseController;
    private IHighlightBracketsRenderLogic _highlightBracketsRenderLogic;

    #region Events
    public event EventHandler? TextChanged;
    #endregion

    #region IsReadOnly
    public bool IsReadOnly
    {
        get { return (bool)GetValue(IsReadOnlyProperty); }
        set { SetValue(IsReadOnlyProperty, value); }
    }

    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
        "IsReadOnly", typeof(bool), typeof(CodeTextBox), new PropertyMetadata(false));
    #endregion

    #region Property SelectionBrush
    public Brush SelectionBrush
    {
        get { return (Brush)GetValue(SelectionBrushProperty); }
        set { SetValue(SelectionBrushProperty, value); }
    }

    public static readonly DependencyProperty SelectionBrushProperty =
        DependencyProperty.Register("SelectionBrush", typeof(Brush), typeof(CodeTextBox), new PropertyMetadata(new SolidColorBrush(new Color { R = 40, G = 80, B = 120, A = 100 })));
    #endregion

    #region HighlightBracketsBrush
    public Brush HighlightPairBracketsBrush
    {
        get { return (Brush)GetValue(HighlightPairBracketsBrushProperty); }
        set { SetValue(HighlightPairBracketsBrushProperty, value); }
    }

    public static readonly DependencyProperty HighlightPairBracketsBrushProperty =
        DependencyProperty.Register("HighlightPairBracketsBrush", typeof(Brush), typeof(CodeTextBox));
    #endregion

    #region HighlightNoPairBracketBrush
    public Brush HighlightNoPairBracketBrush
    {
        get { return (Brush)GetValue(HighlightNoPairBracketBrushProperty); }
        set { SetValue(HighlightNoPairBracketBrushProperty, value); }
    }

    public static readonly DependencyProperty HighlightNoPairBracketBrushProperty =
        DependencyProperty.Register("HighlightNoPairBracketBrush", typeof(Brush), typeof(CodeTextBox));
    #endregion

    #region HighlighteredBrackets
    public string HighlighteredBrackets
    {
        get { return (string)GetValue(HighlighteredBracketsProperty); }
        set { SetValue(HighlighteredBracketsProperty, value); }
    }

    public static readonly DependencyProperty HighlighteredBracketsProperty =
        DependencyProperty.Register("HighlighteredBrackets", typeof(string), typeof(CodeTextBox), new PropertyMetadata(HighlighteredBracketsPropertyChangedCallback));

    private static void HighlighteredBracketsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var codeTextBox = (CodeTextBox)d;
        var highlighteredBrackets = (string)e.NewValue;
        codeTextBox._highlightBracketsRenderLogic = new HighlightBracketsRenderLogic(
            new BracketsHighlighter(highlighteredBrackets, codeTextBox._model.Text, codeTextBox._model.TextCursor), codeTextBox._textMeasures, codeTextBox);
    }
    #endregion

    #region CursorLineHighlightingBrush
    public Brush CursorLineHighlightingBrush
    {
        get { return (Brush)GetValue(CursorLineHighlightingBrushProperty); }
        set { SetValue(CursorLineHighlightingBrushProperty, value); }
    }

    public static readonly DependencyProperty CursorLineHighlightingBrushProperty =
        DependencyProperty.Register("CursorLineHighlightingBrush", typeof(Brush), typeof(CodeTextBox));
    #endregion

    #region Property Text
    public TextHolder TextHolder
    {
        get { return (TextHolder)GetValue(TextHolderProperty); }
        set { SetValue(TextHolderProperty, value); }
    }

    public static readonly DependencyProperty TextHolderProperty =
        DependencyProperty.Register("TextHolder", typeof(TextHolder), typeof(CodeTextBox), new PropertyMetadata(TextHolderPropertyChangedCallback));

    private static void TextHolderPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var codeTextBox = (CodeTextBox)d;
        var textHolder = (TextHolder)e.NewValue;
        var initialText = textHolder.GetTextAction != null ? textHolder.GetTextAction() : null;
        if (!String.IsNullOrWhiteSpace(initialText)) codeTextBox.UpdateText(initialText);
        textHolder.GetTextAction = () => codeTextBox._model.Text.ToString();
        textHolder.SetTextAction = codeTextBox.UpdateText;
    }
    #endregion

    #region Property CodeProvider
    public ICodeProvider CodeProvider
    {
        get { return (ICodeProvider)GetValue(CodeProviderProperty); }
        set { SetValue(CodeProviderProperty, value); }
    }

    public static readonly DependencyProperty CodeProviderProperty =
        DependencyProperty.Register("CodeProvider", typeof(ICodeProvider), typeof(CodeTextBox), new PropertyMetadata(new CodeProviders.EmptyCodeProvider(), CodeProviderPropertyChangedCallback));

    private static void CodeProviderPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var codeTextBox = (CodeTextBox)d;
        if (e.NewValue != null)
        {
            codeTextBox.OnUpdateCodeProvider();
            codeTextBox.InvalidateVisual();
        }
    }
    #endregion

    #region VerticalScrollBarValue
    public double VerticalScrollBarValue
    {
        get { return (double)GetValue(VerticalScrollBarValueProperty); }
        set { SetValue(VerticalScrollBarValueProperty, value); }
    }

    public static readonly DependencyProperty VerticalScrollBarValueProperty =
        DependencyProperty.Register("VerticalScrollBarValue", typeof(double), typeof(CodeTextBox), new PropertyMetadata(0.0, VerticalScrollBarValueChangedCallback));

    private static void VerticalScrollBarValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var codeTextBox = (CodeTextBox)d;
        if ((double)e.NewValue < 0) codeTextBox.VerticalScrollBarValue = 0.0;
        codeTextBox.InvalidateVisual();
    }
    #endregion

    #region VerticalScrollBarMaximum
    public double VerticalScrollBarMaximum
    {
        get { return (double)GetValue(VerticalScrollBarMaximumProperty); }
        set { SetValue(VerticalScrollBarMaximumProperty, value); }
    }

    public static readonly DependencyProperty VerticalScrollBarMaximumProperty =
        DependencyProperty.Register("VerticalScrollBarMaximum", typeof(double), typeof(CodeTextBox), new PropertyMetadata(0.0, ScrollBarChangedCallback));
    #endregion

    #region VerticalScrollBarViewportSize
    public double VerticalScrollBarViewportSize
    {
        get { return (double)GetValue(VerticalScrollBarViewportSizeProperty); }
        set { SetValue(VerticalScrollBarViewportSizeProperty, value); }
    }

    public static readonly DependencyProperty VerticalScrollBarViewportSizeProperty =
        DependencyProperty.Register("VerticalScrollBarViewportSize", typeof(double), typeof(CodeTextBox), new PropertyMetadata(0.0, ScrollBarChangedCallback));
    #endregion

    #region HorizontalScrollBarValue
    public double HorizontalScrollBarValue
    {
        get { return (double)GetValue(HorizontalScrollBarValueProperty); }
        set { SetValue(HorizontalScrollBarValueProperty, value); }
    }

    public static readonly DependencyProperty HorizontalScrollBarValueProperty =
        DependencyProperty.Register("HorizontalScrollBarValue", typeof(double), typeof(CodeTextBox), new PropertyMetadata(0.0, HorizontalScrollBarValueChangedCallback));

    private static void HorizontalScrollBarValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var codeTextBox = (CodeTextBox)d;
        if ((double)e.NewValue < 0) codeTextBox.HorizontalScrollBarValue = 0.0;
        codeTextBox.InvalidateVisual();
    }
    #endregion

    #region HorizontalScrollBarMaximum
    public double HorizontalScrollBarMaximum
    {
        get { return (double)GetValue(HorizontalScrollBarMaximumProperty); }
        set { SetValue(HorizontalScrollBarMaximumProperty, value); }
    }

    public static readonly DependencyProperty HorizontalScrollBarMaximumProperty =
        DependencyProperty.Register("HorizontalScrollBarMaximum", typeof(double), typeof(CodeTextBox), new PropertyMetadata(0.0, ScrollBarChangedCallback));
    #endregion

    #region HorizontalScrollBarViewportSize
    public double HorizontalScrollBarViewportSize
    {
        get { return (double)GetValue(HorizontalScrollBarViewportSizeProperty); }
        set { SetValue(HorizontalScrollBarViewportSizeProperty, value); }
    }

    public static readonly DependencyProperty HorizontalScrollBarViewportSizeProperty =
        DependencyProperty.Register("HorizontalScrollBarViewportSize", typeof(double), typeof(CodeTextBox), new PropertyMetadata(0.0, ScrollBarChangedCallback));
    #endregion

    #region Commands
    public CodeTextBoxCommands Commands
    {
        get { return (CodeTextBoxCommands)GetValue(CommandsProperty); }
        set { SetValue(CommandsProperty, value); }
    }

    public static readonly DependencyProperty CommandsProperty = DependencyProperty.Register(
        "Commands", typeof(CodeTextBoxCommands), typeof(CodeTextBox), new PropertyMetadata(PropertyChangedCallback));

    private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var codeTextBox = (CodeTextBox)d;
        var commands = (CodeTextBoxCommands?)e.NewValue;
        if (commands != null)
        {
            commands.Init(new InputCommandContext(codeTextBox, codeTextBox._model, codeTextBox._viewport, codeTextBox, codeTextBox._textMeasures));
        }
    }
    #endregion

    private static void ScrollBarChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var codeTextBox = (CodeTextBox)d;
        codeTextBox.InvalidateVisual();
    }

    private static void OnFontSettingsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var codeTextBox = (CodeTextBox)d;
        codeTextBox._fontSettings.FontFamily = codeTextBox.FontFamily;
        codeTextBox._fontSettings.FontSize = codeTextBox.FontSize;
        codeTextBox._fontSettings.FontStretch = codeTextBox.FontStretch;
        codeTextBox._fontSettings.FontStyle = codeTextBox.FontStyle;
        codeTextBox._fontSettings.FontWeight = codeTextBox.FontWeight;
        codeTextBox._textMeasures.UpdateMeasures();
    }

    static CodeTextBox()
    {
        FontSizeProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(OnFontSettingsChanged));
        FontFamilyProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(OnFontSettingsChanged));
        FontStyleProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(OnFontSettingsChanged));
        FontWeightProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(OnFontSettingsChanged));
        FontStretchProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(OnFontSettingsChanged));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private int _textLinesCount;
    public int TextLinesCount
    {
        get => _textLinesCount;
        set
        {
            _textLinesCount = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TextLinesCount"));
        }
    }

    private double _textLineHeight;
    public double TextLineHeight
    {
        get => _textLineHeight;
        set
        {
            _textLineHeight = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TextLineHeight"));
        }
    }

    public CodeTextBox()
    {
        _model = new CodeTextBoxModel();
        _fontSettings = new() { FontSize = FontSize, FontFamily = FontFamily, FontStyle = FontStyle, FontWeight = FontWeight, FontStretch = FontStretch };
        _textMeasures = new TextMeasures(_fontSettings);
        _viewport = new Viewport(this, _textMeasures);
        _textRenderLogic = new TextRenderLogic(_model, _fontSettings, _textMeasures, _viewport, this);
        _textSelectionRenderLogic = new TextSelectionRenderLogic();
        _tokenKindUpdater = new TokenKindUpdater(_model.Tokens);
        _cursorRenderLogic = new CursorRenderLogic(_model.TextCursor, _textMeasures, this);
        _highlightBracketsRenderLogic = new DummyHighlightBracketsRenderLogic();
        Commands = new CodeTextBoxCommands();
        Commands.Init(new InputCommandContext(this, _model, _viewport, this, _textMeasures));
        _keyboardController = new KeyboardController(Commands, _model, _model);
        _mouseController = new MouseController(this, _model, _model, _model, _viewport, this);
        _model.Text.TextChanged += (s, e) => TextChanged?.Invoke(this, EventArgs.Empty);
        var textEvents = new TextEvents(_model.Text);
        textEvents.LinesCountChanged += (s, e) => TextLinesCount = e.LinesCount;
        TextLinesCount = 1;
        var textMeasuresEvents = new TextMeasuresEvents(_textMeasures);
        textMeasuresEvents.LineHeightChanged += (s, e) => TextLineHeight = e.LineHeight;
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
        var cursorLine = (Line)Template.FindName("CursorLine", this);
        _cursorRenderLogic.SetCursor(cursorLine, Foreground);
    }

    protected override void OnRender(DrawingContext context)
    {
        context.PushClip(new RectangleGeometry(new Rect(-1, -1, ActualWidth + 1, ActualHeight + 1)));
        context.DrawRectangle(Background ?? Brushes.White, null, new Rect(0, 0, ActualWidth, ActualHeight));
        if (IsFocused)
        {
            _cursorRenderLogic.DrawHighlightedCursorLine(context, CursorLineHighlightingBrush, ActualWidth);
        }
        _textSelectionRenderLogic.DrawSelectedLines(context, SelectionBrush, _model.TextSelection.GetSelectedLines(_model.Text), _textMeasures, this);
        _highlightBracketsRenderLogic.DrawHighlightedBrackets(context, HighlightPairBracketsBrush, HighlightNoPairBracketBrush);
        _textRenderLogic.DrawText(context, Foreground);
        if (IsFocused)
        {
            _cursorRenderLogic.DrawCursor();
        }
        context.Pop();
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        VerticalScrollBarViewportSize = sizeInfo.NewSize.Height;
        HorizontalScrollBarViewportSize = sizeInfo.NewSize.Width;
        _viewport.UpdateScrollbarsMaximumValues(_model.Text);
        InvalidateVisual();
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        var positionInControl = e.GetPosition(this);
        var shiftPressed = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
        _mouseController.OnMouseDown(positionInControl, shiftPressed);
        Mouse.Capture(this);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        var positionInControl = e.GetPosition(this);
        _mouseController.OnMouseMove(positionInControl, e.LeftButton);
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        base.OnMouseUp(e);
        Mouse.Capture(null);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        _mouseController.OnMouseWheel(e.Delta);
    }

    protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
    {
        var positionInControl = e.GetPosition(this);
        _mouseController.OnMouseDoubleClick(positionInControl);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        var controlPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
        var altPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt;
        var shiftPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
        var key = e.Key == Key.System ? e.SystemKey : e.Key;
        e.Handled = _keyboardController.OnKeyDown(key, controlPressed, altPressed, shiftPressed, IsReadOnly);
    }

    protected override void OnTextInput(TextCompositionEventArgs e)
    {
        _keyboardController.OnTextInput(e.Text, IsReadOnly);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);
        _cursorRenderLogic.HideCursor();
        InvalidateVisual();
    }

    private void UpdateText(string text)
    {
        _model.SetText(text);
        OnUpdateCodeProvider();
        InvalidateVisual();
    }

    private void OnUpdateCodeProvider()
    {
        _model.SetCodeProvider(CodeProvider);
        if (CodeProvider is ITokenKindUpdatable tokenKindUpdatable)
        {
            tokenKindUpdatable.TokenKindUpdated += (s, e) =>
            {
                _tokenKindUpdater.UpdateTokenKinds(e.UpdatedTokenKinds);
                InvalidateVisual();
            };
        }
        _viewport.UpdateScrollbarsMaximumValues(_model.Text);
    }
}
