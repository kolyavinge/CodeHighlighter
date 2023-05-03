using CodeHighlighter.Ancillary;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Core;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal interface IInputActionContext
{
    ICodeProvider CodeProvider { get; }
    ICodeTextBoxView CodeTextBox { get; set; }
    IText Text { get; }
    ITextCursor TextCursor { get; }
    ITextEventsInternal TextEvents { get; }
    ITextMeasuresInternal TextMeasures { get; }
    ITextSelectionInternal TextSelection { get; }
    ITextSelector TextSelector { get; }
    ITokensColors TokenColors { get; }
    ITokensInternal Tokens { get; }
    IViewportInternal Viewport { get; }
    IViewportCursorPositionCorrector CursorPositionCorrector { get; }
    IPageScroller PageScroller { get; }
    ILineFoldsUpdater LineFoldsUpdater { get; }
}

internal class InputActionContext : IInputActionContext
{
    public ICodeProvider CodeProvider { get; }
    public IText Text { get; }
    public ITextCursor TextCursor { get; }
    public ITextMeasuresInternal TextMeasures { get; }
    public ITextSelectionInternal TextSelection { get; }
    public ITextSelector TextSelector { get; }
    public ITokensInternal Tokens { get; }
    public ITokensColors TokenColors { get; }
    public IViewportInternal Viewport { get; set; }
    public IViewportCursorPositionCorrector CursorPositionCorrector { get; }
    public IPageScroller PageScroller { get; }
    public ILineFoldsUpdater LineFoldsUpdater { get; }
    public ITextEventsInternal TextEvents { get; }
    public ICodeTextBoxView CodeTextBox { get; set; }

    public InputActionContext(
        ICodeProvider codeProvider,
        IText text,
        ITextCursor textCursor,
        ITextMeasuresInternal textMeasures,
        ITextSelectionInternal textSelection,
        ITextSelector textSelector,
        ITokensInternal tokens,
        ITokensColors tokenColors,
        IViewportInternal viewport,
        IViewportCursorPositionCorrector cursorPositionCorrector,
        IPageScroller pageScroller,
        ILineFoldsUpdater lineFoldsUpdater,
        ITextEventsInternal textEvents)
    {
        CodeProvider = codeProvider;
        Text = text;
        TextCursor = textCursor;
        TextMeasures = textMeasures;
        TextSelection = textSelection;
        TextSelector = textSelector;
        Tokens = tokens;
        TokenColors = tokenColors;
        Viewport = viewport;
        CursorPositionCorrector = cursorPositionCorrector;
        PageScroller = pageScroller;
        LineFoldsUpdater = lineFoldsUpdater;
        TextEvents = textEvents;
        CodeTextBox = DummyCodeTextBox.Instance;
    }
}
