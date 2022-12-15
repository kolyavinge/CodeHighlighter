using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal interface IMoveCursorToInputAction
{
    void Do(InputActionContext context, CursorPosition position);
}

[InputAction]
internal class MoveCursorToInputAction : InputAction, IMoveCursorToInputAction
{
    public static readonly MoveCursorToInputAction Instance = new();

    public void Do(InputActionContext context, CursorPosition position)
    {
        context.TextCursor.MoveTo(position);
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
