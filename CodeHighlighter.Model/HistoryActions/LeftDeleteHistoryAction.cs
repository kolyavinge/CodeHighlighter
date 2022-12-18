using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal interface ILeftDeleteHistoryAction : IHistoryAction { }

internal class LeftDeleteHistoryAction : TextHistoryAction<DeleteResult>, ILeftDeleteHistoryAction
{
    private readonly IInputActionsFactory _inputActionsFactory;

    public LeftDeleteHistoryAction(IInputActionsFactory inputActionsFactory, IInputActionContext context) : base(context)
    {
        _inputActionsFactory = inputActionsFactory;
    }

    public override bool Do()
    {
        Result = _inputActionsFactory.Get<ILeftDeleteInputAction>().Do(_context);
        if (Result.HasDeleted || Result.OldCursorPosition.Kind == CursorPositionKind.Virtual) _context.CodeTextBox.InvalidateVisual();

        return Result.HasDeleted;
    }

    public override void Undo()
    {
        ResetSelection();
        SetCursorToEndPosition();
        var deletedSelectedText = Result.DeletedSelectedText != "" ? Result.DeletedSelectedText : Result.CharCharDeleteResult.DeletedChar.ToString();
        _inputActionsFactory.Get<IInsertTextInputAction>().Do(_context, deletedSelectedText);
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
        _inputActionsFactory.Get<ILeftDeleteInputAction>().Do(_context);
        _context.CodeTextBox.InvalidateVisual();
    }
}
