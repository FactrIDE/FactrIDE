using Newtonsoft.Json;

namespace FactrIDE.Gemini.Modules.FactorioDebugger.Json
{
    public class ValueExecutionStep : Value
    {
        [JsonProperty("file")]
        public string File { get; set; }

        [JsonProperty("line")]
        public int Line { get; set; }

        [JsonProperty("locals")]
        public ValueJsonTable Locals { get; set; }

        [JsonProperty("varArgs")]
        public ValueJsonTable VarArgs { get; set; }

        [JsonProperty("upValues")]
        public ValueJsonTable UpValues { get; set; }
    }
}