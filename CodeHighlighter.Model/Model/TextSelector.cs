using System.Collections.Generic;

namespace CodeHighlighter.Model;

public interface ITextSelector
{
    void ActivateSelection();
    void CompleteSelection();
    string GetSelectedText();
}

internal class TextSelector : ITextSelector
{
    private readonly IText _text;
    private readonly ITextCursor _textCursor;
    private readonly ITextSelection _textSelection;

    public TextSelector(IText text, ITextCursor textCursor, ITextSelection textSelection)
    {
        _text = text;
        _textCursor = textCursor;
        _textSelection = textSelection;
    }

    public void ActivateSelection()
    {
        if (!_textSelection.InProgress)
        {
            _textSelection.InProgress = true;
            _textSelection.StartPosition = _textCursor.Position;
            _textSelection.EndPosition = _textCursor.Position;
        }
    }

    public void CompleteSelection()
    {
        _textSelection.InProgress = false;
    }

    public string GetSelectedText()
    {
        if (!_textSelection.IsExist) return "";
        var selectedLines = new List<string>();
        foreach (var line in _textSelection.GetSelectedLines())
        {
            selectedLines.Add(_text.GetLine(line.LineIndex).GetSubstring(line.LeftColumnIndex, line.RightColumnIndex - line.LeftColumnIndex));
        }

        return String.Join(Environment.NewLine, selectedLines);
    }
}
