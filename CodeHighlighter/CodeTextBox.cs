using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CodeHighlighter.Model;

namespace CodeHighlighter
{
    public class CodeTextBox : Control, IViewportContext
    {
        private readonly CodeTextBoxModel _model;
        private readonly FontSettings _fontSettings;
        private readonly TextMeasures _textMeasures;
        private readonly Viewport _viewport;

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
            DependencyProperty.Register("CodeProvider", typeof(ICodeProvider), typeof(CodeTextBox), new PropertyMetadata(CodeProviderPropertyChangedCallback));

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
            _viewport = new Viewport(this, _model.Text, _textMeasures);
            Cursor = Cursors.IBeam;
            FocusVisualStyle = null;
        }

        protected override void OnRender(DrawingContext context)
        {
            context.PushClip(new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight)));
            context.DrawRectangle(Background ?? Brushes.White, null, new Rect(0, 0, ActualWidth, ActualHeight));
            // selection
            foreach (var line in _model.TextSelection.GetSelectedLines(_model.Text))
            {
                DrawSelectionLine(context, line.LineIndex, line.LeftColumnIndex, line.RightColumnIndex);
            }
            // lexems
            var typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            var startLine = (int)(VerticalScrollBarValue / _textMeasures.LineHeight);
            var linesCount = _viewport.GetLinesCountInViewport();
            var endLine = Math.Min(startLine + linesCount, _model.Text.VisibleLinesCount);
            var offsetY = -(VerticalScrollBarValue % _textMeasures.LineHeight);
            for (var lineIndex = startLine; lineIndex < endLine; lineIndex++)
            {
                var offsetX = -HorizontalScrollBarValue;
                var lineLexems = _model.Lexems.GetLexemsForLine(lineIndex);
                foreach (var lexem in lineLexems)
                {
                    var text = _model.Text.GetSubstring(lineIndex, lexem.ColumnIndex, lexem.Length);
                    var brush = _model.LexemColors.GetColorBrushOrNull(lexem.Kind) ?? Foreground;
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
                        new Point((int)cursorAbsolutePoint.X + 1.0, (int)cursorAbsolutePoint.Y),
                        new Point((int)cursorAbsolutePoint.X + 1.0, (int)(cursorAbsolutePoint.Y + _textMeasures.LineHeight)));
                }
            }
            context.Pop();
        }

        private void DrawSelectionLine(DrawingContext context, int lineIndex, int leftColumnIndex, int rightColumnIndex)
        {
            var leftColumnPos = leftColumnIndex * _textMeasures.LetterWidth - HorizontalScrollBarValue;
            var rightColumnPos = rightColumnIndex * _textMeasures.LetterWidth - HorizontalScrollBarValue;
            context.DrawRectangle(
                SelectionBrush,
                null,
                new Rect(leftColumnPos, lineIndex * _textMeasures.LineHeight - VerticalScrollBarValue, rightColumnPos - leftColumnPos, _textMeasures.LineHeight));
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            VerticalScrollBarViewportSize = sizeInfo.NewSize.Height;
            HorizontalScrollBarViewportSize = sizeInfo.NewSize.Width;
            _viewport.UpdateScrollbarsMaximumValues();
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            var needToInvalidate = true;
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                _model.MoveCursorUp();
            }
            else if (e.Key == Key.Down)
            {
                e.Handled = true;
                _model.MoveCursorDown();
            }
            else if (e.Key == Key.Left)
            {
                e.Handled = true;
                _model.MoveCursorLeft();
            }
            else if (e.Key == Key.Right)
            {
                e.Handled = true;
                _model.MoveCursorRight();
            }
            else if (e.Key == Key.Home && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                _model.MoveCursorTextBegin();
            }
            else if (e.Key == Key.Home)
            {
                _model.MoveCursorStartLine();
            }
            else if (e.Key == Key.End && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                _model.MoveCursorTextEnd();
            }
            else if (e.Key == Key.End)
            {
                _model.MoveCursorEndLine();
            }
            else if (e.Key == Key.PageUp)
            {
                _model.MoveCursorPageUp(_viewport.GetLinesCountInViewport());
            }
            else if (e.Key == Key.PageDown)
            {
                _model.MoveCursorPageDown(_viewport.GetLinesCountInViewport());
            }
            else if (e.Key == Key.Return)
            {
                _model.NewLine();
            }
            else if (e.Key == Key.Back)
            {
                _model.LeftDelete();
            }
            else if (e.Key == Key.Delete)
            {
                _model.RightDelete();
            }
            else if (e.Key == Key.Tab)
            {
                _model.AppendChar('\t');
            }
            else if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                _model.StartSelection();
            }
            else if (e.Key == Key.A && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                _model.SelectAll();
            }
            else if (e.Key == Key.X && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                Clipboard.SetText(_model.GetSelectedText());
                _model.LeftDelete();
            }
            else if (e.Key == Key.C && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                Clipboard.SetText(_model.GetSelectedText());
            }
            else if (e.Key == Key.V && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                _model.InsertText(Clipboard.GetText());
            }
            else
            {
                needToInvalidate = false;
            }
            if (needToInvalidate)
            {
                _viewport.CorrectViewport(_model.TextCursor.GetAbsolutePosition(_textMeasures));
                InvalidateVisual();
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

        private static readonly HashSet<char> _notAllowedSymbols = new(new[] { '\n', '\r', '\b' });
        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            var text = e.Text.Where(ch => !_notAllowedSymbols.Contains(ch)).ToList();
            if (!text.Any()) return;
            foreach (var ch in text)
            {
                _model.AppendChar(ch);
            }
            _viewport.CorrectViewport(_model.TextCursor.GetAbsolutePosition(_textMeasures));
            InvalidateVisual();
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
            _viewport.UpdateScrollbarsMaximumValues();
        }
    }
}
