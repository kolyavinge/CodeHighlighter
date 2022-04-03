using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using CodeHighlighter.Drawing;
using CodeHighlighter.TextProcessing;

namespace CodeHighlighter
{
    public class CodeTextBox : Control
    {
        private readonly FontSettings _fontSettings;
        private readonly Text _text;
        private readonly Lexems _lexems;
        private readonly LexemsColors _lexemColors;
        private readonly TextMeasures _textMeasures;
        private readonly TextCursor _textCursor;
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
            codeTextBox._text.SetText((string)e.NewValue);
            codeTextBox.UpdateCodeProvider();
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
            codeTextBox.UpdateCodeProvider();
            codeTextBox.InvalidateVisual();
        }
        #endregion

        static CodeTextBox()
        {
            FontSizeProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(FontSizePropertyChangedCallback));
            FontFamilyProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(FontFamilyPropertyChangedCallback));
            FontStyleProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(FontStylePropertyChangedCallback));
            FontWeightProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(FontWeightPropertyChangedCallback));
            FontStretchProperty.OverrideMetadata(typeof(CodeTextBox), new FrameworkPropertyMetadata(FontStretchPropertyChangedCallback));
        }

        private static void FontSizePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CodeTextBox)d;
            control._fontSettings.FontSize = (double)e.NewValue;
            control._textMeasures.UpdateMeasures();
        }

        private static void FontFamilyPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CodeTextBox)d;
            control._fontSettings.FontFamily = (FontFamily)e.NewValue;
            control._textMeasures.UpdateMeasures();
        }

        private static void FontStylePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CodeTextBox)d;
            control._fontSettings.FontStyle = (FontStyle)e.NewValue;
            control._textMeasures.UpdateMeasures();
        }

        private static void FontWeightPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CodeTextBox)d;
            control._fontSettings.FontWeight = (FontWeight)e.NewValue;
            control._textMeasures.UpdateMeasures();
        }

        private static void FontStretchPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CodeTextBox)d;
            control._fontSettings.FontStretch = (FontStretch)e.NewValue;
            control._textMeasures.UpdateMeasures();
        }

        public CodeTextBox()
        {
            _fontSettings = new();
            _fontSettings.FontSize = FontSize;
            _fontSettings.FontFamily = FontFamily;
            _fontSettings.FontStyle = FontStyle;
            _fontSettings.FontWeight = FontWeight;
            _fontSettings.FontStretch = FontStretch;
            _text = new();
            _lexems = new();
            _lexemColors = new();
            _textMeasures = new(_fontSettings);
            _textCursor = new(_text, _textMeasures);
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
            _viewport = new Viewport(this, _verticalScrollBar, _horizontalScrollBar);
        }

        protected override void OnRender(DrawingContext context)
        {
            context.DrawRectangle(Background ?? Brushes.White, Pens.Transparent, new Rect(0, 0, ActualWidth, ActualHeight));
            var lineHeight = _textMeasures.LineHeight;
            // lexems
            var typeface = MakeTypeface();
            var startLine = (int)(_verticalScrollBar!.Value / lineHeight);
            var linesCount = GetLinesCountInViewport();
            var endLine = Math.Min(startLine + linesCount, _text.LinesCount);
            var offsetY = -(_verticalScrollBar.Value % lineHeight);
            for (var lineIndex = startLine; lineIndex < endLine; lineIndex++)
            {
                var lineLexems = _lexems.GetLexemsForLine(lineIndex);
                if (!lineLexems.Any()) { offsetY += lineHeight; continue; }
                var offsetX = 0.0 - _horizontalScrollBar!.Value;
                foreach (var lexem in lineLexems)
                {
                    var text = _text.GetSubstring(lineIndex, lexem.ColumnIndex, lexem.Length);
                    var brush = _lexemColors.GetColorBrushOrNull(lexem.Kind) ?? Foreground;
                    var formattedText = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, FontSize, brush, 1.0);
                    context.DrawText(formattedText, new Point(offsetX, offsetY));
                    offsetX += formattedText.WidthIncludingTrailingWhitespace;
                }
                offsetY += lineHeight;
            }
            // cursor
            var cursorAbsolutePoint = _textCursor.AbsolutePoint;
            cursorAbsolutePoint.X -= _horizontalScrollBar!.Value;
            cursorAbsolutePoint.Y -= _verticalScrollBar.Value;
            if (cursorAbsolutePoint.X >= 0 && cursorAbsolutePoint.Y >= 0)
            {
                context.DrawLine(TextCursor.BlackPen,
                    new Point((int)cursorAbsolutePoint.X, (int)cursorAbsolutePoint.Y),
                    new Point((int)cursorAbsolutePoint.X, (int)(cursorAbsolutePoint.Y + lineHeight)));
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            _verticalScrollBar!.ViewportSize = sizeInfo.NewSize.Height;
            _horizontalScrollBar!.ViewportSize = sizeInfo.NewSize.Width;
            UpdateScrollbars();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Focus();
            var pos = e.GetPosition(this);
            _textCursor.MoveByClick(pos.X + _horizontalScrollBar!.Value, pos.Y + _verticalScrollBar!.Value);
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
                _textCursor.MoveByUp();
            }
            else if (e.Key == Key.Down)
            {
                _textCursor.MoveByDown();
            }
            else if (e.Key == Key.Left)
            {
                _textCursor.MoveByLeft();
            }
            else if (e.Key == Key.Right)
            {
                _textCursor.MoveByRight();
            }
            else if (e.Key == Key.Home && e.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                _textCursor.MoveByHome();
            }
            else if (e.Key == Key.Home && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                _textCursor.GotoTextBegin();
            }
            else if (e.Key == Key.End && e.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                _textCursor.MoveByEnd();
            }
            else if (e.Key == Key.End && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                _textCursor.GotoTextEnd();
            }
            else if (e.Key == Key.PageUp)
            {
                _textCursor.MoveByPageUp(GetLinesCountInViewport());
            }
            else if (e.Key == Key.PageDown)
            {
                _textCursor.MoveByPageDown(GetLinesCountInViewport());
            }

            CorrectViewport();
            InvalidateVisual();
        }

        private void CorrectViewport()
        {
            if (_textCursor.AbsolutePoint.X < _horizontalScrollBar!.Value)
            {
                _horizontalScrollBar.Value = _textCursor.AbsolutePoint.X;
            }
            else if (_textCursor.AbsolutePoint.X + _textMeasures.LetterWidth > _horizontalScrollBar.Value + _viewport!.Width)
            {
                _horizontalScrollBar.Value = _textCursor.AbsolutePoint.X - _viewport!.Width + _textMeasures.LetterWidth;
            }

            if (_textCursor.AbsolutePoint.Y < _verticalScrollBar!.Value)
            {
                _verticalScrollBar.Value = _textCursor.AbsolutePoint.Y;
            }
            else if (_textCursor.AbsolutePoint.Y + _textMeasures.LineHeight > _verticalScrollBar.Value + _viewport!.Height)
            {
                _verticalScrollBar.Value = _textCursor.AbsolutePoint.Y - _viewport!.Height + _textMeasures.LineHeight;
            }
        }

        private Typeface MakeTypeface() => new(FontFamily, FontStyle, FontWeight, FontStretch);

        private void UpdateCodeProvider()
        {
            if (CodeProvider != null)
            {
                _lexems.SetLexems(_text, CodeProvider.GetLexems(new TextIterator(_text)));
                _lexemColors.SetColors(CodeProvider.GetColors());
                UpdateScrollbars();
            }
        }

        private void UpdateScrollbars()
        {
            _verticalScrollBar!.Maximum = _text.LinesCount * _textMeasures.LineHeight;
            _horizontalScrollBar!.Maximum = _viewport!.Width < _text.GetMaxLineWidth() * _textMeasures.LetterWidth ? _text.GetMaxLineWidth() * _textMeasures.LetterWidth : 0;
        }

        private int GetLinesCountInViewport()
        {
            var result = (int)(ActualHeight / _textMeasures.LineHeight) + 1;
            if (ActualHeight % _textMeasures.LineHeight != 0) result++;

            return result;
        }
    }
}
