using System;

using PCLExt.FileStorage;

using FactrIDE.Storage.Folders;

namespace FactrIDE.Storage.Files
{
    public class CrashLogFile : BaseFile
    {
        public CrashLogFile() : base(new CrashLogsFolder().CreateFile($"{DateTime.UtcNow:yyyy-MM-dd_HH.mm.ss}.log", CreationCollisionOption.OpenIfExists)) { }
    }
}