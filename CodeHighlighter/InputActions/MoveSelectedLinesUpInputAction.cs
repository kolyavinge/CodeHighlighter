using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class MoveSelectedLinesUpInputAction
{
    public static readonly MoveSelectedLinesUpInputAction Instance = new();

    public void Do(InputModel inputModel, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox)
    {
        inputModel.MoveSelectedLinesUp();
        viewport.CorrectByCursorPosition(textCursor);
        codeTextBox?.InvalidateVisual();
    }
}
