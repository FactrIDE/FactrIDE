using FactrIDE.Gemini.Modules.Factorio.ViewModels;

using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace FactrIDE.Gemini.Modules.Factorio.Views
{
    /// <summary>
    /// Interaction logic for FactorioSettingsView.xaml
    /// </summary>
    public partial class FactorioModPortalSettingsView : UserControl
    {
        public FactorioModPortalSettingsView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is FactorioModPortalSettingsViewModel viewModel)
                viewModel.Password = Convert.ToBase64String(ProtectedData.Protect(Encoding.UTF8.GetBytes(TextBoxPassword.Password), null, DataProtectionScope.CurrentUser));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is FactorioModPortalSettingsViewModel viewModel)
            {
                TextBoxPassword.Password = string.IsNullOrEmpty(viewModel.Password)
                    ? string.Empty
                    : Encoding.UTF8.GetString(ProtectedData.Unprotect(Convert.FromBase64String(viewModel.Password), null, DataProtectionScope.CurrentUser));
            }

            if (CheckBoxUseFactorioCredentials.IsChecked == true)
            {
                TextBoxUsername.IsEnabled = false;
                TextBoxPassword.IsEnabled = false;
            }
            else
            {
                TextBoxUsername.IsEnabled = true;
                TextBoxPassword.IsEnabled = true;
            }
        }

        private void OnCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (checkBox.IsChecked == true)
                {
                    TextBoxUsername.IsEnabled = false;
                    TextBoxPassword.IsEnabled = false;
                }
                else
                {
                    TextBoxUsername.IsEnabled = true;
                    TextBoxPassword.IsEnabled = true;
                }
            }
        }
    }
}