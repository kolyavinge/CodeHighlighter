using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class MoveSelectedLinesDownHistoryAction : TextHistoryAction<MoveSelectedLinesResult>
{
    private readonly IInputActionsFactory _inputActionsFactory;

    public MoveSelectedLinesDownHistoryAction(IInputActionsFactory inputActionsFactory, IInputActionContext context) : base(context)
    {
        _inputActionsFactory = inputActionsFactory;
    }

    public override bool Do()
    {
        Result = _inputActionsFactory.Get<IMoveSelectedLinesDownInputAction>().Do(_context);
        if (Result.HasMoved) _context.CodeTextBox.InvalidateVisual();

        return Result.HasMoved;
    }

    public override void Undo()
    {
        if (Result.IsSelectionExist)
        {
            RestoreSelectionLineDown();
        }
        else
        {
            ResetSelection();
            SetCursorToEndPosition();
        }
        _inputActionsFactory.Get<IMoveSelectedLinesUpInputAction>().Do(_context);
        SetCursorToStartPosition();
        RestoreSelection();
        _context.CodeTextBox.InvalidateVisual();
    }

    public override void Redo()
    {
        SetCursorToStartPosition();
        if (Result.IsSelectionExist)
        {
            RestoreSelection();
        }
        else
        {
            ResetSelection();
            SetCursorToStartPosition();
        }
        _inputActionsFactory.Get<IMoveSelectedLinesDownInputAction>().Do(_context);
        _context.CodeTextBox.InvalidateVisual();
    }
}
