using ICSharpCode.AvalonEdit;

using System.Windows.Media;

namespace FactrIDE.Gemini.Modules.CodeEditor.Controls
{
    public class CodeEditor : TextEditor
    {
        public CodeEditor()
        {
            FontFamily = new FontFamily("Consolas");
            FontSize = 12;
            ShowLineNumbers = true;
            Options = new TextEditorOptions
            {
                ConvertTabsToSpaces = true
            };
        }
    }
}