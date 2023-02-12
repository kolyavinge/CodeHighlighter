using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering;

internal class CursorRenderLogic
{
    private static readonly double _cursorThickness = 1.0;
    private readonly ObjectAnimationUsingKeyFrames _animation;
    private Line _cursorLine = new();

    public CursorRenderLogic()
    {
        _animation = new ObjectAnimationUsingKeyFrames();
        _animation.Duration = TimeSpan.FromSeconds(1.5);
        _animation.RepeatBehavior = RepeatBehavior.Forever;
        _animation.KeyFrames.Add(new DiscreteObjectKeyFrame(Visibility.Hidden, new TimeSpan(0, 0, 0, 0, 500)));
        _animation.KeyFrames.Add(new DiscreteObjectKeyFrame(Visibility.Visible, new TimeSpan(0, 0, 1)));
    }

    public void SetCursor(Line cursorLine, Brush foreground)
    {
        _cursorLine = cursorLine;
        _cursorLine.SnapsToDevicePixels = true;
        _cursorLine.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
        _cursorLine.Stroke = foreground;
        _cursorLine.StrokeThickness = _cursorThickness;
        _cursorLine.BeginAnimation(Line.VisibilityProperty, _animation);
    }

    public void DrawCursor(ICodeTextBoxModel model)
    {
        var cursorAbsolutePoint = model.AbsoluteCursorPosition;
        cursorAbsolutePoint.X -= model.Viewport.HorizontalScrollBarValue;
        cursorAbsolutePoint.Y -= model.Viewport.VerticalScrollBarValue;
        _cursorLine.X1 = (int)cursorAbsolutePoint.X + _cursorThickness;  // cursor is not cropped to the left
        _cursorLine.Y1 = (int)(cursorAbsolutePoint.Y - 1);
        _cursorLine.X2 = (int)cursorAbsolutePoint.X;
        _cursorLine.Y2 = (int)(cursorAbsolutePoint.Y + model.TextMeasures.LineHeight + 1);
    }

    public void HideCursor()
    {
        _cursorLine.X1 = 0;
        _cursorLine.Y1 = 0;
        _cursorLine.X2 = 0;
        _cursorLine.Y2 = 0;
    }

    public void ResetAnimation()
    {
        _cursorLine.BeginAnimation(Line.VisibilityProperty, null);
        _cursorLine.BeginAnimation(Line.VisibilityProperty, _animation);
    }

    public void DrawHighlightedCursorLine(ICodeTextBoxModel model, DrawingContext context, Brush background, double actualWidth)
    {
        var x = 0.0;
        var y = model.AbsoluteCursorPosition.Y - model.Viewport.VerticalScrollBarValue;
        var width = actualWidth;
        var height = model.TextMeasures.LineHeight;
        context.DrawRectangle(background, null, new(x, y, width, height));
    }
}
