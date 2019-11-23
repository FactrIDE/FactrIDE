using FactrIDE.Gemini.Modules.SolutionExplorer.Providers;

using Gemini.Framework.Commands;
using Gemini.Framework.Services;

using Ookii.Dialogs.Wpf;

using PCLExt.FileStorage.Files;

using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandHandler]
    public class OpenFactrIDESolutionMenuCommandHandler : CommandHandlerBase<OpenFactrIDESolutionMenuCommandDefinition>
    {
        private readonly IShell _shell;
        private readonly ISolutionProvider[] _solutionProviders;
        private readonly ISolutionExplorer _solutionExplorer;

        [ImportingConstructor]
        public OpenFactrIDESolutionMenuCommandHandler(IShell shell, [ImportMany] ISolutionProvider[] solutionProviders, ISolutionExplorer solutionExplorer)
        {
            _shell = shell;
            _solutionProviders = solutionProviders;
            _solutionExplorer = solutionExplorer;
        }

        public override async Task Run(Command command)
        {
            var dialog = new VistaOpenFileDialog
            {
                Title = "Resources.OpenFileDialogTitle",
                Multiselect = false,
                Filter = "All Supported Files|" + string.Join(";", _solutionProviders.SelectMany(p => p.ExtensionTypes)
                             .Select(t => $"*{t.Extension}"))
            };

            // generate filters
            foreach (var provider in _solutionProviders)
            {
                dialog.Filter += $"|{provider.Name}|{string.Join(";", provider.ExtensionTypes.Select(t => $"*{t.Extension}"))}";
            }

            if (dialog.ShowDialog() == true)
            {
                // find provider
                var extension = Path.GetExtension(dialog.FileName);

                var solutionProvider = System.Array.Find(_solutionProviders,
                    p => p.ExtensionTypes.Any(
                        t => string.Equals(t.Extension, extension, System.StringComparison.InvariantCultureIgnoreCase)));
                if (solutionProvider != null)
                {
                    var solutionInfo = await solutionProvider.Load(new FileFromPath(dialog.FileName)).ConfigureAwait(false);
                    _solutionExplorer.Load(solutionInfo);
                    _shell.ShowTool(_solutionExplorer);
                }
            }
        }
    }
}