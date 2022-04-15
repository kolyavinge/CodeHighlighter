using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using CodeHighlighter.Model;

namespace CodeHighlighter
{
    public class CodeTextBox : Control
    {
        private static readonly Pen CursorBlackPen = new(Brushes.Black, 1.5);
        private static readonly Brush SelectionBrush = new SolidColorBrush(new Color { R = 40, G = 80, B = 120, A = 100 });

        private readonly CodeTextBoxModel _model;
        private readonly FontSettings _fontSettings;
        private readonly TextMeasures _textMeasures;
        private Viewport? _viewport;
        private ScrollBar? _verticalScrollBar;
        private RepeatButton? _verticalScrollBarUpButton, _verticalScrollBarDownButton;
        private ScrollBar? _horizontalScrollBar;
        private RepeatButton? _horizontalScrollBarLeftButton, _horizontalScrollBarRightButton;

        #region Property Text
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(CodeTextBox), new PropertyMetadata("", TextPropertyChangedCallback));

        private static void TextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var codeTextBox = (CodeTextBox)d;
            codeTextBox._model.SetText((string)e.NewValue);
            codeTextBox.OnUpdateCodeProvider();
            codeTextBox.InvalidateVisual();
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
            var template = new ControlTemplate(typeof(CodeTextBox));
            template.VisualTree = new FrameworkElementFactory(typeof(Grid), "RootLayout");
            template.VisualTree.AppendChild(new FrameworkElementFactory(typeof(ScrollBar), "VerticalScrollBar"));
            template.VisualTree.AppendChild(new FrameworkElementFactory(typeof(ScrollBar), "HorizontalScrollBar"));
            Template = template;
            Cursor = Cursors.IBeam;
            FocusVisualStyle = null;
        }

        public override void OnApplyTemplate()
        {
            _verticalScrollBar = (ScrollBar)Template.FindName("VerticalScrollBar", this);
            _verticalScrollBar.Minimum = 0;
            _verticalScrollBar.Orientation = Orientation.Vertical;
            _verticalScrollBar.HorizontalAlignment = HorizontalAlignment.Right;
            _verticalScrollBar.Cursor = Cursors.Arrow;
            _verticalScrollBar.Scroll += (s, e) => InvalidateVisual();
            _verticalScrollBar.Loaded += (s, e) =>
            {
                _verticalScrollBarUpButton = (RepeatButton)_verticalScrollBar.Template.FindName("PART_LineUpButton", _verticalScrollBar);
                _verticalScrollBarDownButton = (RepeatButton)_verticalScrollBar.Template.FindName("PART_LineDownButton", _verticalScrollBar);
                _verticalScrollBarUpButton.Click += (s, e) => { _verticalScrollBar.Value -= _textMeasures.LineHeight; InvalidateVisual(); };
                _verticalScrollBarDownButton.Click += (s, e) => { _verticalScrollBar.Value += _textMeasures.LineHeight; InvalidateVisual(); };
            };
            _horizontalScrollBar = (ScrollBar)Template.FindName("HorizontalScrollBar", this);
            _horizontalScrollBar.Minimum = 0;
            _horizontalScrollBar.Orientation = Orientation.Horizontal;
            _horizontalScrollBar.VerticalAlignment = VerticalAlignment.Bottom;
            _horizontalScrollBar.Margin = new Thickness(0, 0, _verticalScrollBar.Width, 0);
            _horizontalScrollBar.Cursor = Cursors.Arrow;
            _horizontalScrollBar.Scroll += (s, e) => InvalidateVisual();
            _horizontalScrollBar.Loaded += (s, e) =>
            {
                _horizontalScrollBarLeftButton = (RepeatButton)_horizontalScrollBar.Template.FindName("PART_LineLeftButton", _horizontalScrollBar);
                _horizontalScrollBarRightButton = (RepeatButton)_horizontalScrollBar.Template.FindName("PART_LineRightButton", _horizontalScrollBar);
                _horizontalScrollBarLeftButton.Click += (s, e) => { _horizontalScrollBar.Value -= _textMeasures.LetterWidth; InvalidateVisual(); };
                _horizontalScrollBarRightButton.Click += (s, e) => { _horizontalScrollBar.Value += _textMeasures.LetterWidth; InvalidateVisual(); };
            };
            _viewport = new Viewport(this, _verticalScrollBar, _horizontalScrollBar, _model.Text, _textMeasures);
        }

        protected override void OnRender(DrawingContext context)
        {
            context.DrawRectangle(Background ?? Brushes.White, null, new Rect(0, 0, ActualWidth, ActualHeight));
            // lexems
            var typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            var startLine = (int)(_verticalScrollBar!.Value / _textMeasures.LineHeight);
            var linesCount = _viewport!.GetLinesCountInViewport();
            var endLine = Math.Min(startLine + linesCount, _model.Text.LinesCount);
            var offsetY = -(_verticalScrollBar.Value % _textMeasures.LineHeight);
            for (var lineIndex = startLine; lineIndex < endLine; lineIndex++)
            {
                var offsetX = -_horizontalScrollBar!.Value;
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
            // selection
            foreach (var line in _model.TextSelection.GetTextSelectionLines(_model.Text))
            {
                DrawSelectionLine(context, line.LineIndex, line.LeftColumnIndex, line.RightColumnIndex);
            }
            // cursor
            var cursorAbsolutePoint = _model.TextCursor.GetAbsolutePosition(_textMeasures);
            cursorAbsolutePoint.X -= _horizontalScrollBar!.Value;
            cursorAbsolutePoint.Y -= _verticalScrollBar.Value;
            if (cursorAbsolutePoint.X >= 0 && cursorAbsolutePoint.Y >= 0)
            {
                context.DrawLine(CursorBlackPen,
                    new Point((int)cursorAbsolutePoint.X, (int)cursorAbsolutePoint.Y),
                    new Point((int)cursorAbsolutePoint.X, (int)(cursorAbsolutePoint.Y + _textMeasures.LineHeight)));
            }
        }

        private void DrawSelectionLine(DrawingContext context, int lineIndex, int leftColumnIndex, int rightColumnIndex)
        {
            var leftColumnPos = leftColumnIndex * _textMeasures.LetterWidth - _horizontalScrollBar!.Value;
            var rightColumnPos = rightColumnIndex * _textMeasures.LetterWidth - _horizontalScrollBar!.Value;
            context.DrawRectangle(
                SelectionBrush,
                null,
                new Rect(leftColumnPos, lineIndex * _textMeasures.LineHeight - _verticalScrollBar!.Value, rightColumnPos - leftColumnPos, _textMeasures.LineHeight));
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            _verticalScrollBar!.ViewportSize = sizeInfo.NewSize.Height;
            _horizontalScrollBar!.ViewportSize = sizeInfo.NewSize.Width;
            _viewport!.UpdateScrollbarsMaximumValues();
            InvalidateVisual();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Focus();
            var positionInControl = e.GetPosition(this);
            var lineIndex = _viewport!.GetCursorLineIndex(positionInControl);
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
                var lineIndex = _viewport!.GetCursorLineIndex(positionInControl);
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
            _verticalScrollBar!.Value -= e.Delta;
            InvalidateVisual();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            var needToInvalidate = true;
            if (e.Key == Key.Up)
            {
                _model.MoveCursorUp();
            }
            else if (e.Key == Key.Down)
            {
                _model.MoveCursorDown();
            }
            else if (e.Key == Key.Left)
            {
                _model.MoveCursorLeft();
            }
            else if (e.Key == Key.Right)
            {
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
                _model.MoveCursorPageUp(_viewport!.GetLinesCountInViewport());
            }
            else if (e.Key == Key.PageDown)
            {
                _model.MoveCursorPageDown(_viewport!.GetLinesCountInViewport());
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
            else if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                _model.StartSelection();
            }
            else if (e.Key == Key.A && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                _model.SelectAll();
            }
            else
            {
                needToInvalidate = false;
            }
            if (needToInvalidate)
            {
                _viewport!.CorrectViewport(_model.TextCursor.GetAbsolutePosition(_textMeasures));
                InvalidateVisual();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
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

        private static readonly HashSet<char> _notAllowedSymbols = new(new char[] { '\n', '\r', '\b' });
        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            var text = e.Text.Where(ch => !_notAllowedSymbols.Contains(ch)).ToList();
            if (!text.Any()) return;
            foreach (var ch in text)
            {
                _model.AppendChar(ch);
            }
            _viewport!.CorrectViewport(_model.TextCursor.GetAbsolutePosition(_textMeasures));
            InvalidateVisual();
        }

        private void OnUpdateCodeProvider()
        {
            _model.SetCodeProvider(CodeProvider);
            _viewport!.UpdateScrollbarsMaximumValues();
        }
    }
}
