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

    private void SetTokens()
    {
        var codeProviderTokens = _codeProvider.GetTokens(new ForwardTextIterator(Text, 0, Text.LinesCount - 1)).ToList();
        Tokens.SetTokens(codeProviderTokens, 0, Text.LinesCount);
        TokenColors.SetColors(_codeProvider.GetColors());
    }
}
