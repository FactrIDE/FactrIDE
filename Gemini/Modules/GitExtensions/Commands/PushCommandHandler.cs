using FactrIDE.Gemini.Modules.SolutionExplorer;

using Gemini.Framework.Commands;
using Gemini.Framework.Services;

using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.GitExtensions.Commands
{
    [CommandHandler]
    public class PushCommandHandler : Git1CommandHandlerBase<PushCommandDefinition>
    {
        [ImportingConstructor]
        public PushCommandHandler(IShell shell, ISolutionExplorer solutionExplorer) : base(shell, solutionExplorer) { }

        public override Task Run(string filePath, string projectPath)
        {
            RunGitEx("push", projectPath);
            return Task.CompletedTask;
        }
    }
}