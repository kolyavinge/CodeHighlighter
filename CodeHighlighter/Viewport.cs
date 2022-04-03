using System.Windows;
using System.Windows.Controls.Primitives;

namespace CodeHighlighter
{
    class Viewport
    {
        private readonly FrameworkElement _control;
        private readonly ScrollBar _verticalScrollBar;
        private readonly ScrollBar _horizontalScrollBar;

        public Viewport(FrameworkElement control, ScrollBar verticalScrollBar, ScrollBar horizontalScrollBar)
        {
            _control = control;
            _verticalScrollBar = verticalScrollBar;
            _horizontalScrollBar = horizontalScrollBar;
        }

        public double Width => _control.ActualWidth - _verticalScrollBar.ActualWidth;

        public double Height => _control.ActualHeight - _horizontalScrollBar.ActualHeight;
    }
}
