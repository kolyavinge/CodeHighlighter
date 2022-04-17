using System.Windows;

namespace CodeHighlighter.Model
{
    interface IViewportContext
    {
        double ActualWidth { get; }
        double ActualHeight { get; }
        double VerticalScrollBarValue { get; set; }
        double VerticalScrollBarMaximum { get; set; }
        double HorizontalScrollBarValue { get; set; }
        double HorizontalScrollBarMaximum { get; set; }
    }

    class Viewport
    {
        private readonly IViewportContext _context;
        private readonly IText _text;
        private readonly ITextMeasures _textMeasures;

        public Viewport(IViewportContext context, IText text, ITextMeasures textMeasures)
        {
            _context = context;
            _text = text;
            _textMeasures = textMeasures;
        }

        public int GetLinesCountInViewport()
        {
            var result = (int)(_context.ActualHeight / _textMeasures.LineHeight) + 1;
            if (_context.ActualHeight % _textMeasures.LineHeight != 0) result++;

            return result;
        }

        public int GetCursorLineIndex(Point cursorClickPosition)
        {
            return (int)((cursorClickPosition.Y + _context.VerticalScrollBarValue) / _textMeasures.LineHeight);
        }

        public int CursorColumnIndex(Point cursorClickPosition)
        {
            return (int)((cursorClickPosition.X + _textMeasures.HalfLetterWidth + _context.HorizontalScrollBarValue) / _textMeasures.LetterWidth);
        }

        public void CorrectViewport(Point cursorGetAbsolutePoint)
        {
            if (cursorGetAbsolutePoint.X < _context.HorizontalScrollBarValue)
            {
                _context.HorizontalScrollBarValue = cursorGetAbsolutePoint.X;
            }
            else if (cursorGetAbsolutePoint.X + _textMeasures.LetterWidth > _context.HorizontalScrollBarValue + _context.ActualWidth)
            {
                _context.HorizontalScrollBarValue = cursorGetAbsolutePoint.X - _context.ActualWidth + _textMeasures.LetterWidth;
            }

            if (cursorGetAbsolutePoint.Y < _context.VerticalScrollBarValue)
            {
                _context.VerticalScrollBarValue = cursorGetAbsolutePoint.Y;
            }
            else if (cursorGetAbsolutePoint.Y + _textMeasures.LineHeight > _context.VerticalScrollBarValue + _context.ActualHeight)
            {
                _context.VerticalScrollBarValue = cursorGetAbsolutePoint.Y - _context.ActualHeight + _textMeasures.LineHeight;
            }
        }

        public void UpdateScrollbarsMaximumValues()
        {
            var maxLineWidthInPixels = _text.GetMaxLineWidth() * _textMeasures.LetterWidth;
            _context.HorizontalScrollBarMaximum = _context.ActualWidth < maxLineWidthInPixels ? maxLineWidthInPixels : 0;
            _context.VerticalScrollBarMaximum = _text.LinesCount * _textMeasures.LineHeight;
        }
    }
}
