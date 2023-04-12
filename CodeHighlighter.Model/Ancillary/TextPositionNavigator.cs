using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Model;

namespace CodeHighlighter.Ancillary;

public interface ITextPositionNavigator
{
    void GotoPrev();
    void GotoNext();
}

internal interface ITextPositionNavigatorInternal : ITextPositionNavigator
{
    void SetPositions(IEnumerable<TextPosition> positions);
}

internal class TextPositionNavigator : ITextPositionNavigatorInternal
{
    private readonly ICodeTextBoxModel _codeTextBoxModel;
    private TextPosition[] _positions;

    public TextPositionNavigator(ICodeTextBoxModel codeTextBoxModel)
    {
        _codeTextBoxModel = codeTextBoxModel;
        _positions = Array.Empty<TextPosition>();
    }

    public void SetPositions(IEnumerable<TextPosition> positions)
    {
        _positions = positions.ToArray();
    }

    public void GotoPrev()
    {
        if (!_positions.Any()) return;
        var positionIndex = GetPrevIndex();
        if (positionIndex == -1) positionIndex = _positions.Length - 1;
        MoveCursorTo(positionIndex);
    }

    public void GotoNext()
    {
        if (!_positions.Any()) return;
        var positionIndex = GetNextIndex();
        if (positionIndex == -1) positionIndex = 0;
        MoveCursorTo(positionIndex);
    }

    private void MoveCursorTo(int positionIndex)
    {
        var position = _positions[positionIndex];
        _codeTextBoxModel.MoveCursorTo(new(position.StartLineIndex, position.StartColumnIndex));
        _codeTextBoxModel.Focus();
    }

    private int GetPrevIndex()
    {
        var position = _codeTextBoxModel.CursorPosition;
        var result = Array.FindLastIndex(_positions, p => p.StartLineIndex < position.LineIndex);
        if (result == -1) result = Array.FindLastIndex(_positions, p => p.StartLineIndex == position.LineIndex && p.StartColumnIndex < position.ColumnIndex);

        return result;
    }

    private int GetNextIndex()
    {
        var position = _codeTextBoxModel.CursorPosition;
        var result = Array.FindIndex(_positions, p => p.StartLineIndex > position.LineIndex);
        if (result == -1) result = Array.FindIndex(_positions, p => p.StartLineIndex == position.LineIndex && p.StartColumnIndex > position.ColumnIndex);

        return result;
    }
}
