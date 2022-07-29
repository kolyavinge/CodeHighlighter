using System;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class ToUpperCaseInputAction
{
    public static readonly ToUpperCaseInputAction Instance = new();

    public void Do(InputModel inputModel, ICodeTextBox? codeTextBox, Action raiseTextChanged)
    {
        inputModel.SetSelectedTextCase(TextCase.Upper);
        raiseTextChanged();
        codeTextBox?.InvalidateVisual();
    }
}
