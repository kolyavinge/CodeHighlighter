using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class DeleteSelectedLinesHistoryAction : TextHistoryAction<DeleteSelectedLinesResult>
{
    public DeleteSelectedLinesHistoryAction(HistoryActionContext context) : base(context)
    {
    }

    public override bool Do()
    {
        _result = DeleteSelectedLinesInputAction.Instance.Do(_context);
        if (_result.HasDeleted || _result.OldCursorPosition.Kind == CursorPositionKind.Virtual) _context.CodeTextBox?.InvalidateVisual();

        return _result.HasDeleted;
    }

    public override void Undo()
    {
        ResetSelection();
        if (_result!.IsSelectionExist)
        {
            _context.InputModel.MoveCursorTo(new(_result!.SelectionStart.LineIndex, 0));
        }
        else
        {
            _context.InputModel.MoveCursorTo(new(_result!.OldCursorPosition.LineIndex, 0));
        }
        InsertTextInputAction.Instance.Do(_context, _result!.DeletedSelectedText);
        SetCursorToStartPosition();
        _context.CodeTextBox?.InvalidateVisual();
    }

    public override void Redo()
    {
        if (_result!.IsSelectionExist)
        {
            RestoreSelection();
        }
        else
        {
            ResetSelection();
            SetCursorToStartPosition();
        }
        DeleteSelectedLinesInputAction.Instance.Do(_context);
        _context.CodeTextBox?.InvalidateVisual();
    }
}
