using Newtonsoft.Json;

using System.Collections.Generic;

namespace FactrIDE.Gemini.Modules.FactorioDebugger.Json
{
    public class ValueLuaObject : Value
    {
        [JsonProperty("luaObjectType")]
        public string LuaObjectType { get; set; }

        [JsonProperty("luaObjectProperties")]
        public Dictionary<string, object> LuaObjectProperties { get; set; }
    }
}