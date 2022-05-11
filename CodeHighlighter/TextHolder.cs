using System;

namespace CodeHighlighter
{
    public class TextHolder
    {
        internal Func<string>? GetTextAction;

        internal Action<string>? SetTextAction;

        public TextHolder() { }

        public TextHolder(string initialText)
        {
            GetTextAction = () => initialText;
        }

        public string TextValue
        {
            get
            {
                if (GetTextAction == null) throw new InvalidOperationException("TextHolder is not initialyzed yet");
                return GetTextAction();
            }
            set
            {
                if (SetTextAction == null) throw new InvalidOperationException("TextHolder is not initialyzed yet");
                SetTextAction(value);
            }
        }
    }
}
