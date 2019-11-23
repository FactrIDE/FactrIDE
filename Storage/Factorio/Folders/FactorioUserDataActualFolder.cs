using PCLExt.FileStorage;
using PCLExt.FileStorage.Folders;

namespace FactrIDE.Storage.Folders
{
    /// <summary>
    /// Factorio User data folder.
    /// </summary>
    public class FactorioUserDataActualFolder : BaseFolder
    {
        private static IFolder GetFolder()
        {
            if(new FactorioApplicationConfigPathFile() is FactorioApplicationConfigPathFile configPathFile && configPathFile.Exists && !configPathFile.UseSystemReadWriteDataDirectories)
            {
                if (new FactorioApplicationConfigFile() is FactorioApplicationConfigFile configFile && configFile.Exists)
                {
                    if (string.IsNullOrEmpty(configFile.WriteData))
                    {
                        return new FactorioUserDataDefaultFolder();
                    }
                    else
                    {
                        return new FolderFromPath(configFile.WriteData);
                    }
                }
            }

            return new FactorioUserDataDefaultFolder();
        }

        public FactorioUserDataActualFolder() : base(GetFolder()) { }
    }
}