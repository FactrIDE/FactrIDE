using Caliburn.Micro;

using FactrIDE.Properties;
using FactrIDE.Storage.Folders;

using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Modules.Output.Views;

using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;

namespace FactrIDE.Gemini.Modules.FactorioLogOutput.ViewModels
{
    [Export(typeof(IFactorioLogOutput))]
    public class OutputViewModel : Tool, IFactorioLogOutput
    {
        public TextWriter Writer { get; }
        public override PaneLocation PreferredLocation => PaneLocation.Bottom;

        private LogWatcher LogWatcher { get; }
        private StringBuilder StringBuilder { get; }

        private IOutputView _view;

        public OutputViewModel()
        {
            ViewLocator.AddNamespaceMapping(typeof(OutputViewModel).Namespace, typeof(OutputView).Namespace);
            DisplayName = Resources.FactorioLogOutputTitle;
            StringBuilder = new StringBuilder();
            Writer = new OutputWriter(this);

            LogWatcher = new LogWatcher(this, new FactorioLogFile().Path);
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

            LogWatcher.EnableRaisingEvents = true;
        }
        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LogWatcher.EnableRaisingEvents = true;
        }
        private void OnUnloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LogWatcher.EnableRaisingEvents = false;
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            LogWatcher.Dispose();
        }
    }
}