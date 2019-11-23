using Newtonsoft.Json;

using System.Collections.Generic;

namespace FactrIDE.Gemini.Modules.FactorioDebugger.Json
{
    public class ValueJsonTable : Value
    {
        [JsonProperty("keys")]
        public List<Key> Keys { get; set; }

        [JsonProperty("values")]
        //[JsonConverter(typeof(ValueJsonConverter))]
        public List<Value> Values { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}