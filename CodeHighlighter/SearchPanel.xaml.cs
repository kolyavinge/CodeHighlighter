using System.Windows;
using CodeHighlighter.Model;

namespace CodeHighlighter;

public partial class SearchPanel : ISearchPanel
{
    #region Model
    public ISearchPanelModel Model
    {
        get => (ISearchPanelModel)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    public static readonly DependencyProperty ModelProperty = DependencyProperty.Register("Model", typeof(ISearchPanelModel), typeof(SearchPanel), new PropertyMetadata(OnModelChangedCallback));

    private static void OnModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var panel = (SearchPanel)d;
        var model = (ISearchPanelModel)e.NewValue;
        model.AttachSearchPanel(panel);
    }
    #endregion

    public SearchPanel()
    {
        InitializeComponent();
    }

    public bool FocusPattern()
    {
        return _pattern.Focus();
    }

    public void SelectAllPattern()
    {
        _pattern.SelectAll();
    }

    private void ButtonGotoPrev(object sender, RoutedEventArgs e)
    {
        Model.Navigator.GotoPrev();
    }

    private void ButtonGotoNext(object sender, RoutedEventArgs e)
    {
        Model.Navigator.GotoNext();
    }

    private void ButtonClear(object sender, RoutedEventArgs e)
    {
        Model.Pattern = "";
    }
}
