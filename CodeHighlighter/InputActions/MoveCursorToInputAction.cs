using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class MoveCursorToInputAction
{
    public static readonly MoveCursorToInputAction Instance = new();

    public void Do(InputActionContext context, CursorPosition position)
    {
        context.InputModel.MoveCursorTo(position);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
    }
}
