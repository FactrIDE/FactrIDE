using Caliburn.Micro;

using FactrIDE.Properties;

using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Modules.Output.Views;

using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FactrIDE.Gemini.Modules.FactorioStandardOutput.ViewModels
{
    [Export(typeof(IFactorioStandardOutput))]
    public class OutputViewModel : Tool, IFactorioStandardOutput
    {
        public TextWriter Writer { get; }
        public override PaneLocation PreferredLocation => PaneLocation.Bottom;
        private StringBuilder StringBuilder { get; }
        private Process Process { get; set; }

        private IOutputView _view;

        public OutputViewModel()
        {
            ViewLocator.AddNamespaceMapping(typeof(OutputViewModel).Namespace, typeof(OutputView).Namespace);
            DisplayName = Resources.FactorioStandardOutputTitle;
            StringBuilder = new StringBuilder();
        }

        public void SetOutput(Process process)
        {
            Clear();
            Process = process;
            Process.Exited += Process_Exited;
            Process.OutputDataReceived += Process_OutputDataReceived;
            Process.ErrorDataReceived += Process_ErrorDataReceived;
            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();
        }
        private void Process_Exited(object sender, System.EventArgs e)
        {
            Process.OutputDataReceived -= Process_OutputDataReceived;
            Process.ErrorDataReceived -= Process_ErrorDataReceived;
        }
        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e?.Data))
            {
                StringBuilder.AppendLine(e.Data);
                OnTextChanged();
            }
            else
            {
                if (!Process.HasExited)
                {
                    //var data = _process.StandardOutput.ReadToEnd();
                    ;
                }
            }
        }
        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e?.Data))
            {
                StringBuilder.AppendLine(e.Data);
                OnTextChanged();
            }
        }

        public void Clear()
        {
            if (_view != null)
                Execute.OnUIThread(() => _view.Clear());
            StringBuilder.Clear();
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
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            Process.OutputDataReceived -= Process_OutputDataReceived;
            Process.ErrorDataReceived -= Process_ErrorDataReceived;
            Process.CancelOutputRead();
            Process.CancelErrorRead();
        }

        public void AppendLine(string text) { }
        public void Append(string text) { }
    }
}