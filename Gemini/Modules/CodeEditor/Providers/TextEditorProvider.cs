using Caliburn.Micro;

using FactrIDE.Gemini.Modules.CodeEditor.ViewModels;

using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Modules.CodeEditor;

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.Startup
{
    [Export(typeof(IEditorProvider))]
    public class TextEditorProvider : IEditorProvider
    {
        private readonly List<string> _extensions = new List<string>
        {
            ".txt",
            ".cfg",
            ""
        };

        public IEnumerable<EditorFileType> FileTypes
        {
            get
            {
                yield return new EditorFileType("Text File", ".txt");
                yield return new EditorFileType("Config/Text File", ".cfg");
                yield return new EditorFileType("Unknown File", "");
            }
        }

        public bool Handles(string path)
        {
            var extension = Path.GetExtension(path);
            return _extensions.Contains(extension);
        }

        public IDocument Create() => new CodeEditorViewModel(IoC.Get<LanguageDefinitionManager>());

        public Task New(IDocument document, string name) => ((CodeEditorViewModel) document).New(name);

        public Task Open(IDocument document, string path) => ((CodeEditorViewModel) document).Load(path);
    }
}