using PCLExt.FileStorage;
using PCLExt.FileStorage.Folders;

using System;

using SystemInfoLibrary.OperatingSystem;

namespace FactrIDE.Storage.Folders
{
    /// <summary>
    /// The default Factorio User data folders
    /// </summary>
    public class FactorioUserDataDefaultFolder : BaseFolder
    {
        private static IFolder GetFolder() => OperatingSystemInfo.GetOperatingSystemInfo().OperatingSystemType switch
        {
            OperatingSystemType.Windows => new FolderFromPath(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Factorio")),
            OperatingSystemType.Linux => new FolderFromPath("~/.factorio"),
            OperatingSystemType.MacOSX => new FolderFromPath("~/Library/Application Support/factorio"),
            _ => new NonExistingFolder(""),
        };

        public FactorioUserDataDefaultFolder() : base(GetFolder()) { }
    }
}