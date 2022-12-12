namespace CodeHighlighter.Common;

public readonly struct Color
{
    public readonly byte R;
    public readonly byte G;
    public readonly byte B;
    public readonly byte A;

    public Color(byte r, byte g, byte b, byte a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public Color(byte r, byte g, byte b) : this(r, g, b, 255)
    {
    }
}
