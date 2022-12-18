using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal interface IAppendNewLineHistoryAction : IHistoryAction { }

internal class AppendNewLineHistoryAction : TextHistoryAction<AppendNewLineResult>, IAppendNewLineHistoryAction
{
    private readonly IInputActionsFactory _inputActionsFactory;

    public AppendNewLineHistoryAction(IInputActionsFactory inputActionsFactory, IInputActionContext context) : base(context)
    {
        _inputActionsFactory = inputActionsFactory;
    }

    public override bool Do()
    {
        Result = _inputActionsFactory.Get<IAppendNewLineInputAction>().Do(_context);
        _context.CodeTextBox.InvalidateVisual();

        return true;
    }

    public override void Undo()
    {
        ResetSelection();
        if (Result.IsSelectionExist)
        {
            _context.TextCursor.MoveTo(new(Result.SelectionStart.LineIndex, Result.SelectionStart.ColumnIndex));
        }
        else
        {
            SetCursorToStartPosition();
        }
        _inputActionsFactory.Get<IRightDeleteInputAction>().Do(_context);
        if (Result.IsSelectionExist)
        {
            _inputActionsFactory.Get<IInsertTextInputAction>().Do(_context, Result.DeletedSelectedText);
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
        _inputActionsFactory.Get<IAppendNewLineInputAction>().Do(_context);
        _context.CodeTextBox.InvalidateVisual();
    }
}
