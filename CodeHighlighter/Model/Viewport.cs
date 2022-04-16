using System.Windows;

namespace CodeHighlighter.Model
{
    interface IScrollBarHolder
    {
        double VerticalScrollBarValue { get; set; }
        double VerticalScrollBarMaximum { get; set; }
        double VerticalScrollBarViewportSize { get; set; }
        double HorizontalScrollBarValue { get; set; }
        double HorizontalScrollBarMaximum { get; set; }
        double HorizontalScrollBarViewportSize { get; set; }
    }

    class Viewport
    {
        private readonly FrameworkElement _control;
        private readonly IScrollBarHolder _scrollbarHolder;
        private readonly IText _text;
        private readonly ITextMeasures _textMeasures;

        public Viewport(
            FrameworkElement control, IScrollBarHolder scrollbarHolder, IText text, ITextMeasures textMeasures)
        {
            _control = control;
            _scrollbarHolder = scrollbarHolder;
            _text = text;
            _textMeasures = textMeasures;
        }

        public int GetLinesCountInViewport()
        {
            var result = (int)(_control.ActualHeight / _textMeasures.LineHeight) + 1;
            if (_control.ActualHeight % _textMeasures.LineHeight != 0) result++;

            return result;
        }

        public int GetCursorLineIndex(Point cursorClickPosition)
        {
            return (int)((cursorClickPosition.Y + _scrollbarHolder.VerticalScrollBarValue) / _textMeasures.LineHeight);
        }

        public int CursorColumnIndex(Point cursorClickPosition)
        {
            return (int)((cursorClickPosition.X + _textMeasures.HalfLetterWidth + _scrollbarHolder.HorizontalScrollBarValue) / _textMeasures.LetterWidth);
        }

        public void CorrectViewport(Point cursorGetAbsolutePoint)
        {
            if (cursorGetAbsolutePoint.X < _scrollbarHolder.HorizontalScrollBarValue)
            {
                _scrollbarHolder.HorizontalScrollBarValue = cursorGetAbsolutePoint.X;
            }
            else if (cursorGetAbsolutePoint.X + _textMeasures.LetterWidth > _scrollbarHolder.HorizontalScrollBarValue + _control.ActualWidth)
            {
                _scrollbarHolder.HorizontalScrollBarValue = cursorGetAbsolutePoint.X - _control.ActualWidth + _textMeasures.LetterWidth;
            }

            if (cursorGetAbsolutePoint.Y < _scrollbarHolder.VerticalScrollBarValue)
            {
                _scrollbarHolder.VerticalScrollBarValue = cursorGetAbsolutePoint.Y;
            }
            else if (cursorGetAbsolutePoint.Y + _textMeasures.LineHeight > _scrollbarHolder.VerticalScrollBarValue + _control.ActualHeight)
            {
                _scrollbarHolder.VerticalScrollBarValue = cursorGetAbsolutePoint.Y - _control.ActualHeight + _textMeasures.LineHeight;
            }
        }

        public void UpdateScrollbarsMaximumValues()
        {
            var maxLineWidthInPixels = _text.GetMaxLineWidth() * _textMeasures.LetterWidth;
            _scrollbarHolder.HorizontalScrollBarMaximum = _control.ActualWidth < maxLineWidthInPixels ? maxLineWidthInPixels : 0;
            _scrollbarHolder.VerticalScrollBarMaximum = _text.LinesCount * _textMeasures.LineHeight;
        }
    }
}
