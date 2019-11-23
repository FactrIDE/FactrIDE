using FactrIDE.Properties;

using Microsoft.Win32;

using PCLExt.FileStorage;
using PCLExt.FileStorage.Folders;

using Steamworks;

using SystemInfoLibrary.OperatingSystem;

namespace FactrIDE.Storage.Folders
{
    /// <summary>
    /// Factorio folder that contains the /bin folder
    /// </summary>
    public class FactorioApplicationFolder : BaseFolder
    {
        private static IFolder GetFolder()
        {
            if (Settings.Default.RunFactorioViaSteam)
            {
                if (SteamAPI.Init())
                {
                    LogManager.WriteLine("SteamAPI.Init() success.");
                    SteamAppList.GetAppInstallDir(new AppId_t(427520), out var factorioInstallPath, 260);
                    SteamAPI.Shutdown();
                    LogManager.WriteLine("SteamAPI.Shutdown().");
                    return new FolderFromPath(factorioInstallPath);
                }
                else
                {
                    LogManager.WriteLine("SteamAPI.Init() failure.");

                    switch (OperatingSystemInfo.GetOperatingSystemInfo().OperatingSystemType)
                    {
                        case OperatingSystemType.Windows:
                            if (Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 427520", "InstallLocation", null) is string path)
                            {
                                return new FolderFromPath(path);
                            }
                            break;
                        default:
                            LogManager.WriteLine("Couldn't get Steam data.");
                            break;
                    }
                }
            }
            else
            {
                switch (OperatingSystemInfo.GetOperatingSystemInfo().OperatingSystemType)
                {
                    case OperatingSystemType.Windows:
                        if (Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Factorio_is1", "InstallLocation", null) is string path)
                        {
                            return new FolderFromPath(path);
                        }
                        break;
                }

                return new FolderFromPath(Settings.Default.FactorioNonSteamFolderPath);
            }

            return new NonExistingFolder("");
        }

        public FactorioApplicationFolder() : base(GetFolder()) { }
    }
}