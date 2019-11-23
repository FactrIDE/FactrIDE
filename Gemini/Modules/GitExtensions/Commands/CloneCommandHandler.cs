using FactrIDE.Gemini.Modules.SolutionExplorer;

using Gemini.Framework.Commands;
using Gemini.Framework.Services;

using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.GitExtensions.Commands
{
    [CommandHandler]
    public class CloneCommandHandler : Git2CommandHandlerBase<CloneCommandDefinition>
    {
        [ImportingConstructor]
        public CloneCommandHandler(IShell shell, ISolutionExplorer solutionExplorer) : base(shell, solutionExplorer) { }

        public override Task Run(string filePath, string projectPath)
        {
            RunGitEx("clone", projectPath);
            return Task.CompletedTask;
        }
    }
}