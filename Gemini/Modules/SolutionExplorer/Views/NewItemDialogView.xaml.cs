using System.Windows;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Views
{
    /// <summary>
    /// Interaction logic for NewItemDialogView.xaml
    /// </summary>
    public partial class NewItemDialogView : Window
    {
        public NewItemDialogView()
        {
            InitializeComponent();
        }

        private void OnNewItemLoaded(object sender, RoutedEventArgs e)
        {
            TextBoxName.Clear();
            TextBoxName.Focus();
        }
    }
}