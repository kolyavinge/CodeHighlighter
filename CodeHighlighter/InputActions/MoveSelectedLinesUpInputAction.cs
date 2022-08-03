using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class MoveSelectedLinesUpInputAction
{
    public static readonly MoveSelectedLinesUpInputAction Instance = new();

    public MoveSelectedLinesResult Do(InputActionContext context)
    {
        var result = context.InputModel.MoveSelectedLinesUp();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.CodeTextBox?.InvalidateVisual();

        return result;
    }
}
