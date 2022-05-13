using CodeHighlighter.Model;

namespace CodeHighlighter
{
    public static class TextIteratorBuilder
    {
        public static ITextIterator FromString(string text)
        {
            return new ForwardTextIterator(new Text(text));
        }
    }
}
