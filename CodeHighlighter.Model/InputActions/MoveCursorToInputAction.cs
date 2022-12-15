using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class MoveCursorToInputAction : InputAction
{
    public static readonly MoveCursorToInputAction Instance = new();

    public void Do(InputActionContext context, CursorPosition position)
    {
        context.TextCursor.MoveTo(position);
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
