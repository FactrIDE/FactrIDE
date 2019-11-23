using FactrIDE.Gemini.Modules.FactorioModPortal;

using Newtonsoft.Json;

using PCLExt.FileStorage;
using PCLExt.FileStorage.Extensions;

namespace FactrIDE.Storage.Folders
{
    public class FactorioPlayerDataFile : BaseFile
    {
        public PlayerData PlayerData
        {
            get => JsonConvert.DeserializeObject<PlayerData>(this.ReadAllText());
            //set => this.WriteAllText(JsonConvert.SerializeObject(value, Formatting.Indented));
        }

        public FactorioPlayerDataFile() : base(new FactorioUserDataActualFolder().CreateFile("player-data.json", CreationCollisionOption.OpenIfExists)) { }
    }
}