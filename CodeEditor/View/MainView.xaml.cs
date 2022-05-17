using System.Windows;
using System.Windows.Controls.Primitives;
using CodeEditor.ViewModel;

namespace CodeEditor.View;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        verticalScrollBar.Loaded += (s, e) =>
        {
            var verticalScrollBarUpButton = (RepeatButton)verticalScrollBar.Template.FindName("PART_LineUpButton", verticalScrollBar);
            var verticalScrollBarDownButton = (RepeatButton)verticalScrollBar.Template.FindName("PART_LineDownButton", verticalScrollBar);
            verticalScrollBarUpButton.Click += (s, e) => { verticalScrollBar.Value -= 10.0; };
            verticalScrollBarDownButton.Click += (s, e) => { verticalScrollBar.Value += 10.0; };
        };
        horizontalScrollBar.Loaded += (s, e) =>
        {
            var horizontalScrollBarLeftButton = (RepeatButton)horizontalScrollBar.Template.FindName("PART_LineLeftButton", horizontalScrollBar);
            var horizontalScrollBarRightButton = (RepeatButton)horizontalScrollBar.Template.FindName("PART_LineRightButton", horizontalScrollBar);
            horizontalScrollBarLeftButton.Click += (s, e) => { horizontalScrollBar.Value -= 10.0; };
            horizontalScrollBarRightButton.Click += (s, e) => { horizontalScrollBar.Value += 10.0; };
        };
    }
}
