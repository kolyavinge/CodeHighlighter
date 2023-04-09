namespace CodeHighlighter.Ancillary;

internal static class CharUtils
{
    public static bool IsCharEqualIgnoreCase(char a, char b)
    {
        var diff = a - b;

        if (diff == 0) return true;

        if (a < 128 && b < 128) // ascii
        {
            if ((65 <= a && a <= 90 || 97 <= a && a <= 122) &&
                (65 <= b && b <= 90 || 97 <= b && b <= 122)) // english alphabet
            {
                return diff == -32 || diff == 32;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return Char.ToUpperInvariant(a) == Char.ToUpperInvariant(b);
        }
    }
}
