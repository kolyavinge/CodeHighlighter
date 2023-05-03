using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CodeEditor.Mvvm;
using CodeHighlighter;
using CodeHighlighter.Ancillary;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Common;
using CodeHighlighter.Core;
using CodeHighlighter.Model;

namespace CodeEditor.ViewModel;

public class MainViewModel
{
    public ICodeTextBox CodeTextBoxModel { get; }

    public ILineNumberPanel LineNumberPanelModel { get; }

    public ILineFoldingPanel LineFoldingPanelModel { get; }

    public ISearchPanel SearchPanelModel { get; }

    public ICodeProvider CodeProvider { get; }

    public ICommand CopyTextCommand => new ActionCommand(CopyText);

    public ICommand InsertLineCommand => new ActionCommand(InsertLine);

    public string? SelectedLineToGoto { get; set; }

    public ICommand GotoLineCommand => new ActionCommand(GotoLine);

    public ICommand KeyDownCommand { get; }

    private bool _isReadOnly;
    public bool IsReadOnly
    {
        get => _isReadOnly;
        set { _isReadOnly = value; CodeTextBoxModel.IsReadOnly = value; }
    }

    private bool _isAlterLinesColor;
    public bool IsAlterLinesColor
    {
        get => _isAlterLinesColor;
        set
        {
            _isAlterLinesColor = value;
            if (value)
            {
                for (int i = 0; i < CodeTextBoxModel.TextLines.Count; i += 2)
                {
                    CodeTextBoxModel.LinesDecoration[i] = new LineDecoration { Background = new(250, 220, 160) };
                }
            }
            else
            {
                for (int i = 0; i < CodeTextBoxModel.TextLines.Count; i++)
                {
                    CodeTextBoxModel.LinesDecoration[i] = null;
                }
            }
        }
    }

    private bool _isGapEnabled;
    public bool IsGapEnabled
    {
        get => _isGapEnabled;
        set
        {
            _isGapEnabled = value;
            if (_isGapEnabled)
            {
                CodeTextBoxModel.Gaps[0] = new(3);
                CodeTextBoxModel.Gaps[3] = new(3);
                CodeTextBoxModel.Gaps[8] = new(2);
                CodeTextBoxModel.Gaps[12] = new(5);
                LineNumberPanelModel.Gaps[0] = new(3);
                LineNumberPanelModel.Gaps[3] = new(3);
                LineNumberPanelModel.Gaps[8] = new(2);
                LineNumberPanelModel.Gaps[12] = new(5);
            }
            else
            {
                CodeTextBoxModel.Gaps.Clear();
                LineNumberPanelModel.Gaps.Clear();
            }
            CodeTextBoxModel.Viewport.UpdateScrollBarsMaximumValues();
        }
    }

    private bool _isFoldEnabled;
    public bool IsFoldEnabled
    {
        get => _isFoldEnabled;
        set
        {
            _isFoldEnabled = value;
            if (_isFoldEnabled)
            {
                CodeTextBoxModel.Folds.Activate(CodeTextBoxModel.Folds.Items.Select(x => x.LineIndex));
            }
            else
            {
                CodeTextBoxModel.Folds.Deactivate(CodeTextBoxModel.Folds.Items.Select(x => x.LineIndex));
            }
        }
    }

    public MainViewModel()
    {
        CodeProvider = new SqlCodeProvider();
        CodeTextBoxModel = CodeTextBoxFactory.MakeModel(CodeProvider, new() { HighlighteredBrackets = "()[]" });
        CodeTextBoxModel.Text = File.ReadAllText(@"D:\Projects\CodeHighlighter\CodeEditor\Examples\sql.txt");
        CodeTextBoxModel.TextEvents.TextChanged += OnTextChanged;
        CodeTextBoxModel.Folds.Items = new LineFold[] { new(8, 13), new(37, 91), new(42, 8) };
        CodeTextBoxModel.TextHighlighter.Add(new TextHighlight[] { new(new(11, 4, 11, 14), new(168, 135, 200)), new(new(17, 4, 18, 6), new(107, 187, 199)) });
        LineNumberPanelModel = LineNumberPanelFactory.MakeModel(CodeTextBoxModel);
        LineFoldingPanelModel = LineFoldingPanelFactory.MakeModel(CodeTextBoxModel);
        SearchPanelModel = SearchPanelFactory.MakeModel(CodeTextBoxModel);
        SearchPanelModel.HighlightColor = Color.FromHex("c7a86b");
        SearchPanelModel.Pattern = "begin";
        KeyDownCommand = new ActionCommand<KeyEventArgs>(KeyDown);
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        //Debug.WriteLine($"{e.AddedLines.StartLineIndex + 1}:{e.AddedLines.LinesCount} / {e.DeletedLines.StartLineIndex + 1}:{e.DeletedLines.LinesCount}");
    }

    private void CopyText()
    {
        Clipboard.SetText(CodeTextBoxModel.Text.ToString());
    }

    private void InsertLine()
    {
        CodeTextBoxModel.MoveCursorTextEnd();
        CodeTextBoxModel.AppendNewLine();
        CodeTextBoxModel.InsertText("new inserted line");
    }

    private void GotoLine()
    {
        if (Int32.TryParse(SelectedLineToGoto ?? "", out int gotoLine))
        {
            CodeTextBoxModel.GotoLine(gotoLine - 1);
        }
    }

    private void KeyDown(KeyEventArgs e)
    {
        var key = e.Key == Key.System ? e.SystemKey : e.Key;
        var controlPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
        var altPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt;
        var shiftPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
        if (controlPressed && !altPressed && !shiftPressed && key == Key.L)
        {
            CodeTextBoxModel.DeleteSelectedLines();
            e.Handled = true;
        }
        else if (controlPressed && !altPressed && !shiftPressed && key == Key.Z)
        {
            CodeTextBoxModel.History.Undo();
            e.Handled = true;
        }
        else if (controlPressed && !altPressed && !shiftPressed && key == Key.Y)
        {
            CodeTextBoxModel.History.Redo();
            e.Handled = true;
        }
        else if (controlPressed && !altPressed && shiftPressed && key == Key.U)
        {
            CodeTextBoxModel.SetTextCase(TextCase.Upper);
            e.Handled = true;
        }
        else if (controlPressed && !altPressed && !shiftPressed && key == Key.U)
        {
            CodeTextBoxModel.SetTextCase(TextCase.Lower);
            e.Handled = true;
        }
        else if (!controlPressed && altPressed && !shiftPressed && key == Key.Up)
        {
            CodeTextBoxModel.MoveSelectedLinesUp();
            e.Handled = true;
        }
        else if (!controlPressed && altPressed && !shiftPressed && key == Key.Down)
        {
            CodeTextBoxModel.MoveSelectedLinesDown();
            e.Handled = true;
        }
    }
}
