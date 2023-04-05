namespace CodeHighlighter.Ancillary;

public readonly struct TextPosition
{
    public readonly int StartLineIndex;
    public readonly int StartColumnIndex;
    public readonly int EndLineIndex;
    public readonly int EndColumnIndex;

    public TextPosition(int startLineIndex, int startColumnIndex, int endLineIndex, int endColumnIndex)
    {
        StartLineIndex = startLineIndex;
        StartColumnIndex = startColumnIndex;
        EndLineIndex = endLineIndex;
        EndColumnIndex = endColumnIndex;
    }
}
