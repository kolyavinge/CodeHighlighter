﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering;

internal class CursorRenderLogic
{
    private Line _cursorLine = new();

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

    public void DrawCursor(ITextCursor textCursor, TextMeasures textMeasures, IViewportContext viewportContext)
    {
        var cursorAbsolutePoint = textCursor.GetAbsolutePosition(textMeasures);
        cursorAbsolutePoint.X -= viewportContext.HorizontalScrollBarValue;
        cursorAbsolutePoint.Y -= viewportContext.VerticalScrollBarValue;
        _cursorLine.X1 = (int)cursorAbsolutePoint.X + 1;  // +1 - cursor not clipping on the left edge
        _cursorLine.Y1 = (int)(cursorAbsolutePoint.Y - 1);
        _cursorLine.X2 = (int)cursorAbsolutePoint.X;
        _cursorLine.Y2 = (int)(cursorAbsolutePoint.Y + textMeasures.LineHeight + 1);
    }

    public void HideCursor()
    {
        _cursorLine.X1 = 0;
        _cursorLine.Y1 = 0;
        _cursorLine.X2 = 0;
        _cursorLine.Y2 = 0;
    }

    public void DrawHighlightedCursorLine(
        DrawingContext context, Brush background, double actualWidth, ITextCursor textCursor, TextMeasures textMeasures, IViewportContext viewportContext)
    {
        var x = 0.0;
        var y = textCursor.LineIndex * textMeasures.LineHeight - viewportContext.VerticalScrollBarValue;
        var width = actualWidth;
        var height = textMeasures.LineHeight;
        context.DrawRectangle(background, null, new(x, y, width, height));
    }
}
