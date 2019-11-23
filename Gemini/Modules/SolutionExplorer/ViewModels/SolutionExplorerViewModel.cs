using Caliburn.Micro;

using FactrIDE.Gemini.Modules.FactorioLogOutput;
using FactrIDE.Gemini.Modules.FactorioStandardOutput;
using FactrIDE.Gemini.Modules.SolutionExplorer.Extensions;
using FactrIDE.Gemini.Modules.SolutionExplorer.Models;
using FactrIDE.Gemini.Modules.SolutionExplorer.Providers;
using FactrIDE.Properties;

using Gemini.Framework;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Modules.Inspector;

using Idealde.Framework.Projects;
using Idealde.Modules.ProjectExplorer.Models;

using Ookii.Dialogs.Wpf;

using PCLExt.FileStorage;
using PCLExt.FileStorage.Files;
using PCLExt.FileStorage.Folders;

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.ViewModels
{
    [Export(typeof(ISolutionExplorer))]
    public partial class SolutionExplorerViewModel : Tool, ISolutionExplorer
    {
        public override PaneLocation PreferredLocation => PaneLocation.Left;

        //public ProjectItemBase Root { get; }
        public IObservableCollection<ProjectItemBase> ItemTree { get; } = new BindableCollection<ProjectItemBase>();

        private ProjectItemBase _selectedItem;
        public ProjectItemBase SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (Equals(value, _selectedItem)) return;
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        private SolutionInfoBase _currentSolutionInfo;
        public SolutionInfoBase SolutionInfo
        {
            get => _currentSolutionInfo;
            set
            {
                if (Equals(_currentSolutionInfo, value)) return;
                _currentSolutionInfo = value;
                //RefreshProject();
            }
        }

        public ProjectInfoBase SelectedProject => (SelectedItem as ProjectItem)?.GetItemProjectRoot().Tag as ProjectInfoBase;

        private ICommandService CommandService { get; }
        private IShell Shell { get; }
        private IWindowManager WindowManager { get; }
        private IInspectorTool InspectorTool { get; }
        private IFactorioStandardOutput StandardOutput { get; }
        private IFactorioLogOutput LogOutput { get; }
        private NewItemDialogViewModel NewItemDialog { get; }

        [ImportingConstructor]
        public SolutionExplorerViewModel(ICommandService commandService, IShell shell, IWindowManager windowManager, IInspectorTool inspectorTool, IFactorioStandardOutput standardOutput, IFactorioLogOutput logOutput, NewItemDialogViewModel newItemViewModel)
        {
            CommandService = commandService;
            Shell = shell;
            WindowManager = windowManager;
            InspectorTool = inspectorTool;
            StandardOutput = standardOutput;
            LogOutput = logOutput;
            NewItemDialog = newItemViewModel;

            DisplayName = "Solution Explorer";
        }

        private async void SaveSolution()
        {
            SaveNewSolution();

            await SolutionInfo.Save().ConfigureAwait(false);
        }

        public void Load(SolutionInfoBase solutionInfo)
        {
            // confirm close old project
            if (SolutionInfo != null)
            {
                if (!ConfirmCloseCurrentProject()) return;
                Close();
            }

            // update current project
            SolutionInfo = solutionInfo;

            RefreshSolution();
        }
        public void Load(ProjectInfoBase projectInfo)
        {
            // confirm close old project
            if (SolutionInfo != null)
            {
                if (!ConfirmCloseCurrentProject()) return;
                Close();
            }

            SolutionInfo = new NoSolutionInfo()
            {
                Name = projectInfo.Name,
                Folder = projectInfo.Folder,
                Projects = { projectInfo }
            };

            RefreshSolution();
        }
        public void AddProject(ProjectInfoBase projectInfo)
        {
            // confirm close old project
            if (SolutionInfo == null)
                return;

            SolutionInfo.Projects.Add(projectInfo);
        }

        private bool ConfirmCloseCurrentProject() => MessageBox.Show(
            string.Format("Resources.AreYouWantToCloseProjectText", SolutionInfo.Name),
            "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes;

        public void Close()
        {
            if (SolutionInfo.HasChanges && MessageBox.Show(
                string.Format("Resources.AskForSaveFileBeforeExit", SolutionInfo.Name), "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                SaveSolution();
            }

            ItemTree.Clear();
        }

        private void RefreshSolution()
        {
            ItemTree.Clear();

            var solutionRoot = new ProjectItem<FactrIDESolutionItemDefinition>()
            {
                //Text = $"Solution '{CurrentSolutionInfo.SolutionName}' ({CurrentSolutionInfo.Projects.Count} projects)",
                Text = $"Solution '{SolutionInfo.Name}'",
                Tag = SolutionInfo,
            };
            foreach (var project in SolutionInfo.Projects)
            {
                var projectRoot = new ProjectItem<FactorioProjectItemDefinition>() { Tag = project };
                project.OnProjectChange += () =>
                {
                    var localProjectRoot = projectRoot;
                    RefreshProject(localProjectRoot);
                };
                RefreshProject(projectRoot);
                //projectRoot.IsOpen = true;
                solutionRoot.AddChild(projectRoot);
            }

            if (SolutionInfo.Projects.Count == 1)
                solutionRoot.Children[0].IsOpen = true;

            solutionRoot.IsOpen = true;
            ItemTree.Add(solutionRoot);
        }

        private (List<ProjectFile> Files, List<ProjectFolder> Folders, List<DependencyBase> Dependencies, IList<ProjectFile> IgnoreFiles, IList<ProjectFolder> IgnoreFolders) GetProjectDataFromTree(ProjectItem project)
        {
            var files = new List<ProjectFile>();
            var folders = new List<ProjectFolder>();
            var dependencies = new List<DependencyBase>();

            var stack = new Stack<ProjectItemBase>();
            stack.Push(project);
            while (stack.Count > 0)
            {
                var parentItem = stack.Pop();
                if (parentItem == null) continue;

                foreach (var item in parentItem.Children.OfType<ProjectItem>())
                {
                    if (item.ProjectItemDefintion is FileProjectItemDefinition)
                    {
                        if (item?.Tag is ProjectFile fileTag)
                        {
                            files.Add(fileTag);
                        }
                        //else
                        //    ;
                    }
                    else if (item.ProjectItemDefintion is FolderProjectItemDefinition)
                    {
                        if (item?.Tag is ProjectFolder folderTag)
                        {
                            folders.Add(folderTag);
                            stack.Push(item);
                        }
                        //else
                        //    ;
                    }
                    else if (item.ProjectItemDefintion is FactorioDependencyListProjectItemDefinition
                        || item.ProjectItemDefintion is FactorioDependencyRequiredListProjectItemDefinition
                        || item.ProjectItemDefintion is FactorioDependencyOptionalListProjectItemDefinition
                        || item.ProjectItemDefintion is FactorioDependencyNotCompatibleListProjectItemDefinition)
                    {
                        stack.Push(item);
                    }
                    else if (item.ProjectItemDefintion is FactorioDependencyProjectItemDefinition)
                    {
                        dependencies.Add(new FactorioModificationDependency(item?.Tag as string));
                    }
                }
            }

            return (files, folders, dependencies, new List<ProjectFile>(), new List<ProjectFolder>());
        }
        private void RebuildProjectTree(ProjectItem projectRoot, (IList<ProjectFile> Files, IList<ProjectFolder> Folders, IList<DependencyBase> Dependencies, IList<ProjectFile> IgnoreFiles, IList<ProjectFolder> IgnoreFolders) projectLists)
        {
            static void SaveOpenHistory(ProjectItemBase root, string name, IDictionary<string, bool> previousOpenHistory)
            {
                var newName = $"{name}|{root.Text}";
                previousOpenHistory.Add(newName, root.IsOpen);
                foreach (var child in root.Children)
                    SaveOpenHistory(child, newName, previousOpenHistory);
            }
            static void LoadOpenHistory(ProjectItemBase root, string name, IDictionary<string, bool> previousOpenHistory)
            {
                var newName = $"{name}|{root.Text}";
                root.IsOpen = previousOpenHistory.TryGetValue(newName, out var isOpen) && isOpen;
                foreach (var child in root.Children)
                    LoadOpenHistory(child, newName, previousOpenHistory);
            }

            var openHistory = new Dictionary<string, bool>();
            SaveOpenHistory(projectRoot, "", openHistory);


            projectRoot.Children.Clear();

            var dependencyList = new ProjectItem<FactorioDependencyListProjectItemDefinition>() { Text = Resources.ProjectDependencyText };
            var requiredDependencies = new ProjectItem<FactorioDependencyRequiredListProjectItemDefinition>() { Text = Resources.ProjectDependencyRequiredText };
            dependencyList.AddChild(requiredDependencies);
            var optionalDependencies = new ProjectItem<FactorioDependencyOptionalListProjectItemDefinition>() { Text = Resources.ProjectDependencyOptionalText };
            dependencyList.AddChild(optionalDependencies);
            var notCompatibleDependencies = new ProjectItem<FactorioDependencyNotCompatibleListProjectItemDefinition>() { Text = Resources.ProjectDependencyNotCompatibleText };
            dependencyList.AddChild(notCompatibleDependencies);
            foreach (var dependency in projectLists.Dependencies.OfType<FactorioModificationDependency>())
            {
                var item = new ProjectItem<FactorioDependencyProjectItemDefinition>()
                {
                    //Text = $"{dependency.Name} {dependency.Version}",
                    Text = dependency.DisplayText,
                    Tag = dependency.JsonRaw()
                };
                switch (dependency.State)
                {
                    case FactorioModificationDependency.DependencyState.Required:
                        requiredDependencies.AddChild(item);
                        break;
                    case FactorioModificationDependency.DependencyState.Optional:
                        optionalDependencies.AddChild(item);
                        break;
                    case FactorioModificationDependency.DependencyState.NotCompatible:
                        notCompatibleDependencies.AddChild(item);
                        break;
                }
            }
            projectRoot.AddChild(dependencyList);

            var projectFolder = (projectRoot.Tag as ProjectInfoBase)!.Folder;
            var (IgnoreFiles, IgnoreFolders) = GetAllIgnoredItems(projectFolder, projectLists.IgnoreFiles, projectLists.IgnoreFolders);

            var allFiles = Directory.GetFiles(projectFolder.Path, "*", SearchOption.AllDirectories).
                Select(f => new ProjectFile(projectFolder, new FileFromPath(f.NormalizePath())));

            var allFolders = Directory.GetDirectories(projectFolder.Path, "*", SearchOption.AllDirectories)
                .Select(d => new ProjectFolder(projectFolder, new FolderFromPath(d.NormalizePath().GetFullPathWithEndingSlashes())));

            var folders = allFolders//projectLists.Folders
                .ExceptBy(IgnoreFolders, f => f.Path.NormalizePath().GetFullPathWithEndingSlashes(), StringComparer.InvariantCulture)
                .OrderBy(d => d.ProjectLocalPath);
            foreach (var folder in folders)
                GenerateItemTree(folder, projectRoot);

            var files = allFiles//projectLists.Files
                .ExceptBy(IgnoreFiles, f => f.Path.NormalizePath(), StringComparer.InvariantCulture)
                .OrderBy(d => d.ProjectLocalPath);
            foreach (var fileInfo in files)
            {
                var parent = GenerateItemTree(fileInfo, projectRoot);
                if (parent == null) continue;

                var fileName = Path.GetFileName(fileInfo.ProjectLocalPath);
                parent.AddChild(new ProjectItem<FileProjectItemDefinition>
                {
                    Text = fileName,
                    Tag = fileInfo
                });
            }

            LoadOpenHistory(projectRoot, "", openHistory);
        }
        private (IList<ProjectFile> IgnoreFiles, IList<ProjectFolder> IgnoreFolders) GetAllIgnoredItems(IFolder projectFolder, IList<ProjectFile> ignoreFiles, IList<ProjectFolder> ignoreFolders)
        {
            var newIgnoreFiles = new List<ProjectFile>();
            var newIgnoreFolders = new List<ProjectFolder>();

            newIgnoreFiles.AddRange(ignoreFiles);
            newIgnoreFolders.AddRange(ignoreFolders);

            foreach (var ignoreFolder in ignoreFolders)
            {
                var folders = Directory.GetDirectories(ignoreFolder.Path, "*", SearchOption.AllDirectories);
                newIgnoreFolders.AddRange(folders.Select(f => new ProjectFolder(projectFolder, new FolderFromPath(f))));

                var files = Directory.GetFiles(ignoreFolder.Path, "*", SearchOption.AllDirectories);
                newIgnoreFiles.AddRange(files.Select(f => new ProjectFile(projectFolder, new FileFromPath(f))));
            }

            //var files = Directory.GetFiles(projectFolder.Path, "*", SearchOption.AllDirectories);
            //var folders = Directory.GetDirectories(projectFolder.Path, "*", SearchOption.AllDirectories);

            //newIgnoreFiles.AddRange(files.Select(f => new ProjectFile(projectFolder, new FileFromPath(f))));
            //newIgnoreFolders.AddRange(folders.Select(f => new ProjectFolder(projectFolder, new FolderFromPath(f))));

            return (newIgnoreFiles, newIgnoreFolders);
        }

        private void RefreshProject(ProjectItem projectRoot)
        {
            if(projectRoot.ProjectItemDefintion is FactorioProjectItemDefinition && projectRoot.Tag is ProjectInfoBase project)
            {
                //await project.Save();
                //var list = BuildProjectLists(projectRoot);
                projectRoot.Text = $"{project.Name}_{project.Version}";
                var list = (project.Files, project.Folders, project.Dependencies, project.IgnoreFiles, project.IgnoreFolders);
                RebuildProjectTree(projectRoot, list);
            }
        }

        private ProjectItemBase GenerateItemTree(IProjectEntity projectEntity, ProjectItemBase root)
        {
            var localFolder = projectEntity is ProjectFolder folder
                ? folder.ProjectLocalPath
                : (projectEntity is ProjectFile file
                    ? Path.GetDirectoryName(file.ProjectLocalPath)
                    : string.Empty);

            var parentNames = localFolder.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            if (parentNames.Length == 0) return root;

            // find first item
            var parentItem = root.Children
                .FirstOrDefault(i => i is ProjectItem item
                    && item?.ProjectItemDefintion is FolderProjectItemDefinition folderProjectItemDefinition
                    && string.Equals(item.Text, parentNames[0], StringComparison.OrdinalIgnoreCase));
            // create if not exist
            if (parentItem == null)
            {
                parentItem = new ProjectItem<FolderProjectItemDefinition>
                {
                    Text = parentNames[0],
                    Tag = new ProjectFolder(projectEntity.Project, localFolder)
                };
                root.AddChild(parentItem);
            }

            for (var i = 1; i < parentNames.Length; i++)
            {
                // find next item
                var childItem = parentItem.Children
                    .FirstOrDefault(it => it is ProjectItem item
                        && item?.ProjectItemDefintion is FolderProjectItemDefinition
                        && string.Equals(item.Text, parentNames[i], StringComparison.OrdinalIgnoreCase));
                // create if not exist
                if (childItem == null)
                {
                    childItem = new ProjectItem<FolderProjectItemDefinition>
                    {
                        Text = parentNames[i],
                        Tag = new ProjectFolder(projectEntity.Project, localFolder)
                    };
                    parentItem.AddChild(childItem);
                }

                parentItem = childItem;
            }

            return parentItem;
        }


        private void SaveNewSolution()
        {
            if (SolutionInfo is NoSolutionInfo solutionInfo)
            {
                var saveDialog = new VistaSaveFileDialog()
                {
                    InitialDirectory = solutionInfo.Folder.Path,
                    CheckFileExists = false,
                    CheckPathExists = false,
                    CreatePrompt = true,
                    OverwritePrompt = true,
                    FileName = solutionInfo.Name,
                    AddExtension = true,
                    Filter = "FactrIDE Solution|.fms"
                };
                if (saveDialog.ShowDialog() != true) return;

                var solutionFilePath = saveDialog.FileName;
                var solutionFileName = Path.GetFileNameWithoutExtension(solutionFilePath);
                var solutionFolderPath = Path.GetDirectoryName(solutionFilePath);

                var project = _currentSolutionInfo.Projects[0];
                var projectName = project.Name;
                var projectFolderPath = project.Folder.Path;

                var projectFolderPathUri = new Uri(projectFolderPath.GetFullPathWithEndingSlashes(), UriKind.Absolute);
                var solutionFolderPathUri = new Uri(solutionFolderPath.GetFullPathWithEndingSlashes(), UriKind.Absolute);
                string projectPath;
                // Try to get relative path
                try { projectPath = solutionFolderPathUri.MakeRelativeUri(projectFolderPathUri).ToString(); }
                catch (Exception e) when(e is InvalidOperationException) { projectPath = projectFolderPath; }

                _currentSolutionInfo = new FactrIDESolutionInfo(new FolderFromPath(solutionFolderPath),
                    new SolutionDataBase()
                    {
                        Name = solutionFileName,
                        Projects =
                        {
                            new ProjectData()
                            {
                                Name = projectName,
                                Path = projectPath
                            }
                        }
                    }) ;

                RefreshSolution();
            }
        }


        private object GetProjectItemFromCommand(Command command) => command.Tag ?? SelectedItem;
    }
}