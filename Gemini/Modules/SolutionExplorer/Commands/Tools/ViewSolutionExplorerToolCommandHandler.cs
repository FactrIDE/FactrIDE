using System.ComponentModel.Composition;
using System.Threading.Tasks;

using Gemini.Framework.Commands;
using Gemini.Framework.Services;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandHandler]
    public class ViewSolutionExplorerToolCommandHandler : CommandHandlerBase<ViewSolutionExplorerToolCommandDefinition>
    {
        private readonly IShell _shell;
        private readonly ISolutionExplorer _solutionExplorer;

        [ImportingConstructor]
        public ViewSolutionExplorerToolCommandHandler(IShell shell, ISolutionExplorer solutionExplorer)
        {
            _shell = shell;
            _solutionExplorer = solutionExplorer;
        }

        public override Task Run(Command command)
        {
            _shell.ShowTool(_solutionExplorer);
            return Task.CompletedTask;
        }
    }
}