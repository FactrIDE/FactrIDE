using PCLExt.FileStorage;

namespace FactrIDE.Storage.Folders
{
    public class FactorioScriptOutputFolder : BaseFolder
    {
        public FactorioScriptOutputFolder() : base(new FactorioUserDataActualFolder().CreateFolder("script-output", CreationCollisionOption.OpenIfExists).CreateFolder("logs", CreationCollisionOption.OpenIfExists)) { }
    }
}