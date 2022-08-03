using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class MoveSelectedLinesDownInputAction
{
    public static readonly MoveSelectedLinesDownInputAction Instance = new();

    public MoveSelectedLinesResult Do(InputActionContext context)
    {
        var result = context.InputModel.MoveSelectedLinesDown();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.CodeTextBox?.InvalidateVisual();

        return result;
    }
}
