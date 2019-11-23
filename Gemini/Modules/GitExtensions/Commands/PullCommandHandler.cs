using FactrIDE.Gemini.Modules.SolutionExplorer;

using Gemini.Framework.Commands;
using Gemini.Framework.Services;

using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.GitExtensions.Commands
{
    [CommandHandler]
    public class PullCommandHandler : Git1CommandHandlerBase<PullCommandDefinition>
    {
        [ImportingConstructor]
        public PullCommandHandler(IShell shell, ISolutionExplorer solutionExplorer) : base(shell, solutionExplorer) { }

        public override Task Run(string filePath, string projectPath)
        {
            RunGitEx("pull", projectPath);
            return Task.CompletedTask;
        }
    }
}