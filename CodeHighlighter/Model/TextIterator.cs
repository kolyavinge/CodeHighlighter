namespace CodeHighlighter.Model
{
    internal class TextIterator : ITextIterator
    {
        private readonly IText _text;
        private readonly int _endLineIndex;

        public char Char { get; private set; }

        public int LineIndex { get; private set; }

        public int ColumnIndex { get; private set; }

        public bool Eof { get; private set; }

        public char NextChar
        {
            get
            {
                if (Eof) return (char)0;
                if (IsReturn) return (char)0;
                var line = _text.GetLine(LineIndex);
                if (ColumnIndex == line.Length - 1 && LineIndex == _endLineIndex) return (char)0;
                if (ColumnIndex == line.Length - 1) return '\n';
                return line[ColumnIndex + 1];
            }
        }

        private bool IsReturn => Char == '\n';

        public TextIterator(IText text) : this(text, 0, text.LinesCount - 1)
        {
        }

        public TextIterator(IText text, int startLineIndex, int endLineIndex)
        {
            _text = text;
            _endLineIndex = endLineIndex;
            LineIndex = startLineIndex;
            ColumnIndex = -1;
            if (endLineIndex - startLineIndex >= 0)
            {
                MoveNext();
            }
            else
            {
                Eof = true;
            }
        }

        public void MoveNext()
        {
            if (Eof) return;
            if (IsReturn)
            {
                ColumnIndex = 0;
                LineIndex++;
            }
            else
            {
                ColumnIndex++;
            }
            var line = _text.GetLine(LineIndex);
            if (ColumnIndex < line.Length)
            {
                Char = line[ColumnIndex];
            }
            else if (ColumnIndex == line.Length && LineIndex < _endLineIndex)
            {
                Char = '\n';
            }
            else
            {
                Char = (char)0;
                Eof = true;
            }
        }
    }
}
