using PCLExt.FileStorage;

namespace FactrIDE.Storage.Folders
{
    public class CrashLogsFolder : BaseFolder
    {
        public CrashLogsFolder() : base(new LogsFolder().CreateFolder("CrashLogs", CreationCollisionOption.OpenIfExists)) { }
    }
}