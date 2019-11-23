using PCLExt.FileStorage;
using PCLExt.FileStorage.Extensions;

using Salaros.Configuration;

using System.Globalization;

namespace FactrIDE.Storage.Folders
{
    public class FactorioApplicationConfigPathFile : BaseFile
    {
        public bool UseSystemReadWriteDataDirectories
        {
            get => ConfigParser.GetValue("root", "use-system-read-write-data-directories", false);
            //set => ConfigParser.SetValue("root", "use-system-read-write-data-directories", value);
        }

        private ConfigParser ConfigParser => new ConfigParser(
                "[root]\r\nnope=0\r\n" + this.ReadAllText(),
                new ConfigParserSettings
                {
                    MultiLineValues = MultiLineValues.Simple | MultiLineValues.AllowValuelessKeys | MultiLineValues.QuoteDelimitedValues,
                    Culture = new CultureInfo("en-US")
                });

        public FactorioApplicationConfigPathFile() : base(new FactorioApplicationFolder().CreateFile("config-path.cfg", CreationCollisionOption.OpenIfExists)) { }
    }
}