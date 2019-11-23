using Caliburn.Micro;

using Gemini.Modules.Settings;

using System.ComponentModel.Composition;

namespace FactrIDE.Gemini.Modules.DiscordRichPresence.ViewModels
{
    [Export(typeof(ISettingsEditor))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DiscordSettingsViewModel : PropertyChangedBase, ISettingsEditor
    {
        /*
        public string SettingsPagePath => Properties.Resources.SettingsPageDiscord;
        public string SettingsPageName => Properties.Resources.SettingsPageDiscordGeneral;
        */
        public string SettingsPagePath => GeminiResources.SettingsPathEnvironment;
        public string SettingsPageName => Properties.Resources.SettingsPageDiscord;

        private bool _enableDiscordRichPresence;
        public bool EnableDiscordRichPresence
        {
            get => _enableDiscordRichPresence;
            set
            {
                if (value.Equals(_enableDiscordRichPresence)) return;
                _enableDiscordRichPresence = value;
                NotifyOfPropertyChange(() => EnableDiscordRichPresence);
            }
        }

        [ImportingConstructor]
        public DiscordSettingsViewModel()
        {
            EnableDiscordRichPresence = Properties.Settings.Default.EnableDiscordRichPresence;
        }

        public void ApplyChanges()
        {
            Properties.Settings.Default.EnableDiscordRichPresence = EnableDiscordRichPresence;
            Properties.Settings.Default.Save();
        }
    }
}