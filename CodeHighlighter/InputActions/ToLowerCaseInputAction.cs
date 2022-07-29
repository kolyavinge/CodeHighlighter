using System;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class ToLowerCaseInputAction
{
    public static readonly ToLowerCaseInputAction Instance = new();

    public void Do(InputModel inputModel, ICodeTextBox? codeTextBox, Action raiseTextChanged)
    {
        inputModel.SetSelectedTextCase(TextCase.Lower);
        raiseTextChanged();
        codeTextBox?.InvalidateVisual();
    }
}
