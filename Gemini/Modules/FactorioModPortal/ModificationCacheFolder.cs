using PCLExt.FileStorage;
using PCLExt.FileStorage.Folders;

namespace FactrIDE.Gemini.Modules.FactorioModPortal
{
    public class ModificationCacheFolder : BaseFolder
    {
        public ModificationCacheFolder() : base(new CacheRootFolder().CreateFolder("FactrIDE", CreationCollisionOption.OpenIfExists)) { }
    }
}