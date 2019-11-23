using FactrIDE.Storage.Folders;

using Gemini.Framework.Commands;
using Gemini.Framework.Services;

using Ookii.Dialogs.Wpf;

using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.FactorioModificationOutput.Commands
{
    [CommandHandler]
    public class ViewFactorioModOutputCommandHandler : CommandHandlerBase<ViewFactorioModOutputCommandDefinition>
    {
        private readonly IShell _shell;
        private readonly IFactorioModificationOutput _output;

        [ImportingConstructor]
        public ViewFactorioModOutputCommandHandler(IShell shell, IFactorioModificationOutput output)
        {
            _shell = shell;
            _output = output;
        }

        public override Task Run(Command command)
        {
            var dialog = new VistaOpenFileDialog
            {
                Filter = "Log Files (*.log)|*.log",
                InitialDirectory = new FactorioScriptOutputFolder().Path
            };

            if (dialog.ShowDialog() == true)
            {
                _output.SetFilePath(dialog.FileName);
                _shell.ShowTool(_output);
            }

            return Task.CompletedTask;
        }
    }
}