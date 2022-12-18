using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal interface IDeleteSelectedLinesHistoryAction : IHistoryAction { }

internal class DeleteSelectedLinesHistoryAction : TextHistoryAction<DeleteSelectedLinesResult>, IDeleteSelectedLinesHistoryAction
{
    private readonly IInputActionsFactory _inputActionsFactory;

    public DeleteSelectedLinesHistoryAction(IInputActionsFactory inputActionsFactory, IInputActionContext context) : base(context)
    {
        _inputActionsFactory = inputActionsFactory;
    }

    public override bool Do()
    {
        Result = _inputActionsFactory.Get<IDeleteSelectedLinesInputAction>().Do(_context);
        if (Result.HasDeleted || Result.OldCursorPosition.Kind == CursorPositionKind.Virtual) _context.CodeTextBox.InvalidateVisual();

        return Result.HasDeleted;
    }

    public override void Undo()
    {
        ResetSelection();
        if (Result.IsSelectionExist)
        {
            _inputActionsFactory.Get<IMoveCursorToInputAction>().Do(_context, new(Result.SelectionStart.LineIndex, 0));
        }
        else
        {
            _inputActionsFactory.Get<IMoveCursorToInputAction>().Do(_context, new(Result.OldCursorPosition.LineIndex, 0));
        }
        _inputActionsFactory.Get<IInsertTextInputAction>().Do(_context, Result.DeletedSelectedText);
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
        _inputActionsFactory.Get<IDeleteSelectedLinesInputAction>().Do(_context);
        _context.CodeTextBox.InvalidateVisual();
    }
}
