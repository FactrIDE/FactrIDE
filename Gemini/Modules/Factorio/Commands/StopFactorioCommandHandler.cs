using FactrIDE.Gemini.Modules.SolutionExplorer;

using Gemini.Framework.Commands;

using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FactrIDE.Gemini.Modules.Factorio.Commands
{
    [CommandHandler]
    public class StopFactorioCommandHandler : CommandHandlerBase<StopFactorioCommandDefinition>
    {
        private readonly ISolutionExplorer _solutionExplorer;

        [ImportingConstructor]
        public StopFactorioCommandHandler(ISolutionExplorer solutionExplorer)
        {
            _solutionExplorer = solutionExplorer;
        }

        public override void Update(Command command)
        {
            base.Update(command);

            command.Enabled = _solutionExplorer.SelectedProject != null && Process.GetProcessesByName("factorio").Length >= 1;
        }

        public override Task Run(Command command)
        {
            var process = Process.GetProcessesByName("factorio").SingleOrDefault();
            process.EnableRaisingEvents = true;
            process.Exited += (s, e) => CommandManager.InvalidateRequerySuggested();
            process.CloseMainWindow();

            return Task.CompletedTask;
        }
    }
}