using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CodeHighlighter.Commands;
using CodeHighlighter.Input;
using CodeHighlighter.Model;
using CodeHighlighter.Rendering;

namespace CodeHighlighter
{
    public interface ICodeTextBox
    {
        void InvalidateVisual();
    }

    public class CodeTextBox : Control, ICodeTextBox, IViewportContext
    {
        private readonly CodeTextBoxModel _model;
        private readonly Viewport _viewport;
        private readonly FontSettings _fontSettings;
        private readonly TextMeasures _textMeasures;
        private readonly TextSelectionRenderLogic _textSelectionRenderLogic;

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
            "Commands", typeof(CodeTextBoxCommands), typeof(CodeTextBox), new PropertyMetadata(new CodeTextBoxCommands(), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var codeTextBox = (CodeTextBox)d;
            var commands = (CodeTextBoxCommands)e.NewValue;
            commands.Init(new InputCommandContext(codeTextBox, codeTextBox._model, codeTextBox._viewport));
        }
        #endregion

        private static void ScrollBarChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var codeTextBox = (CodeTextBox)d;
            codeTextBox.InvalidateVisual();
        }

        static CodeTextBox()
        {
            FontSizeProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(OnFontSettingsChanged));
            FontFamilyProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(OnFontSettingsChanged));
            FontStyleProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(OnFontSettingsChanged));
            FontWeightProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(OnFontSettingsChanged));
            FontStretchProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(OnFontSettingsChanged));
        }

        private static void OnFontSettingsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CodeTextBox)d;
            control._fontSettings.FontFamily = control.FontFamily;
            control._fontSettings.FontSize = control.FontSize;
            control._fontSettings.FontStretch = control.FontStretch;
            control._fontSettings.FontStyle = control.FontStyle;
            control._fontSettings.FontWeight = control.FontWeight;
            control._textMeasures.UpdateMeasures();
        }

        public CodeTextBox()
        {
            _model = new CodeTextBoxModel();
            _fontSettings = new() { FontSize = FontSize, FontFamily = FontFamily, FontStyle = FontStyle, FontWeight = FontWeight, FontStretch = FontStretch };
            _textMeasures = new TextMeasures(_fontSettings);
            _viewport = new Viewport(this, _textMeasures);
            _textSelectionRenderLogic = new TextSelectionRenderLogic();
            Cursor = Cursors.IBeam;
            FocusVisualStyle = null;
        }

        protected override void OnRender(DrawingContext context)
        {
            context.PushClip(new RectangleGeometry(new Rect(-1, -1, ActualWidth + 1, ActualHeight + 1)));
            context.DrawRectangle(Background ?? Brushes.White, null, new Rect(0, 0, ActualWidth, ActualHeight));
            // selection
            _textSelectionRenderLogic.DrawSelectedLines(context, SelectionBrush, _model.TextSelection.GetSelectedLines(_model.Text), _textMeasures, this);
            // tokens
            var typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            var startLine = (int)(VerticalScrollBarValue / _textMeasures.LineHeight);
            var linesCount = _viewport.GetLinesCountInViewport();
            var endLine = Math.Min(startLine + linesCount, _model.Text.VisibleLinesCount);
            var offsetY = -(VerticalScrollBarValue % _textMeasures.LineHeight);
            for (var lineIndex = startLine; lineIndex < endLine; lineIndex++)
            {
                var offsetX = -HorizontalScrollBarValue;
                var lineTokens = _model.Tokens.GetMergedTokens(lineIndex);
                foreach (var token in lineTokens)
                {
                    var text = _model.Text.GetSubstring(lineIndex, token.ColumnIndex, token.Length);
                    var brush = _model.TokenColors.GetColorBrushOrNull(token.Kind) ?? Foreground;
                    var formattedText = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, FontSize, brush, 1.0);
                    context.DrawText(formattedText, new Point(offsetX, offsetY));
                    offsetX += formattedText.WidthIncludingTrailingWhitespace;
                }
                offsetY += _textMeasures.LineHeight;
            }
            // cursor
            if (IsFocused)
            {
                var cursorAbsolutePoint = _model.TextCursor.GetAbsolutePosition(_textMeasures);
                cursorAbsolutePoint.X -= HorizontalScrollBarValue;
                cursorAbsolutePoint.Y -= VerticalScrollBarValue;
                if (cursorAbsolutePoint.X >= 0 && cursorAbsolutePoint.Y >= 0)
                {
                    context.DrawLine(new Pen(Foreground, 2.0),
                        new Point((int)cursorAbsolutePoint.X, (int)cursorAbsolutePoint.Y),
                        new Point((int)cursorAbsolutePoint.X, (int)(cursorAbsolutePoint.Y + _textMeasures.LineHeight)));
                }
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
            Focus();
            var positionInControl = e.GetPosition(this);
            var lineIndex = _viewport.GetCursorLineIndex(positionInControl);
            var columnIndex = _viewport.CursorColumnIndex(positionInControl);
            _model.MoveCursorTo(lineIndex, columnIndex);
            InvalidateVisual();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _model.StartSelection();
                var positionInControl = e.GetPosition(this);
                var lineIndex = _viewport.GetCursorLineIndex(positionInControl);
                var columnIndex = _viewport.CursorColumnIndex(positionInControl);
                _model.MoveCursorTo(lineIndex, columnIndex);
                InvalidateVisual();
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            _model.EndSelection();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            VerticalScrollBarValue -= e.Delta;
            InvalidateVisual();
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            var positionInControl = e.GetPosition(this);
            var lineIndex = _viewport.GetCursorLineIndex(positionInControl);
            var columnIndex = _viewport.CursorColumnIndex(positionInControl);
            _model.SelectToken(lineIndex, columnIndex);
            InvalidateVisual();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            var controlPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            // with control pressed
            if (controlPressed && e.Key == Key.Up)
            {
                e.Handled = true;
                Commands.ScrollLineUpCommand.Execute();
            }
            else if (controlPressed && e.Key == Key.Down)
            {
                e.Handled = true;
                Commands.ScrollLineDownCommand.Execute();
            }
            else if (controlPressed && e.Key == Key.Home)
            {
                Commands.MoveCursorTextBeginCommand.Execute();
            }
            else if (controlPressed && e.Key == Key.End)
            {
                Commands.MoveCursorTextEndCommand.Execute();
            }
            else if (controlPressed && e.Key == Key.A)
            {
                Commands.SelectAllCommand.Execute();
            }
            else if (controlPressed && e.Key == Key.X)
            {
                if (IsReadOnly) return;
                Clipboard.SetText(_model.GetSelectedText());
                Commands.LeftDeleteCommand.Execute();
            }
            else if (controlPressed && e.Key == Key.C)
            {
                Clipboard.SetText(_model.GetSelectedText());
            }
            else if (controlPressed && e.Key == Key.V)
            {
                if (IsReadOnly) return;
                Commands.InsertTextCommand.Execute(new InsertTextCommandParameter(Clipboard.GetText()));
            }
            else if (controlPressed && e.Key == Key.L)
            {
                if (IsReadOnly) return;
                Commands.DeleteSelectedLinesCommand.Execute();
            }
            // without any modifiers
            else if (e.Key == Key.Up)
            {
                e.Handled = true;
                Commands.MoveCursorUpCommand.Execute();
            }
            else if (e.Key == Key.Down)
            {
                e.Handled = true;
                Commands.MoveCursorDownCommand.Execute();
            }
            else if (e.Key == Key.Left)
            {
                e.Handled = true;
                Commands.MoveCursorLeftCommand.Execute();
            }
            else if (e.Key == Key.Right)
            {
                e.Handled = true;
                Commands.MoveCursorRightCommand.Execute();
            }
            else if (e.Key == Key.Home)
            {
                Commands.MoveCursorStartLineCommand.Execute();
            }
            else if (e.Key == Key.End)
            {
                Commands.MoveCursorEndLineCommand.Execute();
            }
            else if (e.Key == Key.PageUp)
            {
                Commands.MoveCursorPageUpCommand.Execute();
            }
            else if (e.Key == Key.PageDown)
            {
                Commands.MoveCursorPageDownCommand.Execute();
            }
            else if (e.Key == Key.Return)
            {
                if (IsReadOnly) return;
                Commands.NewLineCommand.Execute();
            }
            else if (e.Key == Key.Back)
            {
                if (IsReadOnly) return;
                Commands.LeftDeleteCommand.Execute();
            }
            else if (e.Key == Key.Delete)
            {
                if (IsReadOnly) return;
                Commands.RightDeleteCommand.Execute();
            }
            else if (e.Key == Key.Tab)
            {
                _model.AppendChar('\t');
            }
            else if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                _model.StartSelection();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            e.Handled = true;
            var needToInvalidate = true;
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                _model.EndSelection();
            }
            else
            {
                needToInvalidate = false;
            }
            if (needToInvalidate)
            {
                InvalidateVisual();
            }
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            if (IsReadOnly) return;
            Commands.TextInputCommand.Execute(new TextInputCommandParameter(e.Text));
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
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
            _viewport.UpdateScrollbarsMaximumValues(_model.Text);
        }
    }
}
