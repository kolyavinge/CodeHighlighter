namespace CodeHighlighter.Common;

public readonly struct Color
{
    public static Color FromHex(string hex)
    {
        hex = hex.Trim().Replace("#", "");
        if (hex.Length == 6)
        {
            var r = Convert.ToByte(hex.Substring(0, 2), 16);
            var g = Convert.ToByte(hex.Substring(2, 2), 16);
            var b = Convert.ToByte(hex.Substring(4, 2), 16);
            return new(r, g, b);
        }
        else if (hex.Length == 8)
        {
            var a = Convert.ToByte(hex.Substring(0, 2), 16);
            var r = Convert.ToByte(hex.Substring(2, 2), 16);
            var g = Convert.ToByte(hex.Substring(4, 2), 16);
            var b = Convert.ToByte(hex.Substring(6, 2), 16);
            return new(a, r, g, b);
        }
        else
        {
            throw new ArgumentException("Incorrect hex value.");
        }
    }

    public readonly byte A;
    public readonly byte R;
    public readonly byte G;
    public readonly byte B;

    public Color(byte a, byte r, byte g, byte b)
    {
        A = a;
        R = r;
        G = g;
        B = b;
    }

    public Color(byte r, byte g, byte b) : this(255, r, g, b)
    {
    }
}
