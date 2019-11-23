using Newtonsoft.Json;

namespace FactrIDE.Gemini.Modules.FactorioDebugger.Json
{
    public class DebugOutput
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("file")]
        public string File { get; set; }

        [JsonProperty("line")]
        public int Line { get; set; }

        [JsonProperty("locals")]
        public ValueJsonTable Locals { get; set; }

        [JsonProperty("upValues")]
        public ValueJsonTable UpValues { get; set; }
    }
}