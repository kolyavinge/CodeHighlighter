using System.Linq;

namespace CodeHighlighter.Model;

internal interface ITextCursorPositionCorrector
{
    void CorrectPosition(ref int lineIndex, ref int columnIndex, ref CursorPositionKind kind);
    void SkipFoldedLinesUp(ref int lineIndex);
    void SkipFoldedLinesDown(ref int lineIndex);
}

internal class TextCursorPositionCorrector : ITextCursorPositionCorrector
{
    private readonly IText _text;
    private readonly ILineFolds _folds;

    public TextCursorPositionCorrector(IText text, ILineFolds folds)
    {
        _text = text;
        _folds = folds;
    }

    public void CorrectPosition(ref int lineIndex, ref int columnIndex, ref CursorPositionKind kind)
    {
        if (lineIndex < 0) lineIndex = 0;
        else if (lineIndex >= _text.LinesCount) lineIndex = _text.LinesCount - 1;
        var lineLength = _text.GetLine(lineIndex).Length;
        kind = CursorPositionKind.Real;
        if (lineLength == 0 && columnIndex > 0 && lineIndex > 0)
        {
            var prevLineIndex = lineIndex - 1;
            var prevLine = _text.GetLine(prevLineIndex);
            while (!prevLine.Any() && prevLineIndex > 0) prevLine = _text.GetLine(--prevLineIndex);
            var spacesCount = prevLine.FindIndex(0, prevLine.Length, ch => ch != ' ');
            if (spacesCount != -1)
            {
                columnIndex = spacesCount;
                if (columnIndex > 0) kind = CursorPositionKind.Virtual;
            }
            else
            {
                columnIndex = 0;
            }
        }
        else
        {
            if (columnIndex < 0) columnIndex = 0;
            else if (columnIndex > lineLength) columnIndex = lineLength;
        }
    }

    public void SkipFoldedLinesUp(ref int lineIndex)
    {
        if (_folds.IsFolded(lineIndex))
        {
            lineIndex = _folds.GetUnfoldedLineIndexUp(lineIndex);
        }
    }

    public void SkipFoldedLinesDown(ref int lineIndex)
    {
        if (_folds.IsFolded(lineIndex))
        {
            lineIndex = _folds.GetUnfoldedLineIndexDown(lineIndex);
        }
    }
}
