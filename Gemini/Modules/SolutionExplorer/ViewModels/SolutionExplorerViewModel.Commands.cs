using FactrIDE.Gemini.Modules.SolutionExplorer.Commands;
using FactrIDE.Gemini.Modules.SolutionExplorer.Models;
using FactrIDE.Gemini.Modules.SolutionExplorer.Providers;
using FactrIDE.Properties;

using Gemini.Framework.Commands;
using Gemini.Modules.Shell.Commands;

using Idealde.Modules.ProjectExplorer.Models;

using Ookii.Dialogs.Wpf;

using PCLExt.FileStorage;
using PCLExt.FileStorage.Files;
using PCLExt.FileStorage.Folders;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

using static FactrIDE.Gemini.Modules.SolutionExplorer.Extensions.ProjectItemExtensions;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.ViewModels
{
    public partial class SolutionExplorerViewModel
        : ICommandHandler<NewLuaFileToProjectCommandDefinition>
        , ICommandHandler<NewLocalizationFileToProjectCommandDefinition>
        , ICommandHandler<NewFolderToProjectCommandDefinition>
        , ICommandHandler<AddFileToProjectCommandDefinition>
        , ICommandHandler<AddFolderToProjectCommandDefinition>
        , ICommandHandler<AddDependencyToProjectCommandDefinition>
        , ICommandHandler<RenameItemFromProjectCommandDefinition>
        , ICommandHandler<RemoveItemFromProjectCommandDefinition>
        , ICommandHandler<SaveFileCommandDefinition>
        , ICommandHandler<SaveFileAsCommandDefinition>
    {
        void ICommandHandler<NewLuaFileToProjectCommandDefinition>.Update(Command command) { }
        async Task ICommandHandler<NewLuaFileToProjectCommandDefinition>.Run(Command command)
        {
            // get project item active this command
            if (command.Tag is ProjectItem item)
                await AddNewFile(item, ".lua").ConfigureAwait(false);
        }
        void ICommandHandler<NewLocalizationFileToProjectCommandDefinition>.Update(Command command) { }
        async Task ICommandHandler<NewLocalizationFileToProjectCommandDefinition>.Run(Command command)
        {
            // get project item active this command
            if (command.Tag is ProjectItem item)
                await AddNewFile(item, ".cfg").ConfigureAwait(false);
        }
        private async Task AddNewFile(ProjectItem parent, string extension)
        {
            if(!(WindowManager.ShowDialog(NewItemDialog) is bool result) || !result) return;

            var newItemName = NewItemDialog.Name.TrimStart().TrimEnd();
            if (!newItemName.EndsWith(extension, StringComparison.OrdinalIgnoreCase)) newItemName += extension;

            if (parent.GetProjectItemFolder() is ProjectFolder parentFolder)
            {
                if (parent.ProjectItemExistInChindren(parentFolder.ProjectLocalPath) || await parentFolder.CheckExistsAsync(newItemName).ConfigureAwait(false) == ExistenceCheckResult.FileExists)
                {
                    MessageBox.Show(string.Format(Resources.ItemAlreadyExists, newItemName), Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                parent.IsOpen = true;

                var file = await parentFolder.CreateFileAsync(newItemName, CreationCollisionOption.OpenIfExists).ConfigureAwait(false);
                Helper.OpenFileEditor(file.Path);
            }
        }

        void ICommandHandler<NewFolderToProjectCommandDefinition>.Update(Command command) { }
        async Task ICommandHandler<NewFolderToProjectCommandDefinition>.Run(Command command)
        {
            if (command.Tag is ProjectItem parent)
            {
                if (!(WindowManager.ShowDialog(NewItemDialog) is bool result) || !result) return;

                var newItemName = NewItemDialog.Name.TrimStart().TrimEnd();

                if (parent.GetProjectItemFolder() is ProjectFolder parentFolder)
                {
                    if (parent.ProjectItemExistInChindren(parentFolder.ProjectLocalPath) || await parentFolder.CheckExistsAsync(newItemName).ConfigureAwait(false) == ExistenceCheckResult.FolderExists)
                    {
                        MessageBox.Show(string.Format(Resources.ItemAlreadyExists, newItemName), Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    parent.IsOpen = true;

                    var folder = await parentFolder.CreateFolderAsync(newItemName, CreationCollisionOption.OpenIfExists).ConfigureAwait(false);
                }
            }
        }


        void ICommandHandler<AddFileToProjectCommandDefinition>.Update(Command command) { }
        async Task ICommandHandler<AddFileToProjectCommandDefinition>.Run(Command command)
        {
            if (command.Tag is ProjectItem item)
            {
                // show dialog to open files
                var dialog = new VistaOpenFileDialog { Multiselect = true, CheckFileExists = true, CheckPathExists = true };
                if (!(dialog.ShowDialog() is bool result) || !result) return;

                foreach (var fileName in dialog.FileNames)
                    await AddExistingFile(item, new FileFromPath(fileName)).ConfigureAwait(false);
            }
        }
        private async Task AddExistingFile(ProjectItem parent, IFile file, bool move = false, bool open = false)
        {
            if (file.Exists && parent.GetProjectItemFolder() is ProjectFolder parentFolder)
            {
                if (parent.ProjectItemExistInChindren(file.Name))
                {
                    MessageBox.Show(string.Format(Resources.ItemAlreadyExists, file.Name), Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                parent.IsOpen = true;

                var newFile = new FileFromPath(Path.Combine(parentFolder.Path, file.Name), true);
                if(move)
                    await file.MoveAsync(newFile).ConfigureAwait(false);
                else
                    await file.CopyAsync(newFile).ConfigureAwait(false);

                if (open)
                    Helper.OpenFileEditor(newFile.Path);
            }
        }

        void ICommandHandler<AddFolderToProjectCommandDefinition>.Update(Command command) { }
        async Task ICommandHandler<AddFolderToProjectCommandDefinition>.Run(Command command)
        {
            if (command.Tag is ProjectItem item)
            {
                var dialog = new VistaFolderBrowserDialog { ShowNewFolderButton = false };
                if (!(dialog.ShowDialog() is bool result) || !result) return;

                await AddExistingFolder(item, new FolderFromPath(dialog.SelectedPath)).ConfigureAwait(false);
            }
        }
        private async Task AddExistingFolder(ProjectItem parent, IFolder folder, bool move = false, bool open = false)
        {
            if (folder.Exists && parent.GetProjectItemFolder() is ProjectFolder parentFolder)
            {
                if (parent.ProjectItemExistInChindren(folder.Name))
                {
                    MessageBox.Show(string.Format(Resources.ItemAlreadyExists, folder.Name), Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                parent.IsOpen = true;

                var newFolder = new FolderFromPath(Path.Combine(parentFolder.Path, folder.Name), true);
                //Directory.Move(folder.Path, newFolder.Path);
                if (move)
                    await folder.MoveAsync(newFolder).ConfigureAwait(false);
                else
                    await folder.CopyAsync(newFolder).ConfigureAwait(false);
            }
        }

        void ICommandHandler<AddDependencyToProjectCommandDefinition>.Update(Command command) { }
        Task ICommandHandler<AddDependencyToProjectCommandDefinition>.Run(Command command)
        {
            if (command.Tag is ProjectItem item)
            {
                var dialog = new VistaOpenFileDialog
                {
                    Filter = "Factorio Modification|*.zip",
                    Multiselect = true,
                    CheckFileExists = true,
                    CheckPathExists = true
                };
                if (!(dialog.ShowDialog() is bool result) || !result) return Task.CompletedTask;

                foreach (var fileName in dialog.FileNames)
                    AddDependency(item, new FileFromPath(fileName));
            }

            return Task.CompletedTask;
        }
        private void AddDependency(ProjectItem parent, IFile file, bool open = false)
        {
            if (file.Exists && parent.GetItemProjectRoot().Tag is FactorioProjectInfo project)
            {
                var dependency = new ProjectDependency(file);
                var jsonRaw = $"{dependency.Data.Name} >= {dependency.Data.Version}";
                switch (parent.ProjectItemDefintion)
                {
                    case FactorioDependencyRequiredListProjectItemDefinition _:
                        jsonRaw = $"{dependency.Data.Name} >= {dependency.Data.Version}";
                        break;
                    case FactorioDependencyOptionalListProjectItemDefinition _:
                        jsonRaw = $"? {dependency.Data.Name} >= {dependency.Data.Version}";
                        break;
                    case FactorioDependencyNotCompatibleListProjectItemDefinition _:
                        jsonRaw = $"! {dependency.Data.Name} >= {dependency.Data.Version}";
                        break;
                }
                var data = new FactorioModificationDependency(jsonRaw);

                if (parent.ProjectItemExistInChindren(data.DisplayText))
                {
                    MessageBox.Show(string.Format(Resources.ItemAlreadyExists, data.DisplayText), Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                parent.IsOpen = true;

                project.Dependencies.Add(new FactorioModificationDependency(data.JsonRaw()));
            }
        }


        void ICommandHandler<RenameItemFromProjectCommandDefinition>.Update(Command command)
        {
            if (GetProjectItemFromCommand(command) is ProjectItem item)
            {
                if (item.ProjectItemDefintion is FileProjectItemDefinition)
                {
                    command.Enabled = true;
                }
                else if (item.ProjectItemDefintion is FolderProjectItemDefinition)
                {
                    command.Enabled = true;
                }
                else if (item.ProjectItemDefintion is FactrIDESolutionItemDefinition)
                {
                    command.Enabled = true;
                }
                else if (item.ProjectItemDefintion is FactorioProjectItemDefinition)
                {
                    command.Enabled = true;
                }
                else
                    command.Enabled = false;
            }
            else
                command.Enabled = false;
        }
        async Task ICommandHandler<RenameItemFromProjectCommandDefinition>.Run(Command command)
        {
            if (GetProjectItemFromCommand(command) is ProjectItem item)
            {
                if (item.ProjectItemDefintion is FactrIDESolutionItemDefinition && item.Tag is FactrIDESolutionInfo solutionInfo)
                {
                    NewItemDialog.Name = solutionInfo.Name;
                    var result = WindowManager.ShowDialog(NewItemDialog) ?? false;
                    if (!result) return;
                    solutionInfo.Name = NewItemDialog.Name.TrimStart().TrimEnd();
                }
                else if (item.ProjectItemDefintion is FactorioProjectItemDefinition && item.Tag is FactorioProjectInfo projectInfo)
                {
                    NewItemDialog.Name = projectInfo.Name;
                    var result = WindowManager.ShowDialog(NewItemDialog) ?? false;
                    if (!result) return;
                    projectInfo.Name = NewItemDialog.Name.TrimStart().TrimEnd();
                }
                else
                {
                    NewItemDialog.Name = item.Text;
                    var result = WindowManager.ShowDialog(NewItemDialog) ?? false;
                    if (!result) return;
                    var name = NewItemDialog.Name.TrimStart().TrimEnd();

                    if (item.ProjectItemDefintion is FolderProjectItemDefinition && item.Tag is ProjectFolder folder)
                    {
                        await folder.RenameAsync(name).ConfigureAwait(false);
                    }
                    else if (item.ProjectItemDefintion is FileProjectItemDefinition && item.Tag is ProjectFile file)
                    {
                        await file.RenameAsync(name).ConfigureAwait(false);
                    }

                    item.Text = NewItemDialog.Name.TrimStart().TrimEnd();
                }
            }
        }


        void ICommandHandler<RemoveItemFromProjectCommandDefinition>.Update(Command command) { }
        async Task ICommandHandler<RemoveItemFromProjectCommandDefinition>.Run(Command command)
        {
            if (GetProjectItemFromCommand(command) is ProjectItem item)
            {
                var parent = item.Parent;
                if (parent == null) return;

                if (item.ProjectItemDefintion is FolderProjectItemDefinition && ConfirmRemoveFolder() && item.Tag is ProjectFolder folder)
                {
                    await folder.DeleteAsync().ConfigureAwait(false);
                }
                else if (item.ProjectItemDefintion is FileProjectItemDefinition && ConfirmRemoveFile(item.Text) && item.Tag is ProjectFile file)
                {
                    await file.DeleteAsync().ConfigureAwait(false);
                }
                else if (item.ProjectItemDefintion is FactorioDependencyProjectItemDefinition && ConfirmRemoveFolder() && item.Tag is string jsonRaw)
                {
                    if (item.GetItemProjectRoot().Tag is FactorioProjectInfo project)
                    {
                        var dependency = project.Dependencies.Single(d => d is FactorioModificationDependency fmd && fmd.JsonRaw().Equals(jsonRaw, StringComparison.InvariantCulture));
                        project.Dependencies.Remove(dependency);
                    }
                }
            }
        }
        private bool ConfirmRemoveFile(string name) =>
            MessageBox.Show(string.Format("Resources.DoYouWantToRemoveText", name), "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        private bool ConfirmRemoveFolder() =>
            MessageBox.Show("Resources.DoYouWantToRemoveFolderText", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes;


        void ICommandHandler<SaveFileCommandDefinition>.Update(Command command)
        {
            if (GetProjectItemFromCommand(command) is ProjectItem item)
            {
                if(item.ProjectItemDefintion is FactrIDESolutionItemDefinition && item.Tag is FactrIDESolutionInfo solutionInfo)
                {
                    command.Text = Resources.SaveSolutionCommandText;
                    command.ToolTip = Resources.SaveSolutionCommandToolTip;
                    command.Enabled = solutionInfo.HasChanges;
                }
                if (item.ProjectItemDefintion is FactrIDESolutionItemDefinition && item.Tag is NoSolutionInfo)
                {
                    command.Text = Resources.SaveSolutionCommandText;
                    command.ToolTip = Resources.SaveSolutionCommandToolTip;
                    command.Enabled = true;
                }
                else if(item.ProjectItemDefintion is FactorioProjectItemDefinition && item.Tag is FactorioProjectInfo projectInfo)
                {
                    command.Text = Resources.SaveProjectCommandText;
                    command.ToolTip = Resources.SaveProjectCommandToolTip;
                    command.Enabled = projectInfo.HasChanges;
                }
                else
                {
                    command.Text = Resources.SaveEmptyCommandText;
                    command.ToolTip = Resources.SaveEmptyCommandToolTip;
                    command.Enabled = false;
                }
            }

            if (string.IsNullOrWhiteSpace(command.ToolTip))
                command.ToolTip = command.Text;
        }
        async Task ICommandHandler<SaveFileCommandDefinition>.Run(Command command)
        {
            if (GetProjectItemFromCommand(command) is ProjectItem item)
            {
                if (item.ProjectItemDefintion is FactrIDESolutionItemDefinition)
                {
                    SaveSolution();
                }
                else if (item.ProjectItemDefintion is FactorioProjectItemDefinition && item.Tag is FactorioProjectInfo projectInfo)
                {
                    await projectInfo.Save().ConfigureAwait(false);
                }
                else
                {
                    command.Text = Resources.SaveEmptyCommandText;
                    command.ToolTip = Resources.SaveEmptyCommandToolTip;
                    command.Enabled = false;
                }
            }
        }

        void ICommandHandler<SaveFileAsCommandDefinition>.Update(Command command)
        {
            command.Enabled = false;
        }
        Task ICommandHandler<SaveFileAsCommandDefinition>.Run(Command command)
        {
            return Task.CompletedTask;
        }
    }
}