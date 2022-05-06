﻿using CodeHighlighter.Commands;
using CodeHighlighter.Input;

namespace CodeHighlighter
{
    public class CodeTextBoxCommands
    {
        public Command MoveCursorLeftCommand { get; private set; }
        public Command MoveCursorRightCommand { get; private set; }
        public Command MoveCursorUpCommand { get; private set; }
        public Command MoveCursorDownCommand { get; private set; }
        public Command MoveCursorStartLineCommand { get; private set; }
        public Command MoveCursorEndLineCommand { get; private set; }
        public Command MoveCursorTextBeginCommand { get; private set; }
        public Command MoveCursorTextEndCommand { get; private set; }
        public Command MoveCursorPageUpCommand { get; private set; }
        public Command MoveCursorPageDownCommand { get; private set; }
        public Command ScrollLineUpCommand { get; private set; }
        public Command ScrollLineDownCommand { get; private set; }
        public Command SelectAllCommand { get; private set; }
        public Command NewLineCommand { get; private set; }
        public Command InsertTextCommand { get; private set; }
        public Command DeleteSelectedLinesCommand { get; private set; }
        public Command LeftDeleteCommand { get; private set; }
        public Command RightDeleteCommand { get; private set; }

        internal void Init(InputCommandContext context)
        {
            MoveCursorLeftCommand = new MoveCursorLeftCommand(context);
            MoveCursorRightCommand = new MoveCursorRightCommand(context);
            MoveCursorUpCommand = new MoveCursorUpCommand(context);
            MoveCursorDownCommand = new MoveCursorDownCommand(context);
            MoveCursorStartLineCommand = new MoveCursorStartLineCommand(context);
            MoveCursorEndLineCommand = new MoveCursorEndLineCommand(context);
            MoveCursorTextBeginCommand = new MoveCursorTextBeginCommand(context);
            MoveCursorTextEndCommand = new MoveCursorTextEndCommand(context);
            MoveCursorPageUpCommand = new MoveCursorPageUpCommand(context);
            MoveCursorPageDownCommand = new MoveCursorPageDownCommand(context);
            ScrollLineUpCommand = new ScrollLineUpCommand(context);
            ScrollLineDownCommand = new ScrollLineDownCommand(context);
            SelectAllCommand = new SelectAllCommand(context);
            NewLineCommand = new NewLineCommand(context);
            InsertTextCommand = new InsertTextCommand(context);
            DeleteSelectedLinesCommand = new DeleteSelectedLinesCommand(context);
            LeftDeleteCommand = new LeftDeleteCommand(context);
            RightDeleteCommand = new RightDeleteCommand(context);
        }
    }
}
