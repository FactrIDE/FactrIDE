using Newtonsoft.Json;

using System.Collections.Generic;

namespace FactrIDE.Gemini.Modules.FactorioDebugger.Json
{
    public class ValueReference : Value
    {
        [JsonProperty("referenceLocation")]
        public string ReferenceLocation { get; set; }

        [JsonProperty("referencePath")]
        public List<int> ReferencePath { get; set; }

        [JsonProperty("referenceUsesKeys")]
        public List<bool> ReferenceUsesKeys { get; set; }

        [JsonProperty("referenceDepth")]
        public int ReferenceDepth { get; set; }

        /*
        void GetReferedValue(ValueReference reference, ValueJsonTable[] executionStep)
        {
            var cur = executionStep[reference.referenceLocation];
            for (var i = 0; i < reference.referenceDepth; ++i)
            {
                var location = reference.referenceUsesKeys[i] ? cur.keys : cur.values;
                cur = location[reference.referencePath[i]];
            }
            return cur;
        }
        */
    }
}