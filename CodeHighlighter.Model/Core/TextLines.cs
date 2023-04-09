namespace CodeHighlighter.Core;

public interface ITextLines
{
    int Count { get; }

    ITextLine GetLine(int lineIndex);

    string ToString();
}

internal class TextLines : ITextLines
{
    private readonly IText _text;

    public int Count => _text.LinesCount;

    public ITextLine GetLine(int lineIndex) => _text.GetLine(lineIndex);

    public TextLines(IText text)
    {
        _text = text;
    }
}
