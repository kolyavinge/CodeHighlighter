namespace CodeHighlighter.Core;

internal interface IViewportVerticalOffsetUpdater
{
    double GetVerticalOffsetAfterScrollLineUp(double verticalScrollBarValue, double lineHeight);
    double GetVerticalOffsetAfterScrollLineDown(double verticalScrollBarValue, double verticalScrollBarMaximum, double lineHeight);
}

internal class ViewportVerticalOffsetUpdater : IViewportVerticalOffsetUpdater
{
    public double GetVerticalOffsetAfterScrollLineUp(double verticalScrollBarValue, double lineHeight)
    {
        var offset = verticalScrollBarValue;
        var delta = verticalScrollBarValue % lineHeight;
        if (delta == 0) offset -= lineHeight;
        else offset -= delta;
        if (offset < 0) offset = 0;

        return offset;
    }

    public double GetVerticalOffsetAfterScrollLineDown(double verticalScrollBarValue, double verticalScrollBarMaximum, double lineHeight)
    {
        var offset = verticalScrollBarValue;
        var delta = verticalScrollBarValue % lineHeight;
        if (delta == 0) offset += lineHeight;
        else offset += delta;
        if (offset > verticalScrollBarMaximum) offset = verticalScrollBarMaximum;

        return offset;
    }
}
