using PCLExt.FileStorage;

namespace FactrIDE.Storage.Folders
{
    public class FactorioLogFile : BaseFile
    {
        public FactorioLogFile() : base(new FactorioUserDataActualFolder().CreateFile("factorio-current.log", CreationCollisionOption.OpenIfExists)) { }
    }
}