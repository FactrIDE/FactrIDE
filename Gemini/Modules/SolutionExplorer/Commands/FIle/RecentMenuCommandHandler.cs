using System.Threading.Tasks;

using Gemini.Framework.Commands;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandHandler]
    public class RecentMenuCommandHandler : CommandHandlerBase<RecentMenuCommandDefinition>
    {
        public override Task Run(Command command)
        {
            return Task.CompletedTask;
        }
    }
}