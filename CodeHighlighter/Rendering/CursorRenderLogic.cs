using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering;

internal class CursorRenderLogic
{
    private readonly ITextCursor _textCursor;
    private readonly TextMeasures _textMeasures;
    private readonly IViewportContext _viewportContext;
    private Line? _cursorLine;

    public CursorRenderLogic(ITextCursor textCursor, TextMeasures textMeasures, IViewportContext viewportContext)
    {
        _textCursor = textCursor;
        _textMeasures = textMeasures;
        _viewportContext = viewportContext;
    }

    public void SetCursor(Line cursorLine, Brush foreground)
    {
        _cursorLine = cursorLine;
        _cursorLine.SnapsToDevicePixels = true;
        _cursorLine.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
        _cursorLine.Stroke = foreground;
        _cursorLine.StrokeThickness = 1.0;
        var animation = new ObjectAnimationUsingKeyFrames();
        animation.Duration = TimeSpan.FromSeconds(1.5);
        animation.RepeatBehavior = RepeatBehavior.Forever;
        animation.KeyFrames.Add(new DiscreteObjectKeyFrame(Visibility.Hidden, new TimeSpan(0, 0, 0, 0, 500)));
        animation.KeyFrames.Add(new DiscreteObjectKeyFrame(Visibility.Visible, new TimeSpan(0, 0, 1)));
        _cursorLine.BeginAnimation(Line.VisibilityProperty, animation);
    }

    public void DrawCursor()
    {
        var cursorAbsolutePoint = _textCursor.GetAbsolutePosition(_textMeasures);
        cursorAbsolutePoint.X -= _viewportContext.HorizontalScrollBarValue;
        cursorAbsolutePoint.Y -= _viewportContext.VerticalScrollBarValue;
        _cursorLine!.X1 = (int)cursorAbsolutePoint.X;
        _cursorLine!.Y1 = (int)(cursorAbsolutePoint.Y - 1);
        _cursorLine!.X2 = (int)cursorAbsolutePoint.X;
        _cursorLine!.Y2 = (int)(cursorAbsolutePoint.Y + _textMeasures.LineHeight + 1);
    }

    public void HideCursor()
    {
        _cursorLine!.X1 = 0;
        _cursorLine!.Y1 = 0;
        _cursorLine!.X2 = 0;
        _cursorLine!.Y2 = 0;
    }

    public void DrawHighlightedCursorLine(DrawingContext context, Brush background, double actualWidth)
    {
        var x = 0.0;
        var y = _textCursor.LineIndex * _textMeasures.LineHeight - _viewportContext.VerticalScrollBarValue;
        var width = actualWidth;
        var height = _textMeasures.LineHeight;
        context.DrawRectangle(background, null, new(x, y, width, height));
    }
}
