using CodeHighlighter.Core;
using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;

namespace CodeHighlighter.HistoryActions;

internal interface IAppendCharHistoryAction : IHistoryAction
{
    IAppendCharHistoryAction SetParams(char appendedChar);
}

internal class AppendCharHistoryAction : TextHistoryAction<AppendCharResult>, IAppendCharHistoryAction
{
    private readonly IInputActionsFactory _inputActionsFactory;
    private char _appendedChar;

    public AppendCharHistoryAction(IInputActionsFactory inputActionsFactory, IInputActionContext context) : base(context)
    {
        _inputActionsFactory = inputActionsFactory;
    }

    public IAppendCharHistoryAction SetParams(char appendedChar)
    {
        _appendedChar = appendedChar;
        return this;
    }

    public override bool Do()
    {
        Result = _inputActionsFactory.Get<IAppendCharInputAction>().Do(_context, _appendedChar);
        _context.CodeTextBox.InvalidateVisual();

        return true;
    }

    public override void Undo()
    {
        ResetSelection();
        if (Result.IsSelectionExist)
        {
            _context.TextSelection.Set(
                new(Result.SelectionStart.LineIndex, Result.SelectionStart.ColumnIndex),
                new(Result.SelectionStart.LineIndex, Result.SelectionStart.ColumnIndex + 1));

            _inputActionsFactory.Get<IInsertTextInputAction>().Do(_context, Result.DeletedSelectedText);
        }
        else
        {
            SetCursorToStartPosition();
            _inputActionsFactory.Get<IRightDeleteInputAction>().Do(_context);
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
        _inputActionsFactory.Get<IAppendCharInputAction>().Do(_context, _appendedChar);
        _context.CodeTextBox.InvalidateVisual();
    }
}
