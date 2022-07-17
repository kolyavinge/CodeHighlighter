using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model;

internal class InputModel : ITextSource, ITextSelectionActivator, ITokenSelector, ICursorHandler
{
    private readonly Text _text;
    private readonly TextCursor _textCursor;
    private readonly TextSelection _textSelection;
    private readonly Tokens _tokens;
    private readonly TokensColors _tokenColors;
    private ICodeProvider _codeProvider;

    public IText Text => _text;
    public ITextCursor TextCursor => _textCursor;
    public ITextSelection TextSelection => _textSelection;
    public ITokens Tokens => _tokens;
    public ITokensColors TokenColors => _tokenColors;

    public InputModel(Text text, TextCursor textCursor, TextSelection textSelection, Tokens tokens)
    {
        _text = text;
        _textCursor = textCursor;
        _textSelection = textSelection;
        _tokens = tokens;
        _tokenColors = new();
        _codeProvider = new CodeProviders.EmptyCodeProvider();
    }

    public InputModel()
    {
        _text = new();
        _textCursor = new(_text);
        _textSelection = new();
        _tokens = new();
        _tokenColors = new();
        _codeProvider = new CodeProviders.EmptyCodeProvider();
    }

    public void SetCodeProvider(ICodeProvider codeProvider)
    {
        _codeProvider = codeProvider;
        SetTokens();
    }

    public void SetText(string text)
    {
        _text.TextContent = text;
        SetTokens();
    }

    public void MoveCursorTo(int lineIndex, int columnIndex)
    {
        _textCursor.MoveTo(lineIndex, columnIndex);
        SetSelection();
    }

    public void MoveCursorUp()
    {
        _textCursor.MoveUp();
        SetSelection();
    }

    public void MoveCursorDown()
    {
        _textCursor.MoveDown();
        SetSelection();
    }

    public void MoveCursorLeft()
    {
        _textCursor.MoveLeft();
        SetSelection();
    }

    public void MoveCursorRight()
    {
        _textCursor.MoveRight();
        SetSelection();
    }

    public void MoveCursorStartLine()
    {
        _textCursor.MoveStartLine();
        SetSelection();
    }

    public void MoveCursorTextBegin()
    {
        _textCursor.MoveTextBegin();
        SetSelection();
    }

    public void MoveCursorEndLine()
    {
        _textCursor.MoveEndLine();
        SetSelection();
    }

    public void MoveCursorTextEnd()
    {
        _textCursor.MoveTextEnd();
        SetSelection();
    }

    public void MoveCursorPageUp(int pageSize)
    {
        _textCursor.MovePageUp(pageSize);
        SetSelection();
    }

    public void MoveCursorPageDown(int pageSize)
    {
        _textCursor.MovePageDown(pageSize);
        SetSelection();
    }

    public void SelectAll()
    {
        _textSelection.InProgress = false;
        _textSelection.StartLineIndex = 0;
        _textSelection.StartCursorColumnIndex = 0;
        _textSelection.EndLineIndex = _text.LinesCount - 1;
        _textSelection.EndCursorColumnIndex = _text.GetLine(_textSelection.EndLineIndex).Length;
        _textCursor.MoveTextEnd();
    }

    public void ActivateSelection()
    {
        if (!_textSelection.InProgress)
        {
            _textSelection.InProgress = true;
            _textSelection.StartLineIndex = _textCursor.LineIndex;
            _textSelection.StartCursorColumnIndex = _textCursor.ColumnIndex;
            _textSelection.EndLineIndex = _textCursor.LineIndex;
            _textSelection.EndCursorColumnIndex = _textCursor.ColumnIndex;
        }
    }

    public void CompleteSelection()
    {
        _textSelection.InProgress = false;
    }

    private void SetSelection()
    {
        if (_textSelection.InProgress)
        {
            _textSelection.EndLineIndex = _textCursor.LineIndex;
            _textSelection.EndCursorColumnIndex = _textCursor.ColumnIndex;
        }
        else
        {
            _textSelection.StartLineIndex = _textCursor.LineIndex;
            _textSelection.StartCursorColumnIndex = _textCursor.ColumnIndex;
            _textSelection.EndLineIndex = _textCursor.LineIndex;
            _textSelection.EndCursorColumnIndex = _textCursor.ColumnIndex;
        }
    }

    public void SelectToken(int lineIndex, int columnIndex)
    {
        var selector = new TokenSelector();
        var range = selector.GetSelection(_tokens, lineIndex, columnIndex);
        _textSelection.Reset();
        _textSelection.StartLineIndex = lineIndex;
        _textSelection.StartCursorColumnIndex = range.StartCursorColumnIndex;
        _textSelection.EndLineIndex = lineIndex;
        _textSelection.EndCursorColumnIndex = range.EndCursorColumnIndex;
    }

    public void MoveToNextToken()
    {
        var navigator = new TokenNavigator();
        var pos = navigator.MoveRight(_text, _tokens, _textCursor.LineIndex, _textCursor.ColumnIndex);
        _textCursor.MoveTo(pos.LineIndex, pos.ColumnIndex);
        SetSelection();
    }

    public void MoveToPrevToken()
    {
        var navigator = new TokenNavigator();
        var pos = navigator.MoveLeft(_text, _tokens, _textCursor.LineIndex, _textCursor.ColumnIndex);
        _textCursor.MoveTo(pos.LineIndex, pos.ColumnIndex);
        SetSelection();
    }

    public void DeleteLeftToken()
    {
        if (_textSelection.IsExist)
        {
            LeftDelete();
        }
        else
        {
            var navigator = new TokenNavigator();
            var pos = navigator.MoveLeft(_text, _tokens, _textCursor.LineIndex, _textCursor.ColumnIndex);
            ActivateSelection();
            MoveCursorTo(pos.LineIndex, pos.ColumnIndex);
            CompleteSelection();
            LeftDelete();
        }
    }

    public void DeleteRightToken()
    {
        if (_textSelection.IsExist)
        {
            RightDelete();
        }
        else
        {
            var navigator = new TokenNavigator();
            var pos = navigator.MoveRight(_text, _tokens, _textCursor.LineIndex, _textCursor.ColumnIndex);
            ActivateSelection();
            MoveCursorTo(pos.LineIndex, pos.ColumnIndex);
            CompleteSelection();
            LeftDelete();
        }
    }

    public void NewLine()
    {
        if (_textSelection.IsExist) DeleteSelection();
        _text.NewLine(_textCursor.LineIndex, _textCursor.ColumnIndex);
        _tokens.InsertEmptyLine(_textCursor.LineIndex + 1);
        _textCursor.MoveDown();
        _textCursor.MoveStartLine();
        UpdateTokensForLines(_textCursor.LineIndex - 1, 2);
    }

    public void AppendChar(char ch)
    {
        if (_textSelection.IsExist) DeleteSelection();
        _text.AppendChar(_textCursor.LineIndex, _textCursor.ColumnIndex, ch);
        _textCursor.MoveRight();
        UpdateTokensForLines(_textCursor.LineIndex, 1);
    }

    public string GetSelectedText()
    {
        if (!_textSelection.IsExist) return "";
        var selectedLines = new List<string>();
        foreach (var line in _textSelection.GetSelectedLines(_text))
        {
            selectedLines.Add(_text.GetSubstring(line.LineIndex, line.LeftColumnIndex, line.RightColumnIndex - line.LeftColumnIndex));
        }

        return String.Join(Environment.NewLine, selectedLines);
    }

    public void InsertText(string text)
    {
        var insertedText = new Text(text);
        if (_textSelection.IsExist) DeleteSelection();
        _text.Insert(_textCursor.LineIndex, _textCursor.ColumnIndex, insertedText);
        if (insertedText.LinesCount == 1)
        {
            _textCursor.MoveTo(_textCursor.LineIndex, _textCursor.ColumnIndex + insertedText.GetLastLine().Length);
        }
        else
        {
            _textCursor.MoveTo(_textCursor.LineIndex + insertedText.LinesCount - 1, insertedText.GetLastLine().Length);
        }
        UpdateTokensForLines(0, _text.LinesCount);
    }

    public void LeftDelete()
    {
        if (_textSelection.IsExist)
        {
            DeleteSelection();
        }
        else
        {
            (int newLineIndex, int newColumnIndex) = _text.GetCursorPositionAfterLeftDelete(_textCursor.LineIndex, _textCursor.ColumnIndex);
            var deleteResult = _text.LeftDelete(_textCursor.LineIndex, _textCursor.ColumnIndex);
            if (deleteResult.IsLineDeleted)
            {
                _tokens.DeleteLine(_textCursor.LineIndex);
            }
            _textCursor.MoveTo(newLineIndex, newColumnIndex);
        }
        UpdateTokensForLines(_textCursor.LineIndex, 1);
    }

    public void RightDelete()
    {
        if (_textSelection.IsExist)
        {
            DeleteSelection();
        }
        else
        {
            var deleteResult = _text.RightDelete(_textCursor.LineIndex, _textCursor.ColumnIndex);
            if (deleteResult.IsLineDeleted)
            {
                _tokens.DeleteLine(_textCursor.LineIndex + 1);
            }
        }
        UpdateTokensForLines(_textCursor.LineIndex, 1);
    }

    private void DeleteSelection()
    {
        var deleteResult = _text.DeleteSelection(_textSelection);
        _tokens.DeleteLines(deleteResult.FirstDeletedLineIndex, deleteResult.DeletedLinesCount);
        var startCursorPosition = _textSelection.GetSortedPositions().Item1;
        _textCursor.MoveTo(startCursorPosition.LineIndex, startCursorPosition.ColumnIndex);
        _textSelection.Reset();
    }

    public void DeleteSelectedLines()
    {
        TextSelectionPosition start, end;
        if (_textSelection.IsExist)
        {
            (start, end) = _textSelection.GetSortedPositions();
            _textSelection.Reset();
        }
        else
        {
            start = end = new TextSelectionPosition(_textCursor.LineIndex, _textCursor.ColumnIndex);
        }
        for (int i = start.LineIndex; i <= end.LineIndex; i++)
        {
            if (start.LineIndex < _text.LinesCount - 1)
            {
                _text.DeleteLine(start.LineIndex);
                _tokens.DeleteLine(start.LineIndex);
            }
            else
            {
                _text.GetLine(start.LineIndex).Clear();
                _tokens.GetTokens(start.LineIndex).Clear();
            }
        }
        _textCursor.MoveTo(start.LineIndex, start.ColumnIndex);
    }

    public void MoveSelectedLinesUp()
    {
        int sourceIndex, destinationIndex;
        if (_textSelection.IsExist)
        {
            (var start, var end) = _textSelection.GetSortedPositions();
            sourceIndex = start.LineIndex - 1;
            if (sourceIndex < 0) return;
            destinationIndex = end.LineIndex;
        }
        else
        {
            sourceIndex = _textCursor.LineIndex;
            destinationIndex = sourceIndex - 1;
            if (destinationIndex < 0) return;
        }
        _text.ReplaceLines(sourceIndex, destinationIndex);
        _tokens.ReplaceLines(sourceIndex, destinationIndex);
        _textCursor.MoveUp();
        _textSelection.StartLineIndex--;
        _textSelection.EndLineIndex--;
    }

    public void MoveSelectedLinesDown()
    {
        int sourceIndex, destinationIndex;
        if (_textSelection.IsExist)
        {
            (var start, var end) = _textSelection.GetSortedPositions();
            sourceIndex = end.LineIndex + 1;
            if (sourceIndex >= _text.LinesCount) return;
            destinationIndex = start.LineIndex;
        }
        else
        {
            sourceIndex = _textCursor.LineIndex;
            destinationIndex = sourceIndex + 1;
            if (destinationIndex >= _text.LinesCount) return;
        }
        _text.ReplaceLines(sourceIndex, destinationIndex);
        _tokens.ReplaceLines(sourceIndex, destinationIndex);
        _textCursor.MoveDown();
        _textSelection.StartLineIndex++;
        _textSelection.EndLineIndex++;
    }

    public void SetSelectedTextCase(TextCase textCase)
    {
        _text.SetSelectedTextCase(_textSelection, textCase);
    }

    private void SetTokens()
    {
        var codeProviderTokens = _codeProvider.GetTokens(new ForwardTextIterator(_text)).ToList();
        _tokens.SetTokens(codeProviderTokens, 0, _text.LinesCount);
        _tokenColors.SetColors(_codeProvider.GetColors());
    }

    private void UpdateTokensForLines(int startLineIndex, int count)
    {
        var codeProviderTokens = _codeProvider.GetTokens(new ForwardTextIterator(_text, startLineIndex, startLineIndex + count - 1)).ToList();
        _tokens.SetTokens(codeProviderTokens, startLineIndex, count);
    }
}

internal interface ITextSource
{
    string GetSelectedText();
}

internal interface ITextSelectionActivator
{
    void ActivateSelection();
    void CompleteSelection();
}

internal interface ITokenSelector
{
    void SelectToken(int lineIndex, int columnIndex);
}

internal interface ICursorHandler
{
    void MoveCursorTo(int lineIndex, int columnIndex);
}
