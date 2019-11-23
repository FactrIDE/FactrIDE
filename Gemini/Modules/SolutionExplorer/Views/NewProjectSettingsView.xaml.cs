using Ookii.Dialogs.Wpf;

using System.Windows;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Views
{
    /// <summary>
    ///     Interaction logic for NewProjectSettingsView.xaml
    /// </summary>
    public partial class NewProjectSettingsView : Window
    {
        public NewProjectSettingsView()
        {
            InitializeComponent();
        }

        private void OnBrowseButtonClicked(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog { ShowNewFolderButton = true };

            if (dialog.ShowDialog() == true)
                TextBoxLocation.Text = dialog.SelectedPath;

            e.Handled = true;
        }

        private void OnNewProjectSettingsViewLoaded(object sender, RoutedEventArgs e)
        {
            TextBoxProjectName.Clear();
            TextBoxProjectName.Focus();

            TextBoxLocation.Clear();

            TextBoxSolutionName.Clear();

            CheckBoxProjectInRoot.IsChecked = false;
        }
    }
}