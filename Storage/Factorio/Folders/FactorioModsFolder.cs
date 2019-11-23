using PCLExt.FileStorage;

namespace FactrIDE.Storage.Folders
{
    public class FactorioModsFolder : BaseFolder
    {
        public FactorioModsFolder() : base(new FactorioUserDataActualFolder().CreateFolder("mods", CreationCollisionOption.OpenIfExists)) { }
    }
}