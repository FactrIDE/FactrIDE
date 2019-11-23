using Caliburn.Micro;

using Gemini.Modules.Settings;

using System.ComponentModel.Composition;

namespace FactrIDE.Gemini.Modules.Factorio.ViewModels
{
    [Export(typeof(ISettingsEditor))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class FactorioModPortalSettingsViewModel : PropertyChangedBase, ISettingsEditor
    {
        public string SettingsPagePath => Properties.Resources.SettingsPageFactorio;
        public string SettingsPageName => Properties.Resources.SettingsPageFactorioModPortal;

        private bool _useFactorioCredentials;
        public bool UseFactorioCredentials
        {
            get => _useFactorioCredentials;
            set
            {
                if (value.Equals(_useFactorioCredentials)) return;
                _useFactorioCredentials = value;
                NotifyOfPropertyChange(() => _useFactorioCredentials);
            }
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value.Equals(_username)) return;
                _username = value;
                NotifyOfPropertyChange(() => _username);
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (value.Equals(_password)) return;
                _password = value;
                NotifyOfPropertyChange(() => _password);
            }
        }

        [ImportingConstructor]
        public FactorioModPortalSettingsViewModel()
        {
            UseFactorioCredentials = Properties.Settings.Default.UseFactorioCredentials;
            Username = Properties.Settings.Default.Username;
            Password = Properties.Settings.Default.Password;
        }

        public void ApplyChanges()
        {
            Properties.Settings.Default.UseFactorioCredentials = UseFactorioCredentials;
            Properties.Settings.Default.Username = Username;
            Properties.Settings.Default.Password = Password;
            Properties.Settings.Default.Save();
        }
    }
}