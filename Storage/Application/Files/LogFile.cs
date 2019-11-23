using System;

using PCLExt.FileStorage;

using FactrIDE.Storage.Folders;

namespace FactrIDE.Storage.Files
{
    public class LogFile : BaseFile
    {
        public LogFile() : base(new LogsFolder().CreateFile($"{DateTime.Now:yyyy-MM-dd_HH.mm.ss}.log", CreationCollisionOption.OpenIfExists)) { }
    }
}