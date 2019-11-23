using Caliburn.Micro;

using FactrIDE.Properties;

using Gemini.Modules.Settings;

using System.ComponentModel.Composition;

namespace FactrIDE.Gemini.Modules.Startup.ViewModels
{
    [Export(typeof(ISettingsEditor))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ReportingSettingsViewModel : PropertyChangedBase, ISettingsEditor
    {
        public string SettingsPagePath => GeminiResources.SettingsPathEnvironment;
        public string SettingsPageName => Resources.SettingsPageReporting;

        private bool _reportToWeb;
        public bool ReportToWeb
        {
            get => _reportToWeb;
            set
            {
                if (value.Equals(_reportToWeb)) return;
                _reportToWeb = value;
                NotifyOfPropertyChange(() => ReportToWeb);
            }
        }

        [ImportingConstructor]
        public ReportingSettingsViewModel()
        {
            ReportToWeb = Settings.Default.ReportToWeb;
        }

        public void ApplyChanges()
        {
            Settings.Default.ReportToWeb = ReportToWeb;
            Settings.Default.Save();
        }
    }
}