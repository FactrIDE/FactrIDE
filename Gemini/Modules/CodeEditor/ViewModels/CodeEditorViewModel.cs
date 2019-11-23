using Caliburn.Micro;

using Gemini.Modules.CodeEditor;
using Gemini.Modules.CodeEditor.Views;

using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.CodeEditor.ViewModels
{
    [Export(typeof(CodeEditorViewModel))]
    [Export(typeof(global::Gemini.Modules.CodeEditor.ViewModels.CodeEditorViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CodeEditorViewModel : global::Gemini.Modules.CodeEditor.ViewModels.CodeEditorViewModel
    {
        private readonly LanguageDefinitionManager _languageDefinitionManager;

        private ICodeEditorView _view;
        private string _originalText;

        [ImportingConstructor]
        public CodeEditorViewModel(LanguageDefinitionManager languageDefinitionManager) : base(languageDefinitionManager)
        {
            ViewLocator.AddNamespaceMapping(typeof(CodeEditorViewModel).Namespace, typeof(CodeEditorView).Namespace);
            _languageDefinitionManager = languageDefinitionManager;
        }

        protected override Task DoNew()
        {
            _originalText = string.Empty;
            ApplyOriginalText();
            return Task.CompletedTask;
        }

        protected override Task DoLoad(string filePath)
        {
            _originalText = File.ReadAllText(filePath);
            ApplyOriginalText();
            return Task.CompletedTask;
        }

        protected override Task DoSave(string filePath)
        {
            var newText = _view.TextEditor.Text;
            File.WriteAllText(filePath, newText);
            _originalText = newText;
            return Task.CompletedTask;
        }

        private void ApplyOriginalText()
        {
            _view.TextEditor.Text = _originalText;

            _view.TextEditor.TextChanged += delegate
            {
                IsDirty = string.Compare(_originalText, _view.TextEditor.Text) != 0;
            };

            var fileExtension = Path.GetExtension(FileName).ToLower();

            ILanguageDefinition languageDefinition = _languageDefinitionManager.GetDefinitionByExtension(fileExtension);

            SetLanguage(languageDefinition);
        }

        private void SetLanguage(ILanguageDefinition languageDefinition)
        {
            _view.TextEditor.SyntaxHighlighting = languageDefinition?.SyntaxHighlighting;
        }

        protected override void OnViewLoaded(object view)
        {
            _view = (ICodeEditorView) view;
            _view.TextEditor.TextArea.TextEntering += TextArea_TextEntering;
            _view.TextEditor.TextArea.TextEntered += TextArea_TextEntered;
        }

        private CompletionWindow completionWindow;
        private void TextArea_TextEntering(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
        }

        private void TextArea_TextEntered(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text == ".")
            {
                // Open code completion after the user has pressed dot:
                completionWindow = new CompletionWindow(_view.TextEditor.TextArea);
                //var t = _view.TextEditor. ;
                IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
                data.Add(new MyCompletionData("Item1"));
                data.Add(new MyCompletionData("Item2"));
                data.Add(new MyCompletionData("Item3"));
                completionWindow.Show();
                completionWindow.Closed += delegate {
                    completionWindow = null;
                };
            }
        }

        public override bool Equals(object obj)
        {
            return obj is CodeEditorViewModel other
                && string.Equals(FilePath, other.FilePath, StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(FileName, other.FileName, StringComparison.InvariantCultureIgnoreCase);
        }
    }

    /// <summary>
    /// Implements AvalonEdit ICompletionData interface to provide the entries in the
    /// completion drop down.
    /// </summary>
    public class MyCompletionData : ICompletionData
    {
        public MyCompletionData(string text)
        {
            this.Text = text;
        }

        public System.Windows.Media.ImageSource Image => null;

        public string Text { get; }

        // Use this property if you want to show a fancy UIElement in the list.
        public object Content => this.Text;

        public object Description => "Description for " + this.Text;

        public double Priority => 1D;

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, this.Text);
        }
    }
}