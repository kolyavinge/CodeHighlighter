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
        private ScrollBar? _verticalScrollBar;
        private RepeatButton? _verticalScrollBarUpButton, _verticalScrollBarDownButton;
        private RepeatButton? _horizontalScrollBarLeftButton, _horizontalScrollBarRightButton;
        private ScrollBar? _horizontalScrollBar;
        private Text _text;
        private Lexems _lexems;
        private LexemsColors _lexemColors;

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
            codeTextBox._text = new Text((string)e.NewValue);
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

        public CodeTextBox()
        {
            _text = new Text("");
            _lexems = new Lexems();
            _lexemColors = new LexemsColors();
            var template = new ControlTemplate(typeof(CodeTextBox));
            template.VisualTree = new FrameworkElementFactory(typeof(Grid), "RootLayout");
            template.VisualTree.AppendChild(new FrameworkElementFactory(typeof(ScrollBar), "VerticalScrollBar"));
            template.VisualTree.AppendChild(new FrameworkElementFactory(typeof(ScrollBar), "HorizontalScrollBar"));
            Template = template;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            // VerticalScrollBar
            _verticalScrollBar = (ScrollBar)Template.FindName("VerticalScrollBar", this);
            _verticalScrollBar.Minimum = 0;
            _verticalScrollBar.Orientation = Orientation.Vertical;
            _verticalScrollBar.HorizontalAlignment = HorizontalAlignment.Right;
            _verticalScrollBar.Scroll += (s, e) => InvalidateVisual();
            _verticalScrollBar.Loaded += (s, e) =>
            {
                _verticalScrollBarUpButton = (RepeatButton)_verticalScrollBar.Template.FindName("PART_LineUpButton", _verticalScrollBar);
                _verticalScrollBarDownButton = (RepeatButton)_verticalScrollBar.Template.FindName("PART_LineDownButton", _verticalScrollBar);
                _verticalScrollBarUpButton.Click += (s, e) => { _verticalScrollBar.Value -= GetLineHeight(); InvalidateVisual(); };
                _verticalScrollBarDownButton.Click += (s, e) => { _verticalScrollBar.Value += GetLineHeight(); InvalidateVisual(); };
            };
            // HorizontalScrollBar
            _horizontalScrollBar = (ScrollBar)Template.FindName("HorizontalScrollBar", this);
            _horizontalScrollBar.Minimum = 0;
            _horizontalScrollBar.Orientation = Orientation.Horizontal;
            _horizontalScrollBar.VerticalAlignment = VerticalAlignment.Bottom;
            _horizontalScrollBar.Margin = new Thickness(0, 0, _verticalScrollBar.Width, 0);
            _horizontalScrollBar.Scroll += (s, e) => InvalidateVisual();
            _horizontalScrollBar.Loaded += (s, e) =>
            {
                _horizontalScrollBarLeftButton = (RepeatButton)_horizontalScrollBar.Template.FindName("PART_LineLeftButton", _horizontalScrollBar);
                _horizontalScrollBarRightButton = (RepeatButton)_horizontalScrollBar.Template.FindName("PART_LineRightButton", _horizontalScrollBar);
                _horizontalScrollBarLeftButton.Click += (s, e) => { _horizontalScrollBar.Value -= GetLetterWidth(); InvalidateVisual(); };
                _horizontalScrollBarRightButton.Click += (s, e) => { _horizontalScrollBar.Value += GetLetterWidth(); InvalidateVisual(); };
            };
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Background, Pens.TransparentPen, new Rect(0, 0, ActualWidth, ActualHeight));
            var lineHeight = GetLineHeight();
            var typeface = MakeTypeface();
            var startLine = (int)(_verticalScrollBar!.Value / lineHeight);
            var linesCount = (int)(ActualHeight / lineHeight) + 1;
            var endLine = Math.Min(startLine + linesCount, _text.LinesCount);
            var offsetY = -(_verticalScrollBar.Value % lineHeight);
            for (var lineIndex = startLine; lineIndex < endLine; lineIndex++)
            {
                var lineLexems = _lexems.GetLexemsForLine(lineIndex);
                if (!lineLexems.Any()) { offsetY += lineHeight; continue; }
                var offsetX = 8.0 - _horizontalScrollBar!.Value;
                foreach (var lexem in lineLexems)
                {
                    var text = _text.GetSubstring(lineIndex, lexem.ColumnIndex, lexem.Length);
                    var brush = _lexemColors.GetColorBrushOrNull(lexem.Kind) ?? Foreground;
                    var formattedText = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, FontSize, brush, 1.0);
                    drawingContext.DrawText(formattedText, new Point(offsetX, offsetY));
                    offsetX += formattedText.WidthIncludingTrailingWhitespace;
                }
                offsetY += lineHeight;
            }
            _verticalScrollBar.Maximum = _text.LinesCount * lineHeight;
            _horizontalScrollBar!.Maximum = ActualWidth < _text.GetMaxLineWidth() * GetLetterWidth() ? _text.GetMaxLineWidth() * GetLetterWidth() : 0;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            _verticalScrollBar!.ViewportSize = sizeInfo.NewSize.Height;
            _horizontalScrollBar!.ViewportSize = sizeInfo.NewSize.Width;
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            _verticalScrollBar!.Value -= e.Delta;
            InvalidateVisual();
        }

        private double GetLineHeight()
        {
            var formattedText = new FormattedText("A", CultureInfo.InvariantCulture, FlowDirection.LeftToRight, MakeTypeface(), FontSize, Foreground, 1.0);
            return formattedText.Extent;
        }

        private double GetLetterWidth()
        {
            var formattedText = new FormattedText("A", CultureInfo.InvariantCulture, FlowDirection.LeftToRight, MakeTypeface(), FontSize, Foreground, 1.0);
            return formattedText.Width;
        }

        private Typeface MakeTypeface() => new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);

        private void UpdateCodeProvider()
        {
            if (CodeProvider != null)
            {
                _lexems.SetLexems(_text, CodeProvider.GetLexems(new TextIterator(_text)));
                _lexemColors.SetColors(CodeProvider.GetColors());
            }
        }
    }
}
