using System;
using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.CodeProvidering;

namespace CodeHighlighter.Model;

public class CodeTextBoxModel
{
    private ICodeTextBox? _codeTextBox;

    public event EventHandler? TextChanged;

    public Text Text { get; }
    public Tokens Tokens { get; }
    public TextCursor TextCursor { get; }
    internal IViewportContext ViewportContext { get; set; }
    internal Viewport Viewport { get; private set; }
    internal TextSelection TextSelection { get; }
    internal FontSettings FontSettings { get; }
    public TextMeasures TextMeasures { get; }
    internal InputModel InputModel { get; }
    internal BracketsHighlighter BracketsHighlighter { get; }

    public CodeTextBoxModel(ICodeProvider codeProvider, CodeTextBoxModelAdditionalParams? additionalParams = null)
    {
        Text = new Text();
        TextCursor = new TextCursor(Text);
        TextSelection = new TextSelection();
        Tokens = new Tokens();
        FontSettings = new FontSettings();
        TextMeasures = new TextMeasures(FontSettings);
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

    public void MoveCursorLeft()
    {
        InputModel.MoveCursorLeft();
        Viewport.CorrectByCursorPosition(TextCursor);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorRight()
    {
        InputModel.MoveCursorRight();
        Viewport.CorrectByCursorPosition(TextCursor);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorUp()
    {
        InputModel.MoveCursorUp();
        Viewport.CorrectByCursorPosition(TextCursor);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorDown()
    {
        InputModel.MoveCursorDown();
        Viewport.CorrectByCursorPosition(TextCursor);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorStartLine()
    {
        InputModel.MoveCursorStartLine();
        Viewport.CorrectByCursorPosition(TextCursor);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorEndLine()
    {
        InputModel.MoveCursorEndLine();
        Viewport.CorrectByCursorPosition(TextCursor);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorTextBegin()
    {
        InputModel.MoveCursorTextBegin();
        Viewport.CorrectByCursorPosition(TextCursor);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorTextEnd()
    {
        InputModel.MoveCursorTextEnd();
        Viewport.CorrectByCursorPosition(TextCursor);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorPageUp()
    {
        InputModel.MoveCursorPageUp(Viewport.GetLinesCountInViewport());
        Viewport.CorrectByCursorPosition(TextCursor);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorPageDown()
    {
        InputModel.MoveCursorPageDown(Viewport.GetLinesCountInViewport());
        Viewport.CorrectByCursorPosition(TextCursor);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveSelectedLinesUp()
    {
        InputModel.MoveSelectedLinesUp();
        Viewport.CorrectByCursorPosition(TextCursor);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveSelectedLinesDown()
    {
        InputModel.MoveSelectedLinesDown();
        Viewport.CorrectByCursorPosition(TextCursor);
        _codeTextBox?.InvalidateVisual();
    }

    public void GotoLine(int lineIndex)
    {
        lineIndex = lineIndex < Text.LinesCount ? lineIndex : Text.LinesCount;
        InputModel.MoveCursorTo(lineIndex, 0);
        var offsetLine = lineIndex - (Viewport.GetLinesCountInViewport()) / 2;
        if (offsetLine < 0) offsetLine = 0;
        ViewportContext.VerticalScrollBarValue = offsetLine * TextMeasures.LineHeight;
        _codeTextBox?.InvalidateVisual();
        _codeTextBox?.Focus();
    }

    public void ScrollLineUp()
    {
        Viewport.ScrollLineUp();
        _codeTextBox?.InvalidateVisual();
    }

    public void ScrollLineDown()
    {
        Viewport.ScrollLineDown();
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveToPrevToken()
    {
        InputModel.MoveToPrevToken();
        Viewport.CorrectByCursorPosition(TextCursor);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveToNextToken()
    {
        InputModel.MoveToNextToken();
        Viewport.CorrectByCursorPosition(TextCursor);
        _codeTextBox?.InvalidateVisual();
    }

    public void DeleteLeftToken()
    {
        InputModel.DeleteLeftToken();
        Viewport.CorrectByCursorPosition(TextCursor);
        Viewport.UpdateScrollbarsMaximumValues(Text);
        RaiseTextChanged();
        _codeTextBox?.InvalidateVisual();
    }

    public void DeleteRightToken()
    {
        InputModel.DeleteRightToken();
        Viewport.CorrectByCursorPosition(TextCursor);
        Viewport.UpdateScrollbarsMaximumValues(Text);
        RaiseTextChanged();
        _codeTextBox?.InvalidateVisual();
    }

    public void SelectAll()
    {
        InputModel.SelectAll();
        Viewport.CorrectByCursorPosition(TextCursor);
        _codeTextBox?.InvalidateVisual();
    }

    private static readonly HashSet<char> _notAllowedSymbols = new(new[] { '\n', '\r', '\b', '\u001B' });
    public void TextInput(string inputText)
    {
        var text = inputText.Where(ch => !_notAllowedSymbols.Contains(ch)).ToList();
        if (!text.Any()) return;
        foreach (var ch in text) InputModel.AppendChar(ch);
        Viewport.CorrectByCursorPosition(TextCursor);
        Viewport.UpdateScrollbarsMaximumValues(Text);
        RaiseTextChanged();
        _codeTextBox?.InvalidateVisual();
    }

    public void ReplaceText(int cursorStartLineIndex, int cursorStartColumnIndex, int cursorEndLineIndex, int cursorEndColumnIndex, string text)
    {
        InputModel.MoveCursorTo(cursorStartLineIndex, cursorStartColumnIndex);
        InputModel.ActivateSelection();
        InputModel.MoveCursorTo(cursorEndLineIndex, cursorEndColumnIndex);
        InputModel.CompleteSelection();
        InputModel.InsertText(text);
        Viewport.CorrectByCursorPosition(TextCursor);
        Viewport.UpdateScrollbarsMaximumValues(Text);
        RaiseTextChanged();
        _codeTextBox?.InvalidateVisual();
    }

    public void NewLine()
    {
        InputModel.NewLine();
        Viewport.CorrectByCursorPosition(TextCursor);
        Viewport.UpdateScrollbarsMaximumValues(Text);
        RaiseTextChanged();
        _codeTextBox?.InvalidateVisual();
    }

    public void InsertText(string insertedText)
    {
        InputModel.InsertText(insertedText);
        Viewport.CorrectByCursorPosition(TextCursor);
        Viewport.UpdateScrollbarsMaximumValues(Text);
        RaiseTextChanged();
        _codeTextBox?.InvalidateVisual();
    }

    public void DeleteSelectedLines()
    {
        InputModel.DeleteSelectedLines();
        Viewport.CorrectByCursorPosition(TextCursor);
        Viewport.UpdateScrollbarsMaximumValues(Text);
        RaiseTextChanged();
        _codeTextBox?.InvalidateVisual();
    }

    public void LeftDelete()
    {
        InputModel.LeftDelete();
        Viewport.CorrectByCursorPosition(TextCursor);
        Viewport.UpdateScrollbarsMaximumValues(Text);
        RaiseTextChanged();
        _codeTextBox?.InvalidateVisual();
    }

    public void RightDelete()
    {
        InputModel.RightDelete();
        Viewport.CorrectByCursorPosition(TextCursor);
        Viewport.UpdateScrollbarsMaximumValues(Text);
        RaiseTextChanged();
        _codeTextBox?.InvalidateVisual();
    }

    public void ToUpperCase()
    {
        InputModel.SetSelectedTextCase(TextCase.Upper);
        RaiseTextChanged();
        _codeTextBox?.InvalidateVisual();
    }

    public void ToLowerCase()
    {
        InputModel.SetSelectedTextCase(TextCase.Lower);
        RaiseTextChanged();
        _codeTextBox?.InvalidateVisual();
    }

    private void RaiseTextChanged() => TextChanged?.Invoke(this, EventArgs.Empty);
}

public class CodeTextBoxModelAdditionalParams
{
    public string? HighlighteredBrackets { get; set; }
}
