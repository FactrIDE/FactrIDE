using FactrIDE.Properties;

using Gemini.Modules.CodeEditor;

using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace FactrIDE.Gemini.Modules.Lua
{
    public abstract class FromResourceLanguageDefinition : ILanguageDefinition
    {
        private XshdSyntaxDefinition _syntaxDefinition;

        public abstract string Name { get; }
        public abstract IEnumerable<string> FileExtensions { get; set; }
        public abstract string CustomSyntaxHighlightingFileName { get; set; }

        private IHighlightingDefinition _highlightingDefinition;
        public IHighlightingDefinition SyntaxHighlighting => _highlightingDefinition ?? (_highlightingDefinition = LoadHighlightingDefinition());

        private IHighlightingDefinition LoadHighlightingDefinition()
        {
            HighlightingManager highlightingManager = HighlightingManager.Instance;

            if (!string.IsNullOrEmpty(CustomSyntaxHighlightingFileName))
            {
                using var reader = new XmlTextReader(OpenStream(CustomSyntaxHighlightingFileName));
                _syntaxDefinition = HighlightingLoader.LoadXshd(reader);
            }

            if (_syntaxDefinition != null)
            {
                var highlightingDefinition = HighlightingLoader.Load(_syntaxDefinition, highlightingManager);
                highlightingManager.RegisterHighlighting(_syntaxDefinition.Name, _syntaxDefinition.Extensions.ToArray(), highlightingDefinition);
            }

            return highlightingManager.GetDefinition(Name);
        }

        private const string Prefix = "FactrIDE.Resources.";
        public static Stream OpenStream(string name)
        {
            var s = typeof(Resources).Assembly.GetManifestResourceStream(Prefix + name);
            if (s == null) throw new FileNotFoundException($"The resource file '{name}' was not found.");
            return s;
        }
    }
}