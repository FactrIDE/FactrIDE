using Ookii.Dialogs.Wpf;

using System.Windows;
using System.Windows.Controls;

namespace FactrIDE.Gemini.Modules.Factorio.Views
{
    /// <summary>
    /// Interaction logic for FactorioSettingsView.xaml
    /// </summary>
    public partial class FactorioSettingsView : UserControl
    {
        public FactorioSettingsView()
        {
            InitializeComponent();
        }

        private void OnBrowseButtonClicked(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaOpenFileDialog()
            {
                Multiselect = false,
                CheckFileExists = true,
                Filter = "Factorio Executable (factorio.exe)| factorio.exe",
            };

            if (dialog.ShowDialog() == true)
                TextBoxLocation.Text = dialog.FileName;

            e.Handled = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (CheckBoxRunFactorioViaSteam.IsChecked == true)
            {
                TextBoxLocation.IsEnabled = false;
                ButtonBrowse.IsEnabled = false;
            }
            else
            {
                TextBoxLocation.IsEnabled = true;
                ButtonBrowse.IsEnabled = true;
            }
        }

        private void OnCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if(sender is CheckBox checkBox)
            {
                if (checkBox.IsChecked == true)
                {
                    TextBoxLocation.IsEnabled = false;
                    ButtonBrowse.IsEnabled = false;
                }
                else
                {
                    TextBoxLocation.IsEnabled = true;
                    ButtonBrowse.IsEnabled = true;
                }
            }
        }
    }
}