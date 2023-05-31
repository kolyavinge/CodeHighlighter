namespace CodeHighlighter.Common;

public readonly record struct Point(double X, double Y)
{
    public override string ToString()
    {
        return $"{X}:{Y}";
    }
}
