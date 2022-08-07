using System;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class HistoryActionContext : InputActionContext
{
    public ICodeTextBox? CodeTextBox;

    public HistoryActionContext(
        InputModel inputModel,
        Text text,
        TextCursor textCursor,
        TextMeasures textMeasures,
        TextSelection textSelection,
        Viewport viewport,
        IViewportContext viewportContext,
        Action raiseTextChanged) : base(inputModel, text, textCursor, textMeasures, textSelection, viewport, viewportContext, raiseTextChanged)
    {
    }
}
