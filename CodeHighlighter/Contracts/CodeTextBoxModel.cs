namespace CodeHighlighter.Contracts;

public class CodeTextBoxModel
{
    public IText Text { get; }

    public ITextCursor TextCursor { get; }

    public ITextMeasures TextMeasures { get; }

    public CodeTextBoxModel(IText text, ITextCursor textCursor, ITextMeasures textMeasures)
    {
        Text = text;
        TextCursor = textCursor;
        TextMeasures = textMeasures;
    }
}
