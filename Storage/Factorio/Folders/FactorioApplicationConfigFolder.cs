using FactrIDE.Properties;

using PCLExt.FileStorage;
using PCLExt.FileStorage.Folders;

namespace FactrIDE.Storage.Folders
{
    /// <summary>
    /// Factorio folder that is the /config folder
    /// </summary>
    public class FactorioApplicationConfigFolder : BaseFolder
    {
        private static IFolder GetFolder() => Settings.Default.RunFactorioViaSteam
            ? (IFolder) new FactorioUserDataSteamFolder()
            : (IFolder) new FolderFromPath(Settings.Default.FactorioNonSteamFolderPath);

        public FactorioApplicationConfigFolder() : base(GetFolder()) { }
    }
}