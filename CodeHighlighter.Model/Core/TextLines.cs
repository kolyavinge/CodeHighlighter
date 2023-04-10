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

    public TextLines(IText text)
    {
        _text = text;
    }

    public ITextLine GetLine(int lineIndex) => _text.GetLine(lineIndex);

    public override string ToString() => _text.ToString();
}
