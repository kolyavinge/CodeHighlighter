using CodeHighlighter.Core;
using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;

namespace CodeHighlighter.HistoryActions;

internal interface IMoveSelectedLinesUpHistoryAction : IHistoryAction { }

internal class MoveSelectedLinesUpHistoryAction : TextHistoryAction<MoveSelectedLinesResult>, IMoveSelectedLinesUpHistoryAction
{
    private readonly IInputActionsFactory _inputActionsFactory;

    public MoveSelectedLinesUpHistoryAction(IInputActionsFactory inputActionsFactory, IInputActionContext context) : base(context)
    {
        _inputActionsFactory = inputActionsFactory;
    }

    public override bool Do()
    {
        Result = _inputActionsFactory.Get<IMoveSelectedLinesUpInputAction>().Do(_context);
        if (Result.HasMoved) _context.CodeTextBox.InvalidateVisual();

        return Result.HasMoved;
    }

    public override void Undo()
    {
        if (Result.IsSelectionExist)
        {
            RestoreSelectionLineUp();
        }
        else
        {
            ResetSelection();
            SetCursorToEndPosition();
        }
        _inputActionsFactory.Get<IMoveSelectedLinesDownInputAction>().Do(_context);
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
        _inputActionsFactory.Get<IMoveSelectedLinesUpInputAction>().Do(_context);
        _context.CodeTextBox.InvalidateVisual();
    }
}
