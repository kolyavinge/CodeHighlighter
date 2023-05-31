namespace CodeHighlighter.Common;

public readonly record struct Rect(double X, double Y, double Width, double Height)
{
    public override string ToString()
    {
        return $"({X}:{Y}) ({Width}, {Height})";
    }
}
