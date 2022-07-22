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

    internal InputModel()
    {
        Text = new();
        TextCursor = new(Text);
        TextSelection = new();
        Tokens = new();
        TokenColors = new();
        _codeProvider = new EmptyCodeProvider();
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

    public void MoveCursorTo(int lineIndex, int columnIndex)
    {
        TextCursor.MoveTo(lineIndex, columnIndex);
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
        TextSelection.StartLineIndex = 0;
        TextSelection.StartCursorColumnIndex = 0;
        TextSelection.EndLineIndex = Text.LinesCount - 1;
        TextSelection.EndCursorColumnIndex = Text.GetLine(TextSelection.EndLineIndex).Length;
        TextCursor.MoveTextEnd();
    }

    public void ActivateSelection()
    {
        if (!TextSelection.InProgress)
        {
            TextSelection.InProgress = true;
            TextSelection.StartLineIndex = TextCursor.LineIndex;
            TextSelection.StartCursorColumnIndex = TextCursor.ColumnIndex;
            TextSelection.EndLineIndex = TextCursor.LineIndex;
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
            TextSelection.EndLineIndex = TextCursor.LineIndex;
            TextSelection.EndCursorColumnIndex = TextCursor.ColumnIndex;
        }
        else
        {
            TextSelection.StartLineIndex = TextCursor.LineIndex;
            TextSelection.StartCursorColumnIndex = TextCursor.ColumnIndex;
            TextSelection.EndLineIndex = TextCursor.LineIndex;
            TextSelection.EndCursorColumnIndex = TextCursor.ColumnIndex;
        }
    }

    public void SelectToken(int lineIndex, int columnIndex)
    {
        var selector = new TokenSelector();
        var range = selector.GetSelection(Tokens, lineIndex, columnIndex);
        TextSelection.Reset();
        TextSelection.StartLineIndex = lineIndex;
        TextSelection.StartCursorColumnIndex = range.StartCursorColumnIndex;
        TextSelection.EndLineIndex = lineIndex;
        TextSelection.EndCursorColumnIndex = range.EndCursorColumnIndex;
    }

    public void MoveToNextToken()
    {
        var navigator = new TokenNavigator();
        var pos = navigator.MoveRight(Text, Tokens, TextCursor.LineIndex, TextCursor.ColumnIndex);
        TextCursor.MoveTo(pos.LineIndex, pos.ColumnIndex);
        SetSelection();
    }

    public void MoveToPrevToken()
    {
        var navigator = new TokenNavigator();
        var pos = navigator.MoveLeft(Text, Tokens, TextCursor.LineIndex, TextCursor.ColumnIndex);
        TextCursor.MoveTo(pos.LineIndex, pos.ColumnIndex);
        SetSelection();
    }

    public void DeleteLeftToken()
    {
        if (TextSelection.IsExist)
        {
            LeftDelete();
        }
        else
        {
            var navigator = new TokenNavigator();
            var pos = navigator.MoveLeft(Text, Tokens, TextCursor.LineIndex, TextCursor.ColumnIndex);
            ActivateSelection();
            MoveCursorTo(pos.LineIndex, pos.ColumnIndex);
            CompleteSelection();
            LeftDelete();
        }
    }

    public void DeleteRightToken()
    {
        if (TextSelection.IsExist)
        {
            RightDelete();
        }
        else
        {
            var navigator = new TokenNavigator();
            var pos = navigator.MoveRight(Text, Tokens, TextCursor.LineIndex, TextCursor.ColumnIndex);
            ActivateSelection();
            MoveCursorTo(pos.LineIndex, pos.ColumnIndex);
            CompleteSelection();
            LeftDelete();
        }
    }

    public void NewLine()
    {
        if (TextSelection.IsExist) DeleteSelection();
        Text.NewLine(TextCursor.LineIndex, TextCursor.ColumnIndex);
        Tokens.InsertEmptyLine(TextCursor.LineIndex + 1);
        TextCursor.MoveDown();
        TextCursor.MoveStartLine();
        UpdateTokensForLines(TextCursor.LineIndex - 1, 2);
    }

    public void AppendChar(char ch)
    {
        if (TextSelection.IsExist) DeleteSelection();
        Text.AppendChar(TextCursor.LineIndex, TextCursor.ColumnIndex, ch);
        TextCursor.MoveRight();
        UpdateTokensForLines(TextCursor.LineIndex, 1);
    }

    public string GetSelectedText()
    {
        if (!TextSelection.IsExist) return "";
        var selectedLines = new List<string>();
        foreach (var line in TextSelection.GetSelectedLines(Text))
        {
            selectedLines.Add(Text.GetSubstring(line.LineIndex, line.LeftColumnIndex, line.RightColumnIndex - line.LeftColumnIndex));
        }

        return String.Join(Environment.NewLine, selectedLines);
    }

    public void InsertText(string text)
    {
        var insertedText = new Text(text);
        if (TextSelection.IsExist) DeleteSelection();
        Text.Insert(TextCursor.LineIndex, TextCursor.ColumnIndex, insertedText);
        if (insertedText.LinesCount == 1)
        {
            TextCursor.MoveTo(TextCursor.LineIndex, TextCursor.ColumnIndex + insertedText.GetLastLine().Length);
        }
        else
        {
            TextCursor.MoveTo(TextCursor.LineIndex + insertedText.LinesCount - 1, insertedText.GetLastLine().Length);
        }
        UpdateTokensForLines(0, Text.LinesCount);
    }

    public void LeftDelete()
    {
        if (TextSelection.IsExist)
        {
            DeleteSelection();
        }
        else
        {
            (int newLineIndex, int newColumnIndex) = Text.GetCursorPositionAfterLeftDelete(TextCursor.LineIndex, TextCursor.ColumnIndex);
            var deleteResult = Text.LeftDelete(TextCursor.LineIndex, TextCursor.ColumnIndex);
            if (deleteResult.IsLineDeleted)
            {
                Tokens.DeleteLine(TextCursor.LineIndex);
            }
            TextCursor.MoveTo(newLineIndex, newColumnIndex);
        }
        UpdateTokensForLines(TextCursor.LineIndex, 1);
    }

    public void RightDelete()
    {
        if (TextSelection.IsExist)
        {
            DeleteSelection();
        }
        else
        {
            var deleteResult = Text.RightDelete(TextCursor.LineIndex, TextCursor.ColumnIndex);
            if (deleteResult.IsLineDeleted)
            {
                Tokens.DeleteLine(TextCursor.LineIndex + 1);
            }
        }
        UpdateTokensForLines(TextCursor.LineIndex, 1);
    }

    private void DeleteSelection()
    {
        var deleteResult = Text.DeleteSelection(TextSelection);
        Tokens.DeleteLines(deleteResult.FirstDeletedLineIndex, deleteResult.DeletedLinesCount);
        var startCursorPosition = TextSelection.GetSortedPositions().Item1;
        TextCursor.MoveTo(startCursorPosition.LineIndex, startCursorPosition.ColumnIndex);
        TextSelection.Reset();
    }

    public void DeleteSelectedLines()
    {
        TextSelectionPosition start, end;
        if (TextSelection.IsExist)
        {
            (start, end) = TextSelection.GetSortedPositions();
            TextSelection.Reset();
        }
        else
        {
            start = end = new TextSelectionPosition(TextCursor.LineIndex, TextCursor.ColumnIndex);
        }
        for (int i = start.LineIndex; i <= end.LineIndex; i++)
        {
            if (start.LineIndex < Text.LinesCount - 1)
            {
                Text.DeleteLine(start.LineIndex);
                Tokens.DeleteLine(start.LineIndex);
            }
            else
            {
                Text.GetLine(start.LineIndex).Clear();
                Tokens.GetTokens(start.LineIndex).Clear();
            }
        }
        TextCursor.MoveTo(start.LineIndex, start.ColumnIndex);
    }

    public void MoveSelectedLinesUp()
    {
        int sourceIndex, destinationIndex;
        if (TextSelection.IsExist)
        {
            (var start, var end) = TextSelection.GetSortedPositions();
            sourceIndex = start.LineIndex - 1;
            if (sourceIndex < 0) return;
            destinationIndex = end.LineIndex;
        }
        else
        {
            sourceIndex = TextCursor.LineIndex;
            destinationIndex = sourceIndex - 1;
            if (destinationIndex < 0) return;
        }
        Text.ReplaceLines(sourceIndex, destinationIndex);
        Tokens.ReplaceLines(sourceIndex, destinationIndex);
        TextCursor.MoveUp();
        TextSelection.StartLineIndex--;
        TextSelection.EndLineIndex--;
    }

    public void MoveSelectedLinesDown()
    {
        int sourceIndex, destinationIndex;
        if (TextSelection.IsExist)
        {
            (var start, var end) = TextSelection.GetSortedPositions();
            sourceIndex = end.LineIndex + 1;
            if (sourceIndex >= Text.LinesCount) return;
            destinationIndex = start.LineIndex;
        }
        else
        {
            sourceIndex = TextCursor.LineIndex;
            destinationIndex = sourceIndex + 1;
            if (destinationIndex >= Text.LinesCount) return;
        }
        Text.ReplaceLines(sourceIndex, destinationIndex);
        Tokens.ReplaceLines(sourceIndex, destinationIndex);
        TextCursor.MoveDown();
        TextSelection.StartLineIndex++;
        TextSelection.EndLineIndex++;
    }

    public void SetSelectedTextCase(TextCase textCase)
    {
        Text.SetSelectedTextCase(TextSelection, textCase);
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
