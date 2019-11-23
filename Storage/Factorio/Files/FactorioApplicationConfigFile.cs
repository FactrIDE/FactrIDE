using PCLExt.FileStorage;
using PCLExt.FileStorage.Extensions;

using Salaros.Configuration;

using System.Globalization;

namespace FactrIDE.Storage.Folders
{
    public class FactorioApplicationConfigFile : BaseFile
    {
        public string WriteData
        {
            get => ConfigParser.GetValue("path", "write-data", string.Empty);
            //set => ConfigParser.SetValue("path", "write-data", value);
        }
        public string ReadData
        {
            get => ConfigParser.GetValue("path", "read-data", string.Empty);
            //set => ConfigParser.SetValue("path", "read-data", value);
        }

        private ConfigParser ConfigParser => new ConfigParser(
                this.ReadAllText(),
                new ConfigParserSettings
                {
                    MultiLineValues = MultiLineValues.Simple | MultiLineValues.AllowValuelessKeys | MultiLineValues.QuoteDelimitedValues,
                    Culture = new CultureInfo("en-US")
                });

        public FactorioApplicationConfigFile() : base(new FactorioApplicationConfigFolder().CreateFile("config.ini", CreationCollisionOption.OpenIfExists)) { }
    }
}