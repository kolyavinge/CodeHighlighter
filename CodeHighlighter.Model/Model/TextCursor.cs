namespace CodeHighlighter.Model;

public enum CursorPositionKind { Real, Virtual }

public readonly struct CursorPosition
{
    public readonly int LineIndex;
    public readonly int ColumnIndex;
    public readonly CursorPositionKind Kind;

    public CursorPosition(int lineIndex, int columnIndex)
    {
        LineIndex = lineIndex;
        ColumnIndex = columnIndex;
        Kind = CursorPositionKind.Real;
    }

    internal CursorPosition(int lineIndex, int columnIndex, CursorPositionKind kind)
    {
        LineIndex = lineIndex;
        ColumnIndex = columnIndex;
        Kind = kind;
    }

    public override string ToString() => Kind == CursorPositionKind.Real ? $"{LineIndex}:{ColumnIndex}" : $"[{LineIndex}:{ColumnIndex}]";
}

internal interface ITextCursor
{
    int LineIndex { get; }
    int ColumnIndex { get; }
    CursorPositionKind Kind { get; set; }
    CursorPosition Position { get; }
    void MoveDown();
    void MoveEndLine();
    void MoveLeft();
    void MovePageDown(int pageSize);
    void MovePageUp(int pageSize);
    void MoveRight();
    void MoveStartLine();
    void MoveTextBegin();
    void MoveTextEnd();
    void MoveTo(CursorPosition position);
    void MoveUp();
}

internal class TextCursor : ITextCursor
{
    private readonly IText _text;
    private readonly ITextCursorPositionCorrector _corrector;
    private int _lineIndex;
    private int _columnIndex;
    private CursorPositionKind _kind;

    public int LineIndex => _lineIndex;

    public int ColumnIndex => _columnIndex;

    public CursorPositionKind Kind
    {
        get => _kind;
        set => _kind = value;
    }

    public CursorPosition Position => new(_lineIndex, _columnIndex, _kind);

    public TextCursor(IText text, ITextCursorPositionCorrector corrector)
    {
        _text = text;
        _corrector = corrector;
        _lineIndex = 0;
    }

    public void MoveTo(CursorPosition position)
    {
        _lineIndex = position.LineIndex;
        _columnIndex = position.ColumnIndex;
        _kind = position.Kind;
        _corrector.CorrectPosition(ref _lineIndex, ref _columnIndex, ref _kind);
    }

    public void MoveUp()
    {
        _lineIndex--;
        _corrector.SkipFoldedLinesUp(ref _lineIndex);
        _corrector.CorrectPosition(ref _lineIndex, ref _columnIndex, ref _kind);
    }

    public void MoveDown()
    {
        _lineIndex++;
        _corrector.SkipFoldedLinesDown(ref _lineIndex);
        _corrector.CorrectPosition(ref _lineIndex, ref _columnIndex, ref _kind);
    }

    public void MoveLeft()
    {
        if (LineIndex == 0 && ColumnIndex == 0) return;
        if (Kind == CursorPositionKind.Real)
        {
            _columnIndex--;
            if (ColumnIndex == -1)
            {
                _lineIndex--;
                _columnIndex = int.MaxValue;
            }
        }
        else
        {
            _columnIndex = 0;
        }
        _corrector.CorrectPosition(ref _lineIndex, ref _columnIndex, ref _kind);
    }

    public void MoveRight()
    {
        if (LineIndex == _text.LinesCount - 1 && ColumnIndex == _text.GetLine(LineIndex).Length) return;
        if (Kind == CursorPositionKind.Real) _columnIndex++;
        if (Kind == CursorPositionKind.Virtual || ColumnIndex == _text.GetLine(LineIndex).Length + 1)
        {
            _lineIndex++;
            _columnIndex = 0;
        }
        _corrector.CorrectPosition(ref _lineIndex, ref _columnIndex, ref _kind);
    }

    public void MoveStartLine()
    {
        var line = _text.GetLine(LineIndex);
        var spacesCount = line.FindIndex(0, line.Length, ch => ch != ' ');
        if (spacesCount == -1) _columnIndex = 0;
        else if (ColumnIndex > spacesCount) _columnIndex = spacesCount;
        else if (ColumnIndex == spacesCount) _columnIndex = 0;
        else _columnIndex = spacesCount;
        _corrector.CorrectPosition(ref _lineIndex, ref _columnIndex, ref _kind);
    }

    public void MoveEndLine()
    {
        _columnIndex = Int32.MaxValue;
        _corrector.CorrectPosition(ref _lineIndex, ref _columnIndex, ref _kind);
    }

    public void MovePageUp(int pageSize)
    {
        _lineIndex -= pageSize;
        _corrector.SkipFoldedLinesUp(ref _lineIndex);
        _corrector.CorrectPosition(ref _lineIndex, ref _columnIndex, ref _kind);
    }

    public void MovePageDown(int pageSize)
    {
        _lineIndex += pageSize;
        _corrector.SkipFoldedLinesDown(ref _lineIndex);
        _corrector.CorrectPosition(ref _lineIndex, ref _columnIndex, ref _kind);
    }

    public void MoveTextBegin()
    {
        _columnIndex = 0;
        _lineIndex = 0;
        _corrector.CorrectPosition(ref _lineIndex, ref _columnIndex, ref _kind);
    }

    public void MoveTextEnd()
    {
        _lineIndex = _text.LinesCount - 1;
        _columnIndex = _text.GetLine(LineIndex).Length;
        _corrector.CorrectPosition(ref _lineIndex, ref _columnIndex, ref _kind);
    }
}
