using System;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class LeftDeleteHistoryAction : TextHistoryAction<DeleteResult>
{
    public LeftDeleteHistoryAction(HistoryActionContext context) : base(context)
    {
    }

    public override bool Do()
    {
        _result = LeftDeleteInputAction.Instance.Do(_context);
        if (_result.HasDeleted) _context.CodeTextBox?.InvalidateVisual();

        return _result.HasDeleted;
    }

    public override void Undo()
    {
        ResetSelection();
        SetCursorToEndPosition();
        var deletedSelectedText = !String.IsNullOrWhiteSpace(_result!.DeletedSelectedText) ? _result!.DeletedSelectedText : _result.CharCharDeleteResult.DeletedChar.ToString();
        InsertTextInputAction.Instance.Do(_context, deletedSelectedText);
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
        LeftDeleteInputAction.Instance.Do(_context);
        _context.CodeTextBox?.InvalidateVisual();
    }
}
