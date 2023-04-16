using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Core;

namespace CodeHighlighter.Ancillary;

internal interface IWholeWordLogic
{
    IEnumerable<TextPosition> GetResult(IEnumerable<TextPosition> positions);
}

internal class WholeWordLogic : IWholeWordLogic
{
    private readonly ITextLines _textLines;

    public WholeWordLogic(ITextLines textLines)
    {
        _textLines = textLines;
    }

    public IEnumerable<TextPosition> GetResult(IEnumerable<TextPosition> positions)
    {
        return positions.Where(IsWholeWord);
    }

    private bool IsWholeWord(TextPosition p)
    {
        return
            (p.StartColumnIndex == 0 || IsTerminal(_textLines.GetLine(p.StartLineIndex)[p.StartColumnIndex - 1])) &&
            (p.EndColumnIndex == _textLines.GetLine(p.EndLineIndex).Length || IsTerminal(_textLines.GetLine(p.EndLineIndex)[p.EndColumnIndex]));
    }

    public bool IsTerminal(char c)
    {
        return !(Char.IsLetterOrDigit(c) || c == '_');
    }
}
