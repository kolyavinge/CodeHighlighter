using System.Windows;
using CodeHighlighter.Model;

namespace CodeHighlighter;

public partial class SearchPanelView : ISearchPanelView
{
    #region Model
    public ISearchPanel Model
    {
        get => (ISearchPanel)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    public static readonly DependencyProperty ModelProperty = DependencyProperty.Register("Model", typeof(ISearchPanel), typeof(SearchPanelView), new PropertyMetadata(OnModelChangedCallback));

    private static void OnModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var panel = (SearchPanelView)d;
        var model = (ISearchPanel)e.NewValue;
        model.AttachSearchPanel(panel);
    }
    #endregion

    public SearchPanelView()
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
