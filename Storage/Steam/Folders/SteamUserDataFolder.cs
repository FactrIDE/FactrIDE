using PCLExt.FileStorage;

namespace FactrIDE.Storage.Folders
{
    public class SteamUserDataFolder : BaseFolder
    {
        public SteamUserDataFolder() : base(new SteamFolder().CreateFolder("userdata", CreationCollisionOption.OpenIfExists)) { }
    }
}