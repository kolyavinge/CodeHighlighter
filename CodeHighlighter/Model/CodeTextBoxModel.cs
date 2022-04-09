namespace CodeHighlighter.Model
{
    internal class CodeTextBoxModel
    {
        private readonly TextCursor _textCursor;
        private readonly Text _text;
        private readonly TextMeasures _textMeasures;
        private readonly FontSettings _fontSettings;
        private readonly Lexems _lexems;
        private readonly LexemsColors _lexemColors;

        public IFontSettings FontSettings => _fontSettings;
        public IText Text => _text;
        public ILexems Lexems => _lexems;
        public ILexemsColors LexemColors => _lexemColors;
        public ITextMeasures TextMeasures => _textMeasures;
        public ITextCursor TextCursor => _textCursor;
        public Viewport? Viewport { get; set; }

        public CodeTextBoxModel(FontSettings fontSettings)
        {
            _fontSettings = fontSettings;
            _textMeasures = new(FontSettings);
            _text = new();
            _textCursor = new(_text, _textMeasures);
            _lexems = new();
            _lexemColors = new();
        }

        public void SetText(string text)
        {
            _text.SetText(text);
        }

        public void SetFontSettings(FontSettings fontSettings)
        {
            _fontSettings.FontFamily = fontSettings.FontFamily;
            _fontSettings.FontSize = fontSettings.FontSize;
            _fontSettings.FontStretch = fontSettings.FontStretch;
            _fontSettings.FontStyle = fontSettings.FontStyle;
            _fontSettings.FontWeight = fontSettings.FontWeight;
            _textMeasures.UpdateMeasures();
        }

        public void MoveCursorByClick(double x, double y)
        {
            _textCursor.MoveByClick(x, y);
        }

        public void MoveCursorUp()
        {
            _textCursor.MoveUp();
        }

        public void MoveCursorDown()
        {
            _textCursor.MoveDown();
        }

        public void MoveCursorLeft()
        {
            _textCursor.MoveLeft();
        }

        public void MoveCursorRight()
        {
            _textCursor.MoveRight();
        }

        public void MoveCursorHome()
        {
            _textCursor.MoveHome();
        }

        public void CursorGotoTextBegin()
        {
            _textCursor.GotoTextBegin();
        }

        public void MoveCursorEnd()
        {
            _textCursor.MoveEnd();
        }

        public void CursorGotoTextEnd()
        {
            _textCursor.GotoTextEnd();
        }

        public void MoveCursorPageUp()
        {
            _textCursor.MovePageUp(GetLinesCountInViewport());
        }

        public void MoveCursorPageDown()
        {
            _textCursor.MovePageDown(GetLinesCountInViewport());
        }

        public void UpdateLexems(ICodeProvider codeProvider)
        {
            _lexems.SetLexems(_text, codeProvider.GetLexems(new TextIterator(_text)));
            _lexemColors.SetColors(codeProvider.GetColors());
        }

        public int GetLinesCountInViewport()
        {
            var result = (int)(Viewport!.Height / _textMeasures.LineHeight) + 1;
            if (Viewport.Height % _textMeasures.LineHeight != 0) result++;

            return result;
        }

        public void CorrectViewport()
        {
            if (_textCursor.AbsolutePoint.X < Viewport!.HorizontalScrollBar!.Value)
            {
                Viewport.HorizontalScrollBar.Value = _textCursor.AbsolutePoint.X;
            }
            else if (_textCursor.AbsolutePoint.X + _textMeasures.LetterWidth > Viewport.HorizontalScrollBar.Value + Viewport.Width)
            {
                Viewport.HorizontalScrollBar.Value = _textCursor.AbsolutePoint.X - Viewport.Width + _textMeasures.LetterWidth;
            }

            if (_textCursor.AbsolutePoint.Y < Viewport.VerticalScrollBar!.Value)
            {
                Viewport.VerticalScrollBar.Value = _textCursor.AbsolutePoint.Y;
            }
            else if (_textCursor.AbsolutePoint.Y + _textMeasures.LineHeight > Viewport.VerticalScrollBar.Value + Viewport.Height)
            {
                Viewport.VerticalScrollBar.Value = _textCursor.AbsolutePoint.Y - Viewport.Height + _textMeasures.LineHeight;
            }
        }

        public void UpdateScrollbarsMaximumValues()
        {
            var maxLineWidthInPixels = _text.GetMaxLineWidth() * _textMeasures.LetterWidth;
            Viewport!.HorizontalScrollBar!.Maximum = Viewport!.Width < maxLineWidthInPixels ? maxLineWidthInPixels : 0;
            Viewport.VerticalScrollBar!.Maximum = _text.LinesCount * _textMeasures.LineHeight;
        }
    }
}
