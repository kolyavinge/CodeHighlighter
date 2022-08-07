using System;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class InputActionContext
{
    public InputModel InputModel { get; }
    public Text Text { get; }
    public TextCursor TextCursor { get; }
    public TextMeasures TextMeasures { get; }
    public TextSelection TextSelection { get; }
    public Viewport Viewport { get; set; }
    public IViewportContext ViewportContext { get; set; }
    public Action RaiseTextChanged { get; }

    public InputActionContext(
        InputModel inputModel,
        Text text,
        TextCursor textCursor,
        TextMeasures textMeasures,
        TextSelection textSelection,
        Viewport viewport,
        IViewportContext viewportContext,
        Action raiseTextChanged)
    {
        InputModel = inputModel;
        Text = text;
        TextCursor = textCursor;
        TextMeasures = textMeasures;
        TextSelection = textSelection;
        Viewport = viewport;
        ViewportContext = viewportContext;
        RaiseTextChanged = raiseTextChanged;
    }
}
