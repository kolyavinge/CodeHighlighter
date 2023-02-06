namespace CodeHighlighter.Common;

public readonly struct Rect
{
    public readonly double X;
    public readonly double Y;
    public readonly double Width;
    public readonly double Height;

    public Rect(double x, double y, double width, double height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public override string ToString()
    {
        return $"({X}:{Y}) ({Width}, {Height})";
    }
}
