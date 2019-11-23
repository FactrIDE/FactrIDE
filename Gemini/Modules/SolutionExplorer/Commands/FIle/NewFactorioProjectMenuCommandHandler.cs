using Caliburn.Micro;

using FactrIDE.Gemini.Modules.SolutionExplorer.Extensions;
using FactrIDE.Gemini.Modules.SolutionExplorer.Providers;
using FactrIDE.Gemini.Modules.SolutionExplorer.ViewModels;

using Gemini.Framework.Commands;

using PCLExt.FileStorage;
using PCLExt.FileStorage.Folders;

using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandHandler]
    public class NewFactorioProjectMenuCommandHandler : CommandHandlerBase<NewFactorioProjectMenuCommandDefinition>
    {
        private readonly IWindowManager _windowManager;
        private readonly NewProjectSettingsViewModel _newProjectDialog;
        private readonly ISolutionExplorer _solutionExplorer;

        [ImportingConstructor]
        public NewFactorioProjectMenuCommandHandler(IWindowManager windowManager, NewProjectSettingsViewModel newProjectDialog, ISolutionExplorer solutionExplorer)
        {
            _windowManager = windowManager;
            _newProjectDialog = newProjectDialog;
            _solutionExplorer = solutionExplorer;
        }

        public override async Task Run(Command command)
        {
            if (!(_windowManager.ShowDialog(_newProjectDialog) is bool result) || !result) return;

            var root = new FolderFromPath(_newProjectDialog.RootLocation);
            var solutionName = _newProjectDialog.SolutionName.TrimStart().TrimEnd();
            var solutionFolder = await root.CreateFolderAsync(_newProjectDialog.SolutionName, CreationCollisionOption.OpenIfExists).ConfigureAwait(false);
            var projectName = _newProjectDialog.ProjectName.TrimStart().TrimEnd();
            var projectFolder = _newProjectDialog.ProjectInRoot
                ? new FolderFromPath(solutionFolder.Path)
                : await solutionFolder.CreateFolderAsync(projectName, CreationCollisionOption.OpenIfExists).ConfigureAwait(false);

            var solutionData = new SolutionDataBase()
            {
                Name = solutionName,
                Projects =
                {
                    new ProjectData()
                    {
                        Name = projectName,
                        Path = solutionFolder.GetProjectPath(projectFolder)
                    }
                }
            };

            var solutionProvider = new FactrIDESolutionProvider();
            var solutioFile = await solutionProvider.Create(solutionData, solutionFolder).ConfigureAwait(false);
            var solutionInfo = await solutionProvider.Load(solutioFile).ConfigureAwait(false);

            _solutionExplorer.Load(solutionInfo);
        }
    }
}