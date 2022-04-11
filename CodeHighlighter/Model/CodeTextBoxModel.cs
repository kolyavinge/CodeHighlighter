﻿using System.Linq;

namespace CodeHighlighter.Model
{
    internal class CodeTextBoxModel
    {
        private readonly Text _text;
        private readonly TextCursor _textCursor;
        private readonly TextSelection _textSelection;
        private readonly Lexems _lexems;
        private readonly LexemsColors _lexemColors;
        private ICodeProvider _codeProvider;

        public IText Text => _text;
        public ITextCursor TextCursor => _textCursor;
        public ITextSelection TextSelection => _textSelection;
        public ILexems Lexems => _lexems;
        public ILexemsColors LexemColors => _lexemColors;

        public CodeTextBoxModel()
        {
            _text = new();
            _textCursor = new(_text);
            _textSelection = new();
            _lexems = new();
            _lexemColors = new();
            _codeProvider = new CodeProviders.EmptyCodeProvider();
        }

        public void SetCodeProvider(ICodeProvider codeProvider)
        {
            _codeProvider = codeProvider;
            SetLexems();
        }

        public void SetText(string text)
        {
            _text.SetText(text);
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
            _textSelection.StartColumnIndex = 0;
            _textSelection.EndLineIndex = _text.LinesCount - 1;
            _textSelection.EndColumnIndex = _text.GetLine(_textSelection.EndLineIndex).Length;
            _textCursor.MoveTextEnd();
        }

        public void StartSelection()
        {
            _textSelection.InProgress = true;
        }

        public void EndSelection()
        {
            _textSelection.InProgress = false;
        }

        private void SetSelection()
        {
            if (_textSelection.InProgress)
            {
                _textSelection.EndLineIndex = _textCursor.LineIndex;
                _textSelection.EndColumnIndex = _textCursor.ColumnIndex;
            }
            else
            {
                _textSelection.StartLineIndex = _textCursor.LineIndex;
                _textSelection.StartColumnIndex = _textCursor.ColumnIndex;
                _textSelection.EndLineIndex = -1;
            }
        }

        public void NewLine()
        {
            _text.NewLine(_textCursor.LineIndex, _textCursor.ColumnIndex);
            _lexems.InsertEmpty(_textCursor.LineIndex + 1);
            _textCursor.MoveDown();
            _textCursor.MoveStartLine();
            UpdateLexemsForLines(_textCursor.LineIndex - 1, 2);
        }

        public void AppendChar(char ch)
        {
            _text.AppendChar(_textCursor.LineIndex, _textCursor.ColumnIndex, ch);
            _textCursor.MoveRight();
            UpdateLexemsForLines(_textCursor.LineIndex, 1);
        }

        public void LeftDelete()
        {
            (int newLineIndex, int newColumnIndex) = _text.GetCursorPositionAfterLeftDelete(_textCursor.LineIndex, _textCursor.ColumnIndex);
            var deleteResult = _text.LeftDelete(_textCursor.LineIndex, _textCursor.ColumnIndex);
            if (deleteResult.IsLineDeleted)
            {
                _lexems.RemoveAt(_textCursor.LineIndex);
            }
            _textCursor.MoveTo(newLineIndex, newColumnIndex);
            UpdateLexemsForLines(_textCursor.LineIndex, 1);
        }

        public void RightDelete()
        {
            var deleteResult = _text.RightDelete(_textCursor.LineIndex, _textCursor.ColumnIndex);
            if (deleteResult.IsLineDeleted)
            {
                _lexems.RemoveAt(_textCursor.LineIndex + 1);
            }
            UpdateLexemsForLines(_textCursor.LineIndex, 1);
        }

        public void SetLexems()
        {
            _lexems.SetLexems(_text, _codeProvider.GetLexems(new TextIterator(_text)).ToList());
            _lexemColors.SetColors(_codeProvider.GetColors());
        }

        private void UpdateLexemsForLines(int startLineIndex, int count)
        {
            _lexems.ReplaceLexems(_text, _codeProvider.GetLexems(new TextIterator(_text, startLineIndex, startLineIndex + count - 1)).ToList());
        }
    }
}
