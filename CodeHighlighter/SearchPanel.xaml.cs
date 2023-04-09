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
}
