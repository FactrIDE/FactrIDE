using Gemini.Modules.CodeEditor;

using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace FactrIDE.Gemini.Modules.Lua
{
    /*
    [Export(typeof(ILanguageDefinition))]
    internal class MarkDownLanguageDefinition : AvalonEditBuilInLanguageDefinition
    {
        public override string Name { get; } = "MarkDown";
        public override IEnumerable<string> FileExtensions { get; set; } = new List<string>() { ".md" };
    }
    */

    [Export(typeof(ILanguageDefinition))]
    internal class MarkDownWithFontSizeLanguageDefinition : AvalonEditBuilInLanguageDefinition
    {
        public override string Name { get; } = "MarkDownWithFontSize";
        public override IEnumerable<string> FileExtensions { get; set; } = new List<string>() { ".md" };
    }

    [Export(typeof(ILanguageDefinition))]
    internal class JsonLanguageDefinition : AvalonEditBuilInLanguageDefinition
    {
        public override string Name { get; } = "Json";
        public override IEnumerable<string> FileExtensions { get; set; } = new List<string>() { ".json" };
    }
}