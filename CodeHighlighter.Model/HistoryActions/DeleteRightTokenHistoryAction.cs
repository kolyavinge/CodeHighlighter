using CodeHighlighter.Core;
using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;

namespace CodeHighlighter.HistoryActions;

internal interface IDeleteRightTokenHistoryAction : IHistoryAction { }

internal class DeleteRightTokenHistoryAction : TextHistoryAction<DeleteTokenResult>, IDeleteRightTokenHistoryAction
{
    private readonly IInputActionsFactory _inputActionsFactory;

    public DeleteRightTokenHistoryAction(IInputActionsFactory inputActionsFactory, IInputActionContext context) : base(context)
    {
        _inputActionsFactory = inputActionsFactory;
    }

    public override bool Do()
    {
        Result = _inputActionsFactory.Get<IDeleteRightTokenInputAction>().Do(_context);
        if (Result.HasDeleted) _context.CodeTextBox.InvalidateVisual();

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
        RestoreSelection();
        _inputActionsFactory.Get<IDeleteRightTokenInputAction>().Do(_context);
        _context.CodeTextBox.InvalidateVisual();
    }
}
