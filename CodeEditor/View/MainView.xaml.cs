using System.Windows;
using CodeEditor.ViewModel;

namespace CodeEditor.View
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
