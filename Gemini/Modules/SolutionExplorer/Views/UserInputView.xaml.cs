using System.Windows;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Views
{
    /// <summary>
    /// Interaction logic for UserInputView.xaml
    /// </summary>
    public partial class UserInputView : Window
    {
        public UserInputView()
        {
            InitializeComponent();
        }

        private void OnUserInputViewLoaded(object sender, RoutedEventArgs e)
        {
            TextBoxName.Clear();
            TextBoxName.Focus();
        }
    }
}