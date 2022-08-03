using System;
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

    internal static InputModel MakeDefault()
    {
        var text = new Text();
        var textCursor = new TextCursor(text);
        var textSelection = new TextSelection(0, 0, 0, 0);
        var tokens = new Tokens();
        return new(text, textCursor, textSelection, tokens);
    }

    public void SetCodeProvider(ICodeProvider codeProvider)
    {
        _codeProvider = codeProvider;
        SetTokens();
    }

    public void SetText(string text)
    {
        Text.TextContent = text;
        SetTokens();
    }

    public void MoveCursorTo(CursorPosition position)
    {
        TextCursor.MoveTo(position);
        SetSelection();
    }

    public void MoveCursorUp()
    {
        TextCursor.MoveUp();
        SetSelection();
    }

    public void MoveCursorDown()
    {
        TextCursor.MoveDown();
        SetSelection();
    }

    public void MoveCursorLeft()
    {
        TextCursor.MoveLeft();
        SetSelection();
    }

    public void MoveCursorRight()
    {
        TextCursor.MoveRight();
        SetSelection();
    }

    public void MoveCursorStartLine()
    {
        TextCursor.MoveStartLine();
        SetSelection();
    }

    public void MoveCursorTextBegin()
    {
        TextCursor.MoveTextBegin();
        SetSelection();
    }

    public void MoveCursorEndLine()
    {
        TextCursor.MoveEndLine();
        SetSelection();
    }

    public void MoveCursorTextEnd()
    {
        TextCursor.MoveTextEnd();
        SetSelection();
    }

    public void MoveCursorPageUp(int pageSize)
    {
        TextCursor.MovePageUp(pageSize);
        SetSelection();
    }

    public void MoveCursorPageDown(int pageSize)
    {
        TextCursor.MovePageDown(pageSize);
        SetSelection();
    }

    public void SelectAll()
    {
        TextSelection.InProgress = false;
        TextSelection.StartCursorLineIndex = 0;
        TextSelection.StartCursorColumnIndex = 0;
        TextSelection.EndCursorLineIndex = Text.LinesCount - 1;
        TextSelection.EndCursorColumnIndex = Text.GetLine(TextSelection.EndCursorLineIndex).Length;
        TextCursor.MoveTextEnd();
    }

    public void ActivateSelection()
    {
        if (!TextSelection.InProgress)
        {
            TextSelection.InProgress = true;
            TextSelection.StartCursorLineIndex = TextCursor.LineIndex;
            TextSelection.StartCursorColumnIndex = TextCursor.ColumnIndex;
            TextSelection.EndCursorLineIndex = TextCursor.LineIndex;
            TextSelection.EndCursorColumnIndex = TextCursor.ColumnIndex;
        }
    }

    public void CompleteSelection()
    {
        TextSelection.InProgress = false;
    }

    private void SetSelection()
    {
        if (TextSelection.InProgress)
        {
            TextSelection.EndCursorLineIndex = TextCursor.LineIndex;
            TextSelection.EndCursorColumnIndex = TextCursor.ColumnIndex;
        }
        else
        {
            TextSelection.StartCursorLineIndex = TextCursor.LineIndex;
            TextSelection.StartCursorColumnIndex = TextCursor.ColumnIndex;
            TextSelection.EndCursorLineIndex = TextCursor.LineIndex;
            TextSelection.EndCursorColumnIndex = TextCursor.ColumnIndex;
        }
    }

    public void SelectToken(CursorPosition position)
    {
        var selector = new TokenSelector();
        var range = selector.GetSelection(Tokens, position);
        TextSelection.Reset();
        TextSelection.StartCursorLineIndex = position.LineIndex;
        TextSelection.StartCursorColumnIndex = range.StartCursorColumnIndex;
        TextSelection.EndCursorLineIndex = position.LineIndex;
        TextSelection.EndCursorColumnIndex = range.EndCursorColumnIndex;
    }

    public void MoveToNextToken()
    {
        var navigator = new TokenNavigator();
        var pos = navigator.MoveRight(Text, Tokens, TextCursor.LineIndex, TextCursor.ColumnIndex);
        TextCursor.MoveTo(pos);
        SetSelection();
    }

    public void MoveToPrevToken()
    {
        var navigator = new TokenNavigator();
        var pos = navigator.MoveLeft(Text, Tokens, TextCursor.LineIndex, TextCursor.ColumnIndex);
        TextCursor.MoveTo(pos);
        SetSelection();
    }

    public DeleteTokenResult DeleteLeftToken()
    {
        var oldCursorPosition = TextCursor.Position;
        if (!TextSelection.IsExist)
        {
            var navigator = new TokenNavigator();
            var position = navigator.MoveLeft(Text, Tokens, TextCursor.LineIndex, TextCursor.ColumnIndex);
            ActivateSelection();
            MoveCursorTo(position);
            CompleteSelection();
        }
        var (selectionStart, selectionEnd) = TextSelection.GetSortedPositions();
        var deletedSelectedText = GetSelectedText();
        var deleteResult = LeftDelete();
        var newCursorPosition = TextCursor.Position;

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText, deleteResult.HasDeleted);
    }

    public DeleteTokenResult DeleteRightToken()
    {
        var oldCursorPosition = TextCursor.Position;
        if (!TextSelection.IsExist)
        {
            var navigator = new TokenNavigator();
            var position = navigator.MoveRight(Text, Tokens, TextCursor.LineIndex, TextCursor.ColumnIndex);
            ActivateSelection();
            MoveCursorTo(position);
            CompleteSelection();
        }
        var (selectionStart, selectionEnd) = TextSelection.GetSortedPositions();
        var deletedSelectedText = GetSelectedText();
        var deleteResult = RightDelete();
        var newCursorPosition = TextCursor.Position;

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText, deleteResult.HasDeleted);
    }

    public AppendNewLineResult AppendNewLine()
    {
        var oldCursorPosition = TextCursor.Position;
        var (selectionStart, selectionEnd) = TextSelection.GetSortedPositions();
        var deletedSelectedText = GetSelectedText();
        if (TextSelection.IsExist) DeleteSelection();
        Text.AppendNewLine(TextCursor.Position);
        Tokens.InsertEmptyLine(TextCursor.LineIndex + 1);
        TextCursor.MoveDown();
        TextCursor.MoveStartLine();
        var newCursorPosition = TextCursor.Position;
        UpdateTokensForLines(TextCursor.LineIndex - 1, 2);

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText);
    }

    public AppendCharResult AppendChar(char ch)
    {
        var oldCursorPosition = TextCursor.Position;
        var (selectionStart, selectionEnd) = TextSelection.GetSortedPositions();
        var deletedSelectedText = GetSelectedText();
        if (TextSelection.IsExist) DeleteSelection();
        Text.AppendChar(TextCursor.Position, ch);
        TextCursor.MoveRight();
        var newCursorPosition = TextCursor.Position;
        UpdateTokensForLines(TextCursor.LineIndex, 1);

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText, ch);
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

    public InsertTextResult InsertText(string text)
    {
        var insertedText = new Text(text);
        var oldCursorPosition = TextCursor.Position;
        var (selectionStart, selectionEnd) = TextSelection.GetSortedPositions();
        var deletedSelectedText = GetSelectedText();
        if (TextSelection.IsExist) DeleteSelection();
        var insertResult = Text.Insert(TextCursor.Position, insertedText);
        TextCursor.MoveTo(insertResult.EndPosition);
        var newCursorPosition = TextCursor.Position;
        UpdateTokensForLines(0, Text.LinesCount);

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText, insertResult.StartPosition, insertResult.EndPosition, text, insertResult.HasInserted);
    }

    public DeleteResult LeftDelete()
    {
        var charDeleteResult = default(Text.CharDeleteResult);
        var oldCursorPosition = TextCursor.Position;
        var (selectionStart, selectionEnd) = TextSelection.GetSortedPositions();
        var deletedSelectedText = GetSelectedText();
        if (TextSelection.IsExist)
        {
            DeleteSelection();
        }
        else
        {
            var newPosition = Text.GetCursorPositionAfterLeftDelete(TextCursor.Position);
            charDeleteResult = Text.LeftDelete(TextCursor.Position);
            if (charDeleteResult.IsLineDeleted)
            {
                Tokens.DeleteLine(TextCursor.LineIndex);
            }
            TextCursor.MoveTo(newPosition);
        }
        var newCursorPosition = TextCursor.Position;
        UpdateTokensForLines(TextCursor.LineIndex, 1);

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText, charDeleteResult);
    }

    public DeleteResult RightDelete()
    {
        var charDeleteResult = default(Text.CharDeleteResult);
        var oldCursorPosition = TextCursor.Position;
        var (selectionStart, selectionEnd) = TextSelection.GetSortedPositions();
        var deletedSelectedText = GetSelectedText();
        if (TextSelection.IsExist)
        {
            DeleteSelection();
        }
        else
        {
            charDeleteResult = Text.RightDelete(TextCursor.Position);
            if (charDeleteResult.IsLineDeleted)
            {
                Tokens.DeleteLine(TextCursor.LineIndex + 1);
            }
        }
        var newCursorPosition = TextCursor.Position;
        UpdateTokensForLines(TextCursor.LineIndex, 1);

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText, charDeleteResult);
    }

    private void DeleteSelection()
    {
        var deleteResult = Text.DeleteSelection(TextSelection);
        Tokens.DeleteLines(deleteResult.FirstDeletedLineIndex, deleteResult.DeletedLinesCount);
        var startCursorPosition = TextSelection.GetSortedPositions().Item1;
        TextCursor.MoveTo(startCursorPosition);
        TextSelection.Reset();
    }

    public DeleteSelectedLinesResult DeleteSelectedLines()
    {
        var oldCursorPosition = TextCursor.Position;
        var (selectionStart, selectionEnd) = TextSelection.GetSortedPositions();
        int startLineIndex, endLineIndex;
        string deletedSelectedText;
        if (TextSelection.IsExist)
        {
            startLineIndex = selectionStart.LineIndex;
            endLineIndex = selectionEnd.LineIndex;
            deletedSelectedText = String.Join(Environment.NewLine, Enumerable.Range(selectionStart.LineIndex, selectionEnd.LineIndex - selectionStart.LineIndex + 1).Select(Text.GetLine));
            TextSelection.Reset();
        }
        else
        {
            startLineIndex = endLineIndex = TextCursor.LineIndex;
            deletedSelectedText = Text.GetLine(TextCursor.LineIndex).ToString();
        }
        var oldTextLinesCount = Text.LinesCount;
        for (int i = startLineIndex; i <= endLineIndex; i++)
        {
            if (startLineIndex < Text.LinesCount - 1)
            {
                Text.DeleteLine(startLineIndex);
                Tokens.DeleteLine(startLineIndex);
            }
            else
            {
                Text.GetLine(startLineIndex).Clear();
                Tokens.GetTokens(startLineIndex).Clear();
            }
        }
        TextCursor.MoveTo(new(startLineIndex, 0));
        var newCursorPosition = TextCursor.Position;
        if (endLineIndex < oldTextLinesCount - 1)
        {
            deletedSelectedText += Environment.NewLine;
        }

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText);
    }

    public MoveSelectedLinesResult MoveSelectedLinesUp()
    {
        var oldCursorPosition = TextCursor.Position;
        var (selectionStart, selectionEnd) = TextSelection.GetSortedPositions();
        int sourceIndex, destinationIndex;
        if (TextSelection.IsExist)
        {
            sourceIndex = selectionStart.LineIndex - 1;
            if (sourceIndex < 0) return new(oldCursorPosition, oldCursorPosition, selectionStart, selectionEnd);
            destinationIndex = selectionEnd.LineIndex;
        }
        else
        {
            sourceIndex = TextCursor.LineIndex;
            destinationIndex = sourceIndex - 1;
            if (destinationIndex < 0) return new(oldCursorPosition, oldCursorPosition, selectionStart, selectionEnd);
        }
        Text.ReplaceLines(sourceIndex, destinationIndex);
        Tokens.ReplaceLines(sourceIndex, destinationIndex);
        TextCursor.MoveUp();
        TextSelection.StartCursorLineIndex--;
        TextSelection.EndCursorLineIndex--;
        var newCursorPosition = TextCursor.Position;

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd);
    }

    public MoveSelectedLinesResult MoveSelectedLinesDown()
    {
        var oldCursorPosition = TextCursor.Position;
        var (selectionStart, selectionEnd) = TextSelection.GetSortedPositions();
        int sourceIndex, destinationIndex;
        if (TextSelection.IsExist)
        {
            sourceIndex = selectionEnd.LineIndex + 1;
            if (sourceIndex >= Text.LinesCount) return new(oldCursorPosition, oldCursorPosition, selectionStart, selectionEnd);
            destinationIndex = selectionStart.LineIndex;
        }
        else
        {
            sourceIndex = TextCursor.LineIndex;
            destinationIndex = sourceIndex + 1;
            if (destinationIndex >= Text.LinesCount) return new(oldCursorPosition, oldCursorPosition, selectionStart, selectionEnd);
        }
        Text.ReplaceLines(sourceIndex, destinationIndex);
        Tokens.ReplaceLines(sourceIndex, destinationIndex);
        TextCursor.MoveDown();
        TextSelection.StartCursorLineIndex++;
        TextSelection.EndCursorLineIndex++;
        var newCursorPosition = TextCursor.Position;

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd);
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

    private void SetTokens()
    {
        var codeProviderTokens = _codeProvider.GetTokens(new ForwardTextIterator(Text)).ToList();
        Tokens.SetTokens(codeProviderTokens, 0, Text.LinesCount);
        TokenColors.SetColors(_codeProvider.GetColors());
    }

    private void UpdateTokensForLines(int startLineIndex, int count)
    {
        var codeProviderTokens = _codeProvider.GetTokens(new ForwardTextIterator(Text, startLineIndex, startLineIndex + count - 1)).ToList();
        Tokens.SetTokens(codeProviderTokens, startLineIndex, count);
    }
}
