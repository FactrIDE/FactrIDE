using PCLExt.FileStorage;
using PCLExt.FileStorage.Folders;

namespace FactrIDE.Storage.Folders
{
    public class LogsFolder : BaseFolder
    {
        public LogsFolder() : base(new ApplicationRootFolder().CreateFolder("Logs", CreationCollisionOption.OpenIfExists)) { }
    }
}