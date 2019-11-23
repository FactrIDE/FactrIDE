using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;

namespace FactrIDE.Gemini.Modules.FactorioDebugger.Json
{
    public class ValueJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Value);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //writer.WriteValue(((value as Value) ?? new Value(0, 0, 0, 0)).ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            switch (jObject["type"].Value<string>())
            {
                case "JsonTable":
                    return new ValueJsonTable()
                    {
                        Type = jObject["type"].Value<string>(),
                        Keys = JsonConvert.DeserializeObject<List<Key>>(jObject["keys"].ToString()),
                        Values = JsonConvert.DeserializeObject <List <Value>>(jObject["values"].ToString()),
                        Count = jObject["count"].Value<int>(),
                    };
                case "JsonValue":
                    switch (jObject["luaType"].Value<string>())
                    {
                        case "nil":
                            return new ValueNil()
                            {
                                Type = jObject["type"].Value<string>(),
                                LuaType = jObject["luaType"].Value<string>()
                            };
                        case "number":
                            return new ValueNumber()
                            {
                                Type = jObject["type"].Value<string>(),
                                LuaType = jObject["luaType"].Value<string>(),
                                Value = jObject["value"].Value<int>()
                            };
                        case "string":
                            return new ValueString()
                            {
                                Type = jObject["type"].Value<string>(),
                                LuaType = jObject["luaType"].Value<string>(),
                                Value = jObject["value"].Value<string>()
                            };
                        case "boolean":
                            return new ValueBoolean()
                            {
                                Type = jObject["type"].Value<string>(),
                                LuaType = jObject["luaType"].Value<string>(),
                                Value = jObject["value"].Value<bool>()
                            };
                        case "function":
                            return new ValueFunction()
                            {
                                Type = jObject["type"].Value<string>(),
                                LuaType = jObject["luaType"].Value<string>()
                            };
                        case "thread":
                            return new ValueThread()
                            {
                                Type = jObject["type"].Value<string>(),
                                LuaType = jObject["luaType"].Value<string>()
                            };
                        case "userdata":
                            return new ValueUserData()
                            {
                                Type = jObject["type"].Value<string>(),
                                LuaType = jObject["luaType"].Value<string>()
                            };
                    }
                    break;
                case "Reference":
                    return new ValueReference()
                    {
                        Type = jObject["type"].Value<string>(),
                        ReferenceLocation = jObject["referenceLocation"].Value<string>(),
                        ReferencePath = jObject["referencePath"].Value<JArray>().Values<int>().ToList(),
                        ReferenceUsesKeys = jObject["referenceUsesKeys"].Value<JArray>().Values<bool>().ToList(),
                        ReferenceDepth = jObject["referenceDepth"].Value<int>()
                    };
                case "ExecutionStep":
                    return new ValueExecutionStep()
                    {
                        Type = jObject["type"].Value<string>(),
                        File = jObject["type"].Value<string>(),
                        Line = jObject["type"].Value<int>(),
                        Locals = JsonConvert.DeserializeObject<ValueJsonTable>(jObject["locals"].ToString()),
                        UpValues = JsonConvert.DeserializeObject<ValueJsonTable>(jObject["upValues"].ToString()),
                        VarArgs = JsonConvert.DeserializeObject<ValueJsonTable>(jObject["varArgs"].ToString()),
                    };
                case "LuaObject":
                    return new ValueLuaObject()
                    {
                        Type = jObject["type"].Value<string>(),
                        LuaObjectType = jObject["luaObjectType"].Value<string>(),
                    };
            }
            return null;
        }
    }

    public class Key
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("luaType")]
        public string LuaType { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    [JsonConverter(typeof(ValueJsonConverter))]
    public class Value
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class ValueValue : Value
    {
        [JsonProperty("luaType")]
        public string LuaType { get; set; }
    }
    public class ValueNil : ValueValue { }
    public class ValueNumber : ValueValue
    {
        [JsonProperty("value")]
        public int Value { get; set; }
    }
    public class ValueBoolean : ValueValue
    {
        [JsonProperty("value")]
        public bool Value { get; set; }
    }
    public class ValueString : ValueValue
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
    public class ValueFunction : ValueValue { }
    public class ValueThread : ValueValue { }
    public class ValueUserData : ValueValue { }
}