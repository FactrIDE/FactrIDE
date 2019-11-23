using Caliburn.Micro;

using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Modules.Output.Views;

using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;

namespace FactrIDE.Gemini.Modules.FactorioModificationOutput.ViewModels
{
    [Export(typeof(IFactorioModificationOutput))]
    public class OutputViewModel : Tool, IFactorioModificationOutput
    {
        public TextWriter Writer { get; }
        public override PaneLocation PreferredLocation => PaneLocation.Bottom;

        private LogWatcher LogWatcher { get; set; }
        private StringBuilder StringBuilder { get; }
        private string Path { get; set; }

        private IOutputView _view;

        public OutputViewModel()
        {
            ViewLocator.AddNamespaceMapping(typeof(OutputViewModel).Namespace, typeof(OutputView).Namespace);
            StringBuilder = new StringBuilder();
            Writer = new OutputWriter(this);
        }

        public void Clear()
        {
            if (_view != null)
                Execute.OnUIThread(() => _view.Clear());
            StringBuilder.Clear();
        }

        public void AppendLine(string text)
        {
            Append(text + Environment.NewLine);
        }

        public void Append(string text)
        {
            StringBuilder.Append(text);
            OnTextChanged();
        }

        private void OnTextChanged()
        {
            if (_view != null)
                Execute.OnUIThread(() => _view.SetText(StringBuilder.ToString()));
        }

        protected override void OnViewLoaded(object view)
        {
            _view = (IOutputView) view;
            _view.SetText(StringBuilder.ToString());
            _view.ScrollToEnd();

            if (view is OutputView outputView)
            {
                outputView.Loaded += OnLoaded;
                outputView.Unloaded += OnUnloaded;
            }
        }
        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if(LogWatcher != null)
                LogWatcher.EnableRaisingEvents = true;
        }
        private void OnUnloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (LogWatcher != null)
                LogWatcher.EnableRaisingEvents = false;
        }

        public override void SaveState(BinaryWriter writer)
        {
            var pathExist = !string.IsNullOrEmpty(Path);
            writer.Write(pathExist);
            if (pathExist)
                writer.Write(Path);

            base.SaveState(writer);
        }
        public override void LoadState(BinaryReader reader)
        {
            var pathExist = reader.ReadBoolean();
            if (pathExist)
            {
                if (File.Exists(Path))
                {
                    Path = reader.ReadString();
                    SetFilePath(Path);
                }
                else
                    TryClose();
            }
            else
                TryClose();

            base.LoadState(reader);
        }
        public void SetFilePath(string path)
        {
            Path = path;

            DisplayName = System.IO.Path.GetFileName(path);
            LogWatcher?.Dispose();
            LogWatcher = new LogWatcher(this, path);
        }
    }
}