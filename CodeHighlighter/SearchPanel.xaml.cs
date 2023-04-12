using System.Windows;
using CodeHighlighter.Model;

namespace CodeHighlighter;

public partial class SearchPanel
{
    #region Model
    public ISearchPanelModel Model
    {
        get => (ISearchPanelModel)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    public static readonly DependencyProperty ModelProperty = DependencyProperty.Register("Model", typeof(ISearchPanelModel), typeof(SearchPanel));
    #endregion

    public SearchPanel()
    {
        InitializeComponent();
    }

    private void ButtonGotoPrev(object sender, RoutedEventArgs e)
    {
        Model.Navigator.GotoPrev();
    }

    private void ButtonGotoNext(object sender, RoutedEventArgs e)
    {
        Model.Navigator.GotoNext();
    }
}
