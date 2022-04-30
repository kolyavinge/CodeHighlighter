using System;
using System.Collections.Generic;
using System.Linq;

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
                _textSelection.EndLineIndex = _textCursor.LineIndex;
                _textSelection.EndColumnIndex = _textCursor.ColumnIndex;
            }
        }

        public void SelectLexem(int lineIndex, int columnIndex)
        {
            var lexem = _lexems.GetLexem(lineIndex, columnIndex);
            _textSelection.Reset();
            _textSelection.StartLineIndex = lineIndex;
            _textSelection.StartColumnIndex = lexem.StartColumnIndex;
            _textSelection.EndLineIndex = lineIndex;
            _textSelection.EndColumnIndex = lexem.EndColumnIndex;
        }

        public void NewLine()
        {
            if (_textSelection.IsExist) DeleteSelection();
            _text.NewLine(_textCursor.LineIndex, _textCursor.ColumnIndex);
            _lexems.InsertEmptyLine(_textCursor.LineIndex + 1);
            _textCursor.MoveDown();
            _textCursor.MoveStartLine();
            UpdateLexemsForLines(_textCursor.LineIndex - 1, 2);
        }

        public void AppendChar(char ch)
        {
            if (_textSelection.IsExist) DeleteSelection();
            _text.AppendChar(_textCursor.LineIndex, _textCursor.ColumnIndex, ch);
            _textCursor.MoveRight();
            UpdateLexemsForLines(_textCursor.LineIndex, 1);
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
            UpdateLexemsForLines(0, _text.LinesCount);
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
                    _lexems.DeleteLine(_textCursor.LineIndex);
                }
                _textCursor.MoveTo(newLineIndex, newColumnIndex);
            }
            UpdateLexemsForLines(_textCursor.LineIndex, 1);
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
                    _lexems.DeleteLine(_textCursor.LineIndex + 1);
                }
            }
            UpdateLexemsForLines(_textCursor.LineIndex, 1);
        }

        private void DeleteSelection()
        {
            var deleteResult = _text.DeleteSelection(_textSelection);
            _lexems.DeleteLines(deleteResult.FirstDeletedLineIndex, deleteResult.DeletedLinesCount);
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
                    _lexems.DeleteLine(start.LineIndex);
                }
                else
                {
                    _text.GetLine(start.LineIndex).Clear();
                    _lexems.GetMergedLexems(start.LineIndex).Clear();
                }
            }
            _textCursor.MoveTo(start.LineIndex, start.ColumnIndex);
        }

        private void SetLexems()
        {
            var codeProviderLexems = _codeProvider.GetLexems(new TextIterator(_text)).ToList();
            _lexems.SetLexems(codeProviderLexems, 0, _text.LinesCount);
            _lexemColors.SetColors(_codeProvider.GetColors());
        }

        private void UpdateLexemsForLines(int startLineIndex, int count)
        {
            var codeProviderLexems = _codeProvider.GetLexems(new TextIterator(_text, startLineIndex, startLineIndex + count - 1)).ToList();
            _lexems.SetLexems(codeProviderLexems, startLineIndex, count);
        }
    }
}
