using System;
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
        private readonly CodeTextBoxModel _model;
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
            codeTextBox.OnUpdateCodeProvider();
            codeTextBox.InvalidateVisual();
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
            control._model.SetFontSettings(control.MakeFontSettings());
        }

        public CodeTextBox()
        {
            _model = new CodeTextBoxModel(MakeFontSettings());
            var template = new ControlTemplate(typeof(CodeTextBox));
            template.VisualTree = new FrameworkElementFactory(typeof(Grid), "RootLayout");
            template.VisualTree.AppendChild(new FrameworkElementFactory(typeof(ScrollBar), "VerticalScrollBar"));
            template.VisualTree.AppendChild(new FrameworkElementFactory(typeof(ScrollBar), "HorizontalScrollBar"));
            Template = template;
            Cursor = Cursors.IBeam;
            FocusVisualStyle = null;
        }

        private FontSettings MakeFontSettings() => new() { FontSize = FontSize, FontFamily = FontFamily, FontStyle = FontStyle, FontWeight = FontWeight, FontStretch = FontStretch };

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
                _verticalScrollBarUpButton.Click += (s, e) => { _verticalScrollBar.Value -= _model.TextMeasures.LineHeight; InvalidateVisual(); };
                _verticalScrollBarDownButton.Click += (s, e) => { _verticalScrollBar.Value += _model.TextMeasures.LineHeight; InvalidateVisual(); };
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
                _horizontalScrollBarLeftButton.Click += (s, e) => { _horizontalScrollBar.Value -= _model.TextMeasures.LetterWidth; InvalidateVisual(); };
                _horizontalScrollBarRightButton.Click += (s, e) => { _horizontalScrollBar.Value += _model.TextMeasures.LetterWidth; InvalidateVisual(); };
            };
            _model.Viewport = new Viewport(this, _verticalScrollBar, _horizontalScrollBar);
        }

        protected override void OnRender(DrawingContext context)
        {
            context.DrawRectangle(Background ?? Brushes.White, null, new Rect(0, 0, ActualWidth, ActualHeight));
            // lexems
            var typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            var startLine = (int)(_verticalScrollBar!.Value / _model.TextMeasures.LineHeight);
            var linesCount = _model.GetLinesCountInViewport();
            var endLine = Math.Min(startLine + linesCount, _model.Text.LinesCount);
            var offsetY = -(_verticalScrollBar.Value % _model.TextMeasures.LineHeight);
            for (var lineIndex = startLine; lineIndex < endLine; lineIndex++)
            {
                var lineLexems = _model.Lexems.GetLexemsForLine(lineIndex);
                if (!lineLexems.Any()) { offsetY += _model.TextMeasures.LineHeight; continue; }
                var offsetX = -_horizontalScrollBar!.Value;
                foreach (var lexem in lineLexems)
                {
                    var text = _model.Text.GetSubstring(lineIndex, lexem.ColumnIndex, lexem.Length);
                    var brush = _model.LexemColors.GetColorBrushOrNull(lexem.Kind) ?? Foreground;
                    var formattedText = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, FontSize, brush, 1.0);
                    context.DrawText(formattedText, new Point(offsetX, offsetY));
                    offsetX += formattedText.WidthIncludingTrailingWhitespace;
                }
                offsetY += _model.TextMeasures.LineHeight;
            }
            // cursor
            var cursorAbsolutePoint = _model.TextCursor.AbsolutePoint;
            cursorAbsolutePoint.X -= _horizontalScrollBar!.Value;
            cursorAbsolutePoint.Y -= _verticalScrollBar.Value;
            if (cursorAbsolutePoint.X >= 0 && cursorAbsolutePoint.Y >= 0)
            {
                context.DrawLine(TextCursor.BlackPen,
                    new Point((int)cursorAbsolutePoint.X, (int)cursorAbsolutePoint.Y),
                    new Point((int)cursorAbsolutePoint.X, (int)(cursorAbsolutePoint.Y + _model.TextMeasures.LineHeight)));
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            _verticalScrollBar!.ViewportSize = sizeInfo.NewSize.Height;
            _horizontalScrollBar!.ViewportSize = sizeInfo.NewSize.Width;
            _model.UpdateScrollbarsMaximumValues();
            InvalidateVisual();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Focus();
            var pos = e.GetPosition(this);
            _model.MoveCursorByClick(pos.X + _horizontalScrollBar!.Value, pos.Y + _verticalScrollBar!.Value);
            InvalidateVisual();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            _verticalScrollBar!.Value -= e.Delta;
            InvalidateVisual();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
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
            else if (e.Key == Key.Home && e.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                _model.MoveCursorHome();
            }
            else if (e.Key == Key.Home && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                _model.CursorGotoTextBegin();
            }
            else if (e.Key == Key.End && e.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                _model.MoveCursorEnd();
            }
            else if (e.Key == Key.End && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                _model.CursorGotoTextEnd();
            }
            else if (e.Key == Key.PageUp)
            {
                _model.MoveCursorPageUp();
            }
            else if (e.Key == Key.PageDown)
            {
                _model.MoveCursorPageDown();
            }

            _model.CorrectViewport();
            InvalidateVisual();
        }

        private void OnUpdateCodeProvider()
        {
            if (CodeProvider != null)
            {
                _model.UpdateLexems(CodeProvider);
                _model.UpdateScrollbarsMaximumValues();
            }
        }
    }
}
