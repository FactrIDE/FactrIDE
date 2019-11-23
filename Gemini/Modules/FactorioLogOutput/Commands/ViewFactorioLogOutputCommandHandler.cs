using Gemini.Framework.Commands;
using Gemini.Framework.Services;

using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.FactorioLogOutput.Commands
{
    [CommandHandler]
    public class ViewFactorioLogOutputCommandHandler : CommandHandlerBase<ViewFactorioLogOutputCommandDefinition>
    {
        private readonly IShell _shell;
        private readonly IFactorioLogOutput _output;

        [ImportingConstructor]
        public ViewFactorioLogOutputCommandHandler(IShell shell, IFactorioLogOutput output)
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