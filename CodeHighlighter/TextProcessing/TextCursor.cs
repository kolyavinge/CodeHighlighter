using System.Windows;
using System.Windows.Media;

namespace CodeHighlighter.TextProcessing
{
    internal class TextCursor
    {
        public static readonly Pen BlackPen = new(Brushes.Black, 1.5);

        private readonly Text _text;
        private readonly TextMeasures _textMeasures;

        public int LineIndex { get; private set; }

        public int ColumnIndex { get; private set; }

        public Point AbsolutePoint => new(ColumnIndex * _textMeasures.LetterWidth, LineIndex * _textMeasures.LineHeight);

        public TextCursor(Text text, TextMeasures textMeasures)
        {
            _text = text;
            _textMeasures = textMeasures;
            LineIndex = -1;
        }

        public void MoveByClick(double x, double y)
        {
            LineIndex = (int)(y / _textMeasures.LineHeight);
            ColumnIndex = (int)(x / _textMeasures.LetterWidth);
            if (LineIndex >= _text.LinesCount) LineIndex = _text.LinesCount - 1;
            if (ColumnIndex > _text.GetLine(LineIndex).Length) ColumnIndex = _text.GetLine(LineIndex).Length;
        }

        public void MoveByUp()
        {
            LineIndex--;
            CorrectPosition();
        }

        public void MoveByDown()
        {
            LineIndex++;
            CorrectPosition();
        }

        public void MoveByLeft()
        {
            ColumnIndex--;
            if (ColumnIndex == -1)
            {
                LineIndex--;
                ColumnIndex = int.MaxValue;
            }
            CorrectPosition();
        }

        public void MoveByRight()
        {
            ColumnIndex++;
            if (ColumnIndex == _text.GetLine(LineIndex).Length + 1)
            {
                LineIndex++;
                ColumnIndex = 0;
            }
            CorrectPosition();
        }

        public void MoveByHome()
        {
            ColumnIndex = 0;
            CorrectPosition();
        }

        public void MoveByEnd()
        {
            ColumnIndex = _text.GetLine(LineIndex).Length;
            CorrectPosition();
        }

        public void MoveByPageUp(int pageSize)
        {
            LineIndex -= pageSize;
            CorrectPosition();
        }

        public void MoveByPageDown(int pageSize)
        {
            LineIndex += pageSize;
            CorrectPosition();
        }

        public void GotoTextBegin()
        {
            ColumnIndex = 0;
            LineIndex = 0;
        }

        public void GotoTextEnd()
        {
            ColumnIndex = 0;
            LineIndex = _text.LinesCount - 1;
        }

        private void CorrectPosition()
        {
            if (LineIndex < 0) LineIndex = 0;
            if (LineIndex >= _text.LinesCount) LineIndex = _text.LinesCount - 1;
            if (ColumnIndex < 0) ColumnIndex = 0;
            if (ColumnIndex > _text.GetLine(LineIndex).Length) ColumnIndex = _text.GetLine(LineIndex).Length;
        }
    }
}
