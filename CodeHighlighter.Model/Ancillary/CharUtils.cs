namespace CodeHighlighter.Ancillary;

internal static class CharUtils
{
    public static bool IsCharEqualIgnoreCase(char a, char b)
    {
        var diff = a - b;

        if (diff == 0) return true;

        if (a < 128 && b < 128) // ascii
        {
            return diff == -32 || diff == 32;
        }

        return Char.ToUpperInvariant(a) == Char.ToUpperInvariant(b);
    }
}
