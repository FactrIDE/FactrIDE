using PCLExt.FileStorage;

using Steamworks;

using SystemInfoLibrary.OperatingSystem;

namespace FactrIDE.Storage.Folders
{
    public class SteamUserDataCurrentFolder : BaseFolder
    {
        private static IFolder GetFolder()
        {
            var steamID = string.Empty;
            if (SteamAPI.Init())
            {
                LogManager.WriteLine("SteamAPI.Init() success.");
                steamID = SteamUser.GetSteamID().GetAccountID().ToString();
                SteamAPI.Shutdown();
                LogManager.WriteLine("SteamAPI.Shutdown().");
            }
            else
            {
                LogManager.WriteLine("SteamAPI.Init() failure.");

                switch (OperatingSystemInfo.GetOperatingSystemInfo().OperatingSystemType)
                {
                    case OperatingSystemType.Windows:
                        if (Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam\ActiveProcess", "ActiveUser", null) is int userID)
                        {
                            steamID = userID.ToString();
                        }
                        break;
                    default:
                        LogManager.WriteLine("Couldn't get Steam data.");
                        break;
                }
            }

            return string.IsNullOrEmpty(steamID)
                ? (IFolder) new NonExistingFolder("")
                : (IFolder) new SteamUserDataFolder().GetFolder(steamID);
        }

        public SteamUserDataCurrentFolder() : base(GetFolder()) { }
    }
}