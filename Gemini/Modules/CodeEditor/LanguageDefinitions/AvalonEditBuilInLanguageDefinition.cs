using Gemini.Modules.CodeEditor;

using ICSharpCode.AvalonEdit.Highlighting;

using System.Collections.Generic;

namespace FactrIDE.Gemini.Modules.Lua
{
    public abstract class AvalonEditBuilInLanguageDefinition : ILanguageDefinition
    {
        public abstract string Name { get; }
        public abstract IEnumerable<string> FileExtensions { get; set; }
        public string CustomSyntaxHighlightingFileName { get; set; }

        private IHighlightingDefinition _highlightingDefinition;
        public IHighlightingDefinition SyntaxHighlighting => _highlightingDefinition ?? (_highlightingDefinition = HighlightingManager.Instance.GetDefinition(Name));
    }
}