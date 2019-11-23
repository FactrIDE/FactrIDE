using Gemini.Framework.Commands;
using Gemini.Framework.Services;

using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.FactorioStandardOutput.Commands
{
    [CommandHandler]
    public class ViewFactorioStandardOutputCommandHandler : CommandHandlerBase<ViewFactorioStandardOutputCommandDefinition>
    {
        private readonly IShell _shell;
        private readonly IFactorioStandardOutput _output;

        [ImportingConstructor]
        public ViewFactorioStandardOutputCommandHandler(IShell shell, IFactorioStandardOutput output)
        {
            _shell = shell;
            _output = output;
        }

        public override Task Run(Command command)
        {
            _shell.ShowTool(_output);
            return Task.CompletedTask;
        }
    }
}