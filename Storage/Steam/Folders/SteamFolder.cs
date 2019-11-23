using Microsoft.Win32;

using PCLExt.FileStorage;
using PCLExt.FileStorage.Folders;

using System.Diagnostics;

using SystemInfoLibrary.OperatingSystem;

namespace FactrIDE.Storage.Folders
{
    public class SteamFolder : BaseFolder
    {
        private static IFolder GetFolder()
        {
            var steamProcess = System.Array.Find(Process.GetProcessesByName("steam"), p => p.MainModule.FileVersionInfo.CompanyName.Equals("Valve Corporation") && p.MainModule.FileVersionInfo.ProductName.Equals("Steam Client Bootstrapper"));
            if (steamProcess != null)
                return new FolderFromPath(System.IO.Path.GetDirectoryName(steamProcess.MainModule.FileName));

            switch (OperatingSystemInfo.GetOperatingSystemInfo().OperatingSystemType)
            {
                case OperatingSystemType.Windows:
                    return Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam", "SteamPath", null) is string steamPath
                        ? (IFolder) new FolderFromPath(steamPath)
                        : (IFolder) new NonExistingFolder("");
                default:
                    LogManager.WriteLine("Couldn't get Steam folder.");
                    break;
            }

            return new NonExistingFolder("");
        }

        public SteamFolder() : base(GetFolder()) { }
    }
}