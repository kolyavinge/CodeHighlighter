using System.Windows;
using System.Windows.Controls.Primitives;

namespace CodeHighlighter.Model
{
    class Viewport
    {
        private readonly FrameworkElement _control;

        public ScrollBar VerticalScrollBar { get; }

        public ScrollBar HorizontalScrollBar { get; }

        public double Width => _control.ActualWidth - VerticalScrollBar.ActualWidth;

        public double Height => _control.ActualHeight - HorizontalScrollBar.ActualHeight;

        public Viewport(FrameworkElement control, ScrollBar verticalScrollBar, ScrollBar horizontalScrollBar)
        {
            _control = control;
            VerticalScrollBar = verticalScrollBar;
            HorizontalScrollBar = horizontalScrollBar;
        }
    }
}
