using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class MoveCursorStartLineInputAction
{
    public static readonly MoveCursorStartLineInputAction Instance = new();

    public void Do(InputModel inputModel, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox)
    {
        inputModel.MoveCursorStartLine();
        viewport.CorrectByCursorPosition(textCursor);
        codeTextBox?.InvalidateVisual();
    }
}
