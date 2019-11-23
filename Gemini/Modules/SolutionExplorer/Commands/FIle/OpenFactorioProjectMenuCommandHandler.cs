using Gemini.Framework.Commands;
using Gemini.Framework.Services;

using Idealde.Framework.ProjectExplorer.Models;

using Ookii.Dialogs.Wpf;

using PCLExt.FileStorage.Files;

using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandHandler]
    public class OpenFactorioProjectMenuCommandHandler : CommandHandlerBase<OpenFactorioProjectMenuCommandDefinition>
    {
        private readonly IShell _shell;
        private readonly IProjectProvider[] _projectProviders;
        private readonly ISolutionExplorer _solutionExplorer;

        [ImportingConstructor]
        public OpenFactorioProjectMenuCommandHandler(IShell shell, [ImportMany] IProjectProvider[] projectProviders, ISolutionExplorer solutionExplorer)
        {
            _shell = shell;
            _projectProviders = projectProviders;
            _solutionExplorer = solutionExplorer;
        }

        public override Task Run(Command command)
        {
            var dialog = new VistaOpenFileDialog
            {
                Title = "Resources.OpenFileDialogTitle",
                Multiselect = false,
                Filter = "Factorio Modification Info|Info.json"
            };
            if (dialog.ShowDialog() == true)
            {
                // find provider
                var extension = Path.GetExtension(dialog.FileName);

                var projectProvider = System.Array.Find(_projectProviders,
                    p => p.ExtensionTypes.Any(
                        t => string.Equals(t.Extension, extension, System.StringComparison.InvariantCultureIgnoreCase)));
                if (projectProvider != null)
                {
                    var projectInfo = projectProvider.Load(new FileFromPath(dialog.FileName));
                    _solutionExplorer.Load(projectInfo);
                    _shell.ShowTool(_solutionExplorer);
                }
            }

            /*
            var dialog = new VistaOpenFileDialog
            {
                Title = "Resources.OpenFileDialogTitle",
                Multiselect = false,
                Filter = "All Supported Files|" + string.Join(";", _projectProviders.SelectMany(p => p.ExtensionTypes)
                             .Select(t => $"*{t.Extension}"))
            };

            // generate filters
            foreach (var provider in _projectProviders)
            {
                dialog.Filter += $"|{provider.Name}|{string.Join(";", provider.ExtensionTypes.Select(t => $"*{t.Extension}"))}";
            }

            if (dialog.ShowDialog() == true)
            {
                // find provider
                var extension = Path.GetExtension(dialog.FileName);
                var explorer = IoC.Get<ISolutionExplorer>();

                var projectProvider = System.Array.Find(_projectProviders,
                    p => p.ExtensionTypes.Any(
                        t => string.Equals(t.Extension, extension, System.StringComparison.InvariantCultureIgnoreCase)));
                if (projectProvider != null)
                {
                    explorer.LoadProject(new FileFromPath(dialog.FileName), projectProvider);
                    _shell.ShowTool(explorer);
                }
            }
            */

            return Task.CompletedTask;
        }
    }
}