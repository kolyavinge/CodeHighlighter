using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.CodeProvidering;

namespace CodeHighlighter.Model;

internal class InputModel
{
    private ICodeProvider _codeProvider;

    public Text Text { get; }
    public TextCursor TextCursor { get; }
    public TextSelection TextSelection { get; }
    public Tokens Tokens { get; }
    public TokensColors TokenColors { get; }

    public InputModel(Text text, TextCursor textCursor, TextSelection textSelection, Tokens tokens)
    {
        Text = text;
        TextCursor = textCursor;
        TextSelection = textSelection;
        Tokens = tokens;
        TokenColors = new();
        _codeProvider = new EmptyCodeProvider();
    }

    public void SetCodeProvider(ICodeProvider codeProvider)
    {
        _codeProvider = codeProvider;
        SetTokens();
    }

    public SetTextResult SetText(string text)
    {
        var oldCursorPosition = TextCursor.Position;
        var oldText = Text.TextContent;
        TextCursor.MoveTextBegin();
        Text.TextContent = text;
        SetTokens();

        return new(oldCursorPosition, oldText, text);
    }

    public void MoveCursorTo(CursorPosition position)
    {
        TextCursor.MoveTo(position);
        SetSelection();
    }

    public void ActivateSelection()
    {
        if (!TextSelection.InProgress)
        {
            TextSelection.InProgress = true;
            TextSelection.StartPosition = TextCursor.Position;
            TextSelection.EndPosition = TextCursor.Position;
        }
    }

    public void CompleteSelection()
    {
        TextSelection.InProgress = false;
    }

    public string GetSelectedText()
    {
        if (!TextSelection.IsExist) return "";
        var selectedLines = new List<string>();
        foreach (var line in TextSelection.GetSelectedLines(Text))
        {
            selectedLines.Add(Text.GetLine(line.LineIndex).GetSubstring(line.LeftColumnIndex, line.RightColumnIndex - line.LeftColumnIndex));
        }

        return String.Join(Environment.NewLine, selectedLines);
    }

    public CaseResult SetSelectedTextCase(TextCase textCase)
    {
        var cursorPosition = TextCursor.Position;
        var (selectionStart, selectionEnd) = TextSelection.GetSortedPositions();
        var deletedSelectedText = GetSelectedText();
        Text.SetSelectedTextCase(TextSelection, textCase);
        var changedText = GetSelectedText();
        UpdateTokensForLines(selectionStart.LineIndex, selectionEnd.LineIndex - selectionStart.LineIndex + 1);

        return new(cursorPosition, selectionStart, selectionEnd, deletedSelectedText, changedText);
    }

    private void SetSelection()
    {
        if (TextSelection.InProgress)
        {
            TextSelection.EndPosition = TextCursor.Position;
        }
        else
        {
            TextSelection.StartPosition = TextCursor.Position;
            TextSelection.EndPosition = TextCursor.Position;
        }
    }

    private void SetTokens()
    {
        var codeProviderTokens = _codeProvider.GetTokens(new ForwardTextIterator(Text, 0, Text.LinesCount - 1)).ToList();
        Tokens.SetTokens(codeProviderTokens, 0, Text.LinesCount);
        TokenColors.SetColors(_codeProvider.GetColors());
    }

    private void UpdateTokensForLines(int startLineIndex, int count)
    {
        var codeProviderTokens = _codeProvider.GetTokens(new ForwardTextIterator(Text, startLineIndex, startLineIndex + count - 1)).ToList();
        Tokens.SetTokens(codeProviderTokens, startLineIndex, count);
    }
}
