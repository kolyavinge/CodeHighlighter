using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class InputActionContext
{
    public readonly InputModel InputModel;
    public readonly Text Text;
    public readonly TextCursor TextCursor;
    public readonly TextMeasures TextMeasures;
    public readonly TextSelection TextSelection;
    public Viewport Viewport;
    public IViewportContext ViewportContext;
    public readonly Action RaiseTextChanged;
    public readonly Action RaiseTextSet;

    public InputActionContext(
        InputModel inputModel,
        Text text,
        TextCursor textCursor,
        TextMeasures textMeasures,
        TextSelection textSelection,
        Viewport viewport,
        IViewportContext viewportContext,
        Action raiseTextChanged,
        Action raiseTextSet)
    {
        InputModel = inputModel;
        Text = text;
        TextCursor = textCursor;
        TextMeasures = textMeasures;
        TextSelection = textSelection;
        Viewport = viewport;
        ViewportContext = viewportContext;
        RaiseTextChanged = raiseTextChanged;
        RaiseTextSet = raiseTextSet;
    }
}
