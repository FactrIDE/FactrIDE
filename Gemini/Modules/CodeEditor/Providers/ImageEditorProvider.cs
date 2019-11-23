using FactrIDE.Gemini.Modules.CodeEditor.ViewModels;

using Gemini.Framework;
using Gemini.Framework.Services;

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.Startup
{
    [Export(typeof(IEditorProvider))]
    public class ImageEditorProvider : IEditorProvider
    {
        private readonly List<string> _extensions = new List<string>
        {
            ".png",
            ".jpg", ".jpeg",
            ".bmp"
        };

        public IEnumerable<EditorFileType> FileTypes
        {
            get
            {
                yield return new EditorFileType("PNG File", ".png");
                yield return new EditorFileType("JPEG File", ".jpg; *.jpeg");
                yield return new EditorFileType("BMP File", ".bmp");
            }
        }

        public bool Handles(string path)
        {
            var extension = Path.GetExtension(path);
            return _extensions.Contains(extension);
        }

        public IDocument Create() => new ImageViewModel();

        public Task New(IDocument document, string name) => ((ImageViewModel) document).New(name);

        public Task Open(IDocument document, string path) => ((ImageViewModel) document).Load(path);
    }
}