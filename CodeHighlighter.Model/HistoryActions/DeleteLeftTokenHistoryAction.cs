using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal interface IDeleteLeftTokenHistoryAction : IHistoryAction { }

[HistoryAction]
internal class DeleteLeftTokenHistoryAction : TextHistoryAction<DeleteTokenResult>, IDeleteLeftTokenHistoryAction
{
    private readonly IInputActionsFactory _inputActionsFactory;

    public DeleteLeftTokenHistoryAction(IInputActionsFactory inputActionsFactory, IInputActionContext context) : base(context)
    {
        _inputActionsFactory = inputActionsFactory;
    }

    public override bool Do()
    {
        Result = _inputActionsFactory.Get<IDeleteLeftTokenInputAction>().Do(_context);
        if (Result.HasDeleted || Result.OldCursorPosition.Kind == CursorPositionKind.Virtual) _context.CodeTextBox.InvalidateVisual();

        return Result.HasDeleted;
    }

    public override void Undo()
    {
        ResetSelection();
        SetCursorToEndPosition();
        _inputActionsFactory.Get<IInsertTextInputAction>().Do(_context, Result.DeletedSelectedText);
        ClearLineIfVirtualCursor();
        SetCursorToStartPosition();
        _context.CodeTextBox.InvalidateVisual();
    }

    public override void Redo()
    {
        if (!Result.IsSelectionExist && Result.OldCursorPosition.Kind == CursorPositionKind.Virtual)
        {
            ResetSelection();
            SetCursorToStartPosition();
        }
        else
        {
            RestoreSelection();
        }
        _inputActionsFactory.Get<IDeleteLeftTokenInputAction>().Do(_context);
        _context.CodeTextBox.InvalidateVisual();
    }
}
