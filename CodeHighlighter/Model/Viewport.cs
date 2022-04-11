using System.Windows;
using System.Windows.Controls.Primitives;

namespace CodeHighlighter.Model
{
    class Viewport
    {
        private readonly FrameworkElement _control;
        private readonly IText _text;
        private readonly ITextMeasures _textMeasures;

        public ScrollBar VerticalScrollBar { get; }

        public ScrollBar HorizontalScrollBar { get; }

        public double Width => _control.ActualWidth - VerticalScrollBar.ActualWidth;

        public double Height => _control.ActualHeight - HorizontalScrollBar.ActualHeight;

        public Viewport(
            FrameworkElement control, ScrollBar verticalScrollBar, ScrollBar horizontalScrollBar, IText text, ITextMeasures textMeasures)
        {
            _control = control;
            VerticalScrollBar = verticalScrollBar;
            HorizontalScrollBar = horizontalScrollBar;
            _text = text;
            _textMeasures = textMeasures;
        }

        public int GetLinesCountInViewport()
        {
            var result = (int)(Height / _textMeasures.LineHeight) + 1;
            if (Height % _textMeasures.LineHeight != 0) result++;

            return result;
        }

        public int GetCursorLineIndex(Point cursorClickPosition)
        {
            return (int)((cursorClickPosition.Y + VerticalScrollBar!.Value) / _textMeasures.LineHeight);
        }

        public int CursorColumnIndex(Point cursorClickPosition)
        {
            return (int)((cursorClickPosition.X + HorizontalScrollBar!.Value) / _textMeasures.LetterWidth);
        }

        public void CorrectViewport(Point cursorGetAbsolutePoint)
        {
            if (cursorGetAbsolutePoint.X < HorizontalScrollBar!.Value)
            {
                HorizontalScrollBar.Value = cursorGetAbsolutePoint.X;
            }
            else if (cursorGetAbsolutePoint.X + _textMeasures.LetterWidth > HorizontalScrollBar.Value + Width)
            {
                HorizontalScrollBar.Value = cursorGetAbsolutePoint.X - Width + _textMeasures.LetterWidth;
            }

            if (cursorGetAbsolutePoint.Y < VerticalScrollBar!.Value)
            {
                VerticalScrollBar.Value = cursorGetAbsolutePoint.Y;
            }
            else if (cursorGetAbsolutePoint.Y + _textMeasures.LineHeight > VerticalScrollBar.Value + Height)
            {
                VerticalScrollBar.Value = cursorGetAbsolutePoint.Y - Height + _textMeasures.LineHeight;
            }
        }

        public void UpdateScrollbarsMaximumValues()
        {
            var maxLineWidthInPixels = _text.GetMaxLineWidth() * _textMeasures.LetterWidth;
            HorizontalScrollBar!.Maximum = Width < maxLineWidthInPixels ? maxLineWidthInPixels : 0;
            VerticalScrollBar!.Maximum = _text.LinesCount * _textMeasures.LineHeight;
        }
    }
}
