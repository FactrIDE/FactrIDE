using Caliburn.Micro;

using Gemini.Modules.Settings;

using Microsoft.Win32;

using System.ComponentModel.Composition;

using SystemInfoLibrary.OperatingSystem;

namespace FactrIDE.Gemini.Modules.Factorio.ViewModels
{
    [Export(typeof(ISettingsEditor))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class FactorioSettingsViewModel : PropertyChangedBase, ISettingsEditor
    {
        public string SettingsPagePath => Properties.Resources.SettingsPageFactorio;
        public string SettingsPageName => Properties.Resources.SettingsPageFactorioGeneral;

        private string _factorioNonSteamFolderPath;
        public string FactorioNonSteamFolderPath
        {
            get => _factorioNonSteamFolderPath;
            set
            {
                if (value.Equals(_factorioNonSteamFolderPath)) return;
                _factorioNonSteamFolderPath = value;
                NotifyOfPropertyChange(() => FactorioNonSteamFolderPath);
            }
        }

        private bool _runFactorioViaSteam;
        public bool RunFactorioViaSteam
        {
            get => _runFactorioViaSteam;
            set
            {
                if (value.Equals(_runFactorioViaSteam)) return;
                _runFactorioViaSteam = value;
                NotifyOfPropertyChange(() => RunFactorioViaSteam);
            }
        }

        [ImportingConstructor]
        public FactorioSettingsViewModel()
        {
            RunFactorioViaSteam = Properties.Settings.Default.RunFactorioViaSteam;

            FactorioNonSteamFolderPath = string.IsNullOrEmpty(Properties.Settings.Default.FactorioNonSteamFolderPath)
                ? OperatingSystemInfo.GetOperatingSystemInfo().OperatingSystemType switch
                {
                    OperatingSystemType.Windows => Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Factorio_is1", "InstallLocation", null) is string path
                    ? path
                    : string.Empty,
                    _ => null,
                }
            : Properties.Settings.Default.FactorioNonSteamFolderPath;
        }

        public void ApplyChanges()
        {
            Properties.Settings.Default.FactorioNonSteamFolderPath = FactorioNonSteamFolderPath;
            Properties.Settings.Default.RunFactorioViaSteam = RunFactorioViaSteam;
            Properties.Settings.Default.Save();
        }
    }
}