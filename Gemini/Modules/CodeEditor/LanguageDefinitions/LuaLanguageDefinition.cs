using Gemini.Modules.CodeEditor;

using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace FactrIDE.Gemini.Modules.Lua
{
    /*
    [Export(typeof(ILanguageDefinition))]
    internal class SharpLuaLanguageDefinition : FromResourceLanguageDefinition
    {
        public override string Name { get; } = "SharpLua";
        public override IEnumerable<string> FileExtensions { get; set; } = new List<string>() { ".lua" };
        public override string CustomSyntaxHighlightingFileName { get; set; } = "Lua-Mode.xshd";
    }
    */
    [Export(typeof(ILanguageDefinition))]
    internal class LuaLanguageDefinition : FromResourceLanguageDefinition
    {
        public override string Name { get; } = "Lua";
        public override IEnumerable<string> FileExtensions { get; set; } = new List<string>() { ".lua" };
        public override string CustomSyntaxHighlightingFileName { get; set; } = "Lua.xshd";
    }
}