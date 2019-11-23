using FactrIDE.Gemini.Modules.FactorioStandardOutput;
using FactrIDE.Gemini.Modules.SolutionExplorer;
using FactrIDE.Properties;

using Gemini.Framework.Commands;
using Gemini.Framework.Services;

using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FactrIDE.Gemini.Modules.Factorio.Commands
{
    [CommandHandler]
    public class RunFactorioCommandHandler : CommandHandlerBase<RunFactorioCommandDefinition>
    {
        private readonly IShell _shell;
        private readonly ISolutionExplorer _solutionExplorer;
        private readonly IFactorioStandardOutput _output;

        [ImportingConstructor]
        public RunFactorioCommandHandler(IShell shell, ISolutionExplorer solutionExplorer, IFactorioStandardOutput output)
        {
            _shell = shell;
            _solutionExplorer = solutionExplorer;
            _output = output;
        }

        public override void Update(Command command)
        {
            base.Update(command);

            command.Enabled = _solutionExplorer.SelectedProject != null && Process.GetProcessesByName("factorio").Length == 0;
            command.Text = command.Enabled
                ? $"{_solutionExplorer.SelectedProject.Name}"
                : Resources.RunFactorioCommandToolTip;
            command.ToolTip = command.Enabled
                ? $"{Resources.RunFactorioCommandToolTip} with {_solutionExplorer.SelectedProject.Name}"
                : Resources.RunFactorioCommandToolTip;
        }

        public override Task Run(Command command)
        {
            if(_solutionExplorer.SelectedProject != null)
                _solutionExplorer.Run();

            if (Settings.Default.RunFactorioViaSteam)
            {
                var process = Process.Start(new ProcessStartInfo("steam://rungameid/427520")
                {
                    UseShellExecute = true,
                    Verb = "open"
                });
                process.EnableRaisingEvents = true;
                process.Exited += (s, e) => CommandManager.InvalidateRequerySuggested();
            }
            else
            {
                var processStartInfo = new ProcessStartInfo("CMD.exe", $@"/c ""{Settings.Default.FactorioNonSteamFolderPath}""")
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                };
                var process = Process.Start(processStartInfo);
                process.EnableRaisingEvents = true;
                process.Exited += (s, e) => CommandManager.InvalidateRequerySuggested();

                _output.SetOutput(process);
                _shell.ShowTool(_output);
            }

            return Task.CompletedTask;
        }
    }
}