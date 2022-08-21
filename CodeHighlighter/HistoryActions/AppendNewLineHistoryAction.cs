using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class AppendNewLineHistoryAction : TextHistoryAction<AppendNewLineResult>
{
    public AppendNewLineHistoryAction(HistoryActionContext context) : base(context)
    {
    }

    public override bool Do()
    {
        _result = AppendNewLineInputAction.Instance.Do(_context);
        _context.CodeTextBox?.InvalidateVisual();

        return true;
    }

    public override void Undo()
    {
        ResetSelection();
        SetCursorToStartPosition();
        RightDeleteInputAction.Instance.Do(_context);
        if (_result!.IsSelectionExist)
        {
            InsertTextInputAction.Instance.Do(_context, _result.DeletedSelectedText);
        }
        ClearLineIfVirtualCursor();
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
        AppendNewLineInputAction.Instance.Do(_context);
        _context.CodeTextBox?.InvalidateVisual();
    }
}
