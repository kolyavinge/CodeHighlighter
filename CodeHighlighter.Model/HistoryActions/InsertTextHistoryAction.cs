using CodeHighlighter.Core;
using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;

namespace CodeHighlighter.HistoryActions;

internal interface IInsertTextHistoryAction : IHistoryAction
{
    IInsertTextHistoryAction SetParams(string insertedText);
}

internal class InsertTextHistoryAction : TextHistoryAction<InsertTextResult>, IInsertTextHistoryAction
{
    private readonly IInputActionsFactory _inputActionsFactory;
    private string? _insertedText;

    public InsertTextHistoryAction(IInputActionsFactory inputActionsFactory, IInputActionContext context) : base(context)
    {
        _inputActionsFactory = inputActionsFactory;
    }

    public IInsertTextHistoryAction SetParams(string insertedText)
    {
        _insertedText = insertedText;
        return this;
    }

    public override bool Do()
    {
        Result = _inputActionsFactory.Get<IInsertTextInputAction>().Do(_context, _insertedText!);
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
                new(Result.SelectionStart.LineIndex, Result.SelectionStart.ColumnIndex + _insertedText!.Length));

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
        _inputActionsFactory.Get<IInsertTextInputAction>().Do(_context, _insertedText!);
        _context.CodeTextBox.InvalidateVisual();
    }
}
