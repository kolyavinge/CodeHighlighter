using System;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.InputActions;

namespace CodeHighlighter.Model;

public class CodeTextBoxModel
{
    private ICodeTextBox? _codeTextBox;

    public event EventHandler? TextChanged;

    public Text Text { get; }
    public Tokens Tokens { get; }
    public TextCursor TextCursor { get; }
    public TextMeasures TextMeasures { get; }

    internal IViewportContext ViewportContext { get; set; }
    internal Viewport Viewport { get; private set; }
    internal TextSelection TextSelection { get; }
    internal FontSettings FontSettings { get; }
    internal InputModel InputModel { get; }
    internal BracketsHighlighter BracketsHighlighter { get; }

    public CodeTextBoxModel(ICodeProvider codeProvider, CodeTextBoxModelAdditionalParams? additionalParams = null)
    {
        Text = new Text();
        TextCursor = new TextCursor(Text);
        Tokens = new Tokens();
        FontSettings = new FontSettings();
        TextMeasures = new TextMeasures(FontSettings);
        TextSelection = new TextSelection();
        InputModel = new InputModel(Text, TextCursor, TextSelection, Tokens);
        ViewportContext = new DummyViewportContext();
        Viewport = new Viewport(ViewportContext, TextMeasures);
        BracketsHighlighter = new BracketsHighlighter(additionalParams?.HighlighteredBrackets ?? "", Text, TextCursor);
        SetCodeProvider(codeProvider);
    }

    internal void Init(ICodeTextBox? codeTextBox, IViewportContext viewportContext)
    {
        _codeTextBox = codeTextBox;
        ViewportContext = viewportContext;
        Viewport = new Viewport(ViewportContext, TextMeasures);
    }

    private void SetCodeProvider(ICodeProvider codeProvider)
    {
        InputModel.SetCodeProvider(codeProvider);
        if (codeProvider is ITokenKindUpdatable tokenKindUpdatable)
        {
            var tokenKindUpdater = new TokenKindUpdater(Tokens);
            tokenKindUpdatable.TokenKindUpdated += (s, e) =>
            {
                tokenKindUpdater.UpdateTokenKinds(e.UpdatedTokenKinds);
                _codeTextBox?.InvalidateVisual();
            };
        }
    }

    public void SetText(string text)
    {
        InputModel.SetText(text);
        Viewport.UpdateScrollbarsMaximumValues(Text);
        RaiseTextChanged();
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorLeft() => MoveCursorLeftInputAction.Instance.Do(InputModel, TextCursor, Viewport, _codeTextBox);

    public void MoveCursorRight() => MoveCursorRightInputAction.Instance.Do(InputModel, TextCursor, Viewport, _codeTextBox);

    public void MoveCursorUp() => MoveCursorUpInputAction.Instance.Do(InputModel, TextCursor, Viewport, _codeTextBox);

    public void MoveCursorDown() => MoveCursorDownInputAction.Instance.Do(InputModel, TextCursor, Viewport, _codeTextBox);

    public void MoveCursorStartLine() => MoveCursorStartLineInputAction.Instance.Do(InputModel, TextCursor, Viewport, _codeTextBox);

    public void MoveCursorEndLine() => MoveCursorEndLineInputAction.Instance.Do(InputModel, TextCursor, Viewport, _codeTextBox);

    public void MoveCursorTextBegin() => MoveCursorTextBeginInputAction.Instance.Do(InputModel, TextCursor, Viewport, _codeTextBox);

    public void MoveCursorTextEnd() => MoveCursorTextEndInputAction.Instance.Do(InputModel, TextCursor, Viewport, _codeTextBox);

    public void MoveCursorPageUp() => MoveCursorPageUpInputAction.Instance.Do(InputModel, TextCursor, Viewport, _codeTextBox);

    public void MoveCursorPageDown() => MoveCursorPageDownInputAction.Instance.Do(InputModel, TextCursor, Viewport, _codeTextBox);

    public void MoveSelectedLinesUp() => MoveSelectedLinesUpInputAction.Instance.Do(InputModel, TextCursor, Viewport, _codeTextBox);

    public void MoveSelectedLinesDown() => MoveSelectedLinesDownInputAction.Instance.Do(InputModel, TextCursor, Viewport, _codeTextBox);

    public void GotoLine(int lineIndex) => GotoLineInputAction.Instance.Do(lineIndex, InputModel, Text, TextMeasures, Viewport, ViewportContext, _codeTextBox);

    public void ScrollLineUp() => ScrollLineUpInputAction.Instance.Do(Viewport, _codeTextBox);

    public void ScrollLineDown() => ScrollLineDownInputAction.Instance.Do(Viewport, _codeTextBox);

    public void MoveToPrevToken() => MoveToPrevTokenInputAction.Instance.Do(InputModel, TextCursor, Viewport, _codeTextBox);

    public void MoveToNextToken() => MoveToNextTokenInputAction.Instance.Do(InputModel, TextCursor, Viewport, _codeTextBox);

    public void DeleteLeftToken() => DeleteLeftTokenInputAction.Instance.Do(InputModel, Text, TextCursor, Viewport, _codeTextBox, RaiseTextChanged);

    public void DeleteRightToken() => DeleteRightTokenInputAction.Instance.Do(InputModel, Text, TextCursor, Viewport, _codeTextBox, RaiseTextChanged);

    public void SelectAll() => SelectAllInputAction.Instance.Do(InputModel, TextCursor, Viewport, _codeTextBox);

    public void TextInput(string inputText) => TextInputInputAction.Instance.Do(inputText, InputModel, Text, TextCursor, Viewport, _codeTextBox, RaiseTextChanged);

    public void ReplaceText(int cursorStartLineIndex, int cursorStartColumnIndex, int cursorEndLineIndex, int cursorEndColumnIndex, string insertedText)
        => ReplaceTextInputAction.Instance.Do(
            cursorStartLineIndex, cursorStartColumnIndex, cursorEndLineIndex, cursorEndColumnIndex, insertedText, InputModel, Text, TextCursor, Viewport, _codeTextBox, RaiseTextChanged);

    public void NewLine() => NewLineInputAction.Instance.Do(InputModel, Text, TextCursor, Viewport, _codeTextBox, RaiseTextChanged);

    public void InsertText(string insertedText) => InsertTextInputAction.Instance.Do(insertedText, InputModel, Text, TextCursor, Viewport, _codeTextBox, RaiseTextChanged);

    public void DeleteSelectedLines() => DeleteSelectedLinesInputAction.Instance.Do(InputModel, Text, TextCursor, Viewport, _codeTextBox, RaiseTextChanged);

    public void LeftDelete() => LeftDeleteInputAction.Instance.Do(InputModel, Text, TextCursor, Viewport, _codeTextBox, RaiseTextChanged);

    public void RightDelete() => RightDeleteInputAction.Instance.Do(InputModel, Text, TextCursor, Viewport, _codeTextBox, RaiseTextChanged);

    public void ToUpperCase() => ToUpperCaseInputAction.Instance.Do(InputModel, _codeTextBox, RaiseTextChanged);

    public void ToLowerCase() => ToLowerCaseInputAction.Instance.Do(InputModel, _codeTextBox, RaiseTextChanged);

    private void RaiseTextChanged() => TextChanged?.Invoke(this, EventArgs.Empty);
}

public class CodeTextBoxModelAdditionalParams
{
    public string? HighlighteredBrackets { get; set; }
}
