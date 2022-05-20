using CodeHighlighter.Commands;
using CodeHighlighter.Input;

namespace CodeHighlighter;

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
    public Command MoveSelectedLinesUpCommand { get; private set; }
    public Command MoveSelectedLinesDownCommand { get; private set; }
    public Command ScrollLineUpCommand { get; private set; }
    public Command ScrollLineDownCommand { get; private set; }
    public Command MoveToPrevTokenCommand { get; private set; }
    public Command MoveToNextTokenCommand { get; private set; }
    public Command DeleteLeftTokenCommand { get; private set; }
    public Command DeleteRightTokenCommand { get; private set; }
    public Command SelectAllCommand { get; private set; }
    public Command TextInputCommand { get; private set; }
    public Command NewLineCommand { get; private set; }
    public Command InsertTextCommand { get; private set; }
    public Command DeleteSelectedLinesCommand { get; private set; }
    public Command LeftDeleteCommand { get; private set; }
    public Command RightDeleteCommand { get; private set; }

    public CodeTextBoxCommands()
    {
        MoveCursorLeftCommand = new UninitializedCommand();
        MoveCursorRightCommand = new UninitializedCommand();
        MoveCursorUpCommand = new UninitializedCommand();
        MoveCursorDownCommand = new UninitializedCommand();
        MoveCursorStartLineCommand = new UninitializedCommand();
        MoveCursorEndLineCommand = new UninitializedCommand();
        MoveCursorTextBeginCommand = new UninitializedCommand();
        MoveCursorTextEndCommand = new UninitializedCommand();
        MoveCursorPageUpCommand = new UninitializedCommand();
        MoveCursorPageDownCommand = new UninitializedCommand();
        MoveSelectedLinesUpCommand = new UninitializedCommand();
        MoveSelectedLinesDownCommand = new UninitializedCommand();
        ScrollLineUpCommand = new UninitializedCommand();
        ScrollLineDownCommand = new UninitializedCommand();
        MoveToPrevTokenCommand = new UninitializedCommand();
        MoveToNextTokenCommand = new UninitializedCommand();
        DeleteLeftTokenCommand = new UninitializedCommand();
        DeleteRightTokenCommand = new UninitializedCommand();
        SelectAllCommand = new UninitializedCommand();
        TextInputCommand = new UninitializedCommand();
        NewLineCommand = new UninitializedCommand();
        InsertTextCommand = new UninitializedCommand();
        DeleteSelectedLinesCommand = new UninitializedCommand();
        LeftDeleteCommand = new UninitializedCommand();
        RightDeleteCommand = new UninitializedCommand();
    }

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
        MoveSelectedLinesUpCommand = new MoveSelectedLinesUpCommand(context);
        MoveSelectedLinesDownCommand = new MoveSelectedLinesDownCommand(context);
        ScrollLineUpCommand = new ScrollLineUpCommand(context);
        ScrollLineDownCommand = new ScrollLineDownCommand(context);
        MoveToPrevTokenCommand = new MoveToPrevTokenCommand(context);
        MoveToNextTokenCommand = new MoveToNextTokenCommand(context);
        DeleteLeftTokenCommand = new DeleteLeftTokenCommand(context);
        DeleteRightTokenCommand = new DeleteRightTokenCommand(context);
        SelectAllCommand = new SelectAllCommand(context);
        TextInputCommand = new TextInputCommand(context);
        NewLineCommand = new NewLineCommand(context);
        InsertTextCommand = new InsertTextCommand(context);
        DeleteSelectedLinesCommand = new DeleteSelectedLinesCommand(context);
        LeftDeleteCommand = new LeftDeleteCommand(context);
        RightDeleteCommand = new RightDeleteCommand(context);
    }
}
