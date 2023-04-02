using CodeHighlighter.Core;

namespace CodeHighlighter.Ancillary;

public interface ICodeTextBoxModelAdditionalInfo
{
    int TextMaxLineWidth { get; }
}

internal class CodeTextBoxModelAdditionalInfo : ICodeTextBoxModelAdditionalInfo
{
    private readonly IText _text;

    public CodeTextBoxModelAdditionalInfo(IText text)
    {
        _text = text;
    }

    public int TextMaxLineWidth => _text.GetMaxLineWidth();
}
