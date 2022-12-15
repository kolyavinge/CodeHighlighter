using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class RightDeleteHistoryAction : TextHistoryAction<DeleteResult>
{
    private readonly IInputActionsFactory _inputActionsFactory;

    public RightDeleteHistoryAction(IInputActionsFactory inputActionsFactory, InputActionContext context) : base(context)
    {
        _inputActionsFactory = inputActionsFactory;
    }

    public override bool Do()
    {
        Result = _inputActionsFactory.Get<IRightDeleteInputAction>().Do(_context);
        if (Result.HasDeleted) _context.CodeTextBox.InvalidateVisual();

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
        _inputActionsFactory.Get<IRightDeleteInputAction>().Do(_context);
        _context.CodeTextBox.InvalidateVisual();
    }
}
