using Newtonsoft.Json;

using System;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Converters
{
    public class VersionJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Version);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) =>
            writer.WriteValue(((value as Version) ?? new Version(0, 0, 0, 0)).ToString());

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
            Version.Parse(((string)reader.Value) ?? "0.0.0.0");
    }
}