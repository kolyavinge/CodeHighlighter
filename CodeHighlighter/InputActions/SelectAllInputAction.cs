using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class SelectAllInputAction
{
    public static readonly SelectAllInputAction Instance = new();

    public void Do(InputModel inputModel, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox)
    {
        inputModel.SelectAll();
        viewport.CorrectByCursorPosition(textCursor);
        codeTextBox?.InvalidateVisual();
    }
}
