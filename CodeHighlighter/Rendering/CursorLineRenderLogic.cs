using System.Windows.Media;
using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering
{
    internal class CursorLineRenderLogic
    {
        private readonly ITextCursor _textCursor;
        private readonly TextMeasures _textMeasures;
        private readonly IViewportContext _viewportContext;

        public CursorLineRenderLogic(ITextCursor textCursor, TextMeasures textMeasures, IViewportContext viewportContext)
        {
            _textCursor = textCursor;
            _textMeasures = textMeasures;
            _viewportContext = viewportContext;
        }

        public void DrawCursorLine(DrawingContext context, Brush background, double actualWidth)
        {
            var x = 0.0;
            var y = _textCursor.LineIndex * _textMeasures.LineHeight - _viewportContext.VerticalScrollBarValue;
            var width = actualWidth;
            var height = _textMeasures.LineHeight;

            context.DrawRectangle(background, null, new(x, y, width, height));
        }
    }
}
