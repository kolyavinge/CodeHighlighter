using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class SetTextHistoryAction : TextHistoryAction<SetTextResult>
{
    public readonly string _text;

    public SetTextHistoryAction(InputActionContext context, string text) : base(context)
    {
        _text = text;
    }

    public override bool Do()
    {
        Result = SetTextInputAction.Instance.Do(_context, _text);
        _context.CodeTextBox.InvalidateVisual();

        return true;
    }

    public override void Undo()
    {
        SetTextInputAction.Instance.Do(_context, Result.DeletedSelectedText);
        SetCursorToStartPosition();
        _context.CodeTextBox.InvalidateVisual();
    }

    public override void Redo()
    {
        SetTextInputAction.Instance.Do(_context, _text);
        _context.CodeTextBox.InvalidateVisual();
    }
}
