using PCLExt.FileStorage;

namespace FactrIDE.Storage.Folders
{
    public class FactorioUserDataSteamFolder : BaseFolder
    {
        public FactorioUserDataSteamFolder() : base(new SteamUserDataCurrentFolder().CreateFolder("427520", CreationCollisionOption.OpenIfExists).CreateFolder("remote", CreationCollisionOption.OpenIfExists)) { }
    }
}