using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class InsertTextHistoryAction : TextHistoryAction<InsertTextResult>
{
    private readonly IInputActionsFactory _inputActionsFactory;
    private readonly string _insertedText;

    public InsertTextHistoryAction(IInputActionsFactory inputActionsFactory, IInputActionContext context, string insertedText) : base(context)
    {
        _inputActionsFactory = inputActionsFactory;
        _insertedText = insertedText;
    }

    public override bool Do()
    {
        Result = _inputActionsFactory.Get<IInsertTextInputAction>().Do(_context, _insertedText);
        if (Result.HasInserted) _context.CodeTextBox.InvalidateVisual();

        return Result.HasInserted;
    }

    public override void Undo()
    {
        ResetSelection();
        if (Result.IsSelectionExist)
        {
            _context.TextSelection.Set(
                new(Result.SelectionStart.LineIndex, Result.SelectionStart.ColumnIndex),
                new(Result.SelectionStart.LineIndex, Result.SelectionStart.ColumnIndex + _insertedText.Length));

            _inputActionsFactory.Get<IInsertTextInputAction>().Do(_context, Result.DeletedSelectedText);
        }
        else
        {
            _context.TextSelection.Set(Result.InsertStartPosition, Result.InsertEndPosition);
            _inputActionsFactory.Get<ILeftDeleteInputAction>().Do(_context);
        }
        ClearLineIfVirtualCursor();
        SetCursorToStartPosition();
        _context.CodeTextBox.InvalidateVisual();
    }

    public override void Redo()
    {
        if (Result.IsSelectionExist)
        {
            RestoreSelection();
        }
        else
        {
            ResetSelection();
            SetCursorToStartPosition();
        }
        _inputActionsFactory.Get<IInsertTextInputAction>().Do(_context, _insertedText);
        _context.CodeTextBox.InvalidateVisual();
    }
}
