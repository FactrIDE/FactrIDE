using FactrIDE.Gemini.Modules.FactorioModPortal;
using FactrIDE.Gemini.Modules.SolutionExplorer.Commands;
using FactrIDE.Gemini.Modules.SolutionExplorer.Extensions;
using FactrIDE.Gemini.Modules.SolutionExplorer.Inspectors;
using FactrIDE.Gemini.Modules.SolutionExplorer.Models;
using FactrIDE.Gemini.Modules.SolutionExplorer.Providers;

using Gemini.Framework.Commands;
using Gemini.Modules.Inspector;
using Gemini.Modules.Inspector.Inspectors;

using Idealde.Modules.ProjectExplorer.Models;

using Newtonsoft.Json;

using PCLExt.FileStorage.Extensions;
using PCLExt.FileStorage.Files;
using PCLExt.FileStorage.Folders;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.ViewModels
{
    public partial class SolutionExplorerViewModel
        : ICommandHandler<ViewItemPropertiesFromProjectCommandDefinition>
        , ICommandHandler<OpenFolderInProjectCommandDefinition>
        , ICommandHandler<EditProjectFileFromProjectCommandDefinition>
        , ICommandHandler<CutItemFromProjectCommandDefinition>
        , ICommandHandler<CopyItemFromProjectCommandDefinition>
        , ICommandHandler<PasteItemFromProjectCommandDefinition>
        , ICommandHandler<ExcludeItemFromProjectCommandDefinition>
        , ICommandHandler<OpenDependencyFromProjectCommandDefinition>
    {
        void ICommandHandler<ViewItemPropertiesFromProjectCommandDefinition>.Update(Command command) { }
        Task ICommandHandler<ViewItemPropertiesFromProjectCommandDefinition>.Run(Command command)
        {
            // Really
            void BuildDependency(ProjectItem item, FactorioModificationDependency dependency)
            {
                var oldJsonRaw = dependency.JsonRaw();
                dependency.PropertyChanged += (s, e) =>
                {
                    item.Text = dependency.DisplayText;
                    item.Tag = dependency.JsonRaw();
                    if (e.PropertyName.Equals(nameof(dependency.State), StringComparison.OrdinalIgnoreCase))
                    {
                        var projectParent = item.GetItemProjectRoot();
                        if (projectParent.Tag is FactorioProjectInfo project)
                        {
                            FactorioModificationDependency dep = null;
                            if (dep == null) // Really
                            {
                                dep = project.Dependencies
                                .OfType<FactorioModificationDependency>()
                                .SingleOrDefault(fmd => fmd.JsonRaw().Equals(oldJsonRaw, StringComparison.InvariantCulture));
                            }

                            if (dep == null) // Really
                            {
                                dep = project.Dependencies
                                .OfType<FactorioModificationDependency>()
                                .SingleOrDefault(fmd => fmd == dependency);
                            }

                            project.Dependencies.Remove(dep);
                            project.Dependencies.Add(dependency);
                        }
                    }
                };

                InspectorTool.DisplayName = item.Text;
                var properties = TypeDescriptor.GetProperties(dependency);
                InspectorTool.SelectedObject = new InspectableObjectBuilder()
                        .WithEditor(dependency, d => d.Name, new TextBoxEditorViewModel<string>())
                        .WithEditor(dependency, d => d.State, new Inspectors.EnumEditorViewModel<FactorioModificationDependency.DependencyState>())
                        .WithEditor(dependency, d => d.Version, new VersionEditorViewModel())
                        .WithEditor(dependency, d => d.VersionRequirement, new FactrIDE.Gemini.Modules.SolutionExplorer.Inspectors.EnumEditorViewModel<FactorioModificationDependency.VersionState>())
                        .ToInspectableObject();
                Shell.ShowTool(InspectorTool);
            }

            if (command.Tag is ProjectItem item)
            {
                if (item.ProjectItemDefintion is FactorioProjectItemDefinition && item.Tag is FactorioProjectInfo project)
                {
                    var projectFile = project.Folder.GetFile("info.json");
                    var data = JsonConvert.DeserializeObject<FactorioProjectData>(projectFile.ReadAllText());
                    data.PropertyChanged += (s, e) =>
                    {
                        project.Data = data;

                        projectFile.WriteAllText(JsonConvert.SerializeObject(data, Formatting.Indented ));
                        item.Text = $"{data.Name}_{data.Version}";
                    };

                    InspectorTool.DisplayName = item.Text;
                    var properties = TypeDescriptor.GetProperties(data);
                    InspectorTool.SelectedObject = new InspectableObjectBuilder()
                        .WithEditor(data, d => d.Name, new TextBoxEditorViewModel<string>())
                        .WithEditor(data, d => d.Version, new VersionEditorViewModel())
                        .WithEditor(data, d => d.Title, new TextBoxEditorViewModel<string>())
                        .WithEditor(data, d => d.Description, new TextBoxEditorViewModel<string>())
                        .WithEditor(data, d => d.Author, new TextBoxEditorViewModel<string>())
                        .WithEditor(data, d => d.Contact, new TextBoxEditorViewModel<string>())
                        .WithEditor(data, d => d.Homepage, new TextBoxEditorViewModel<string>())
                        .WithEditor(data, d => d.FactorioVersion, new VersionEditorViewModel())
                        .ToInspectableObject();

                    Shell.ShowTool(InspectorTool);
                }
                else if (item.ProjectItemDefintion is FactorioDependencyProjectItemDefinition && item.Tag is string jsonRaw)
                {
                    var dependency = new FactorioModificationDependency(jsonRaw);
                    BuildDependency(item, dependency);
                }
            }

            return Task.CompletedTask;
        }

        void ICommandHandler<OpenFolderInProjectCommandDefinition>.Update(Command command) { }
        Task ICommandHandler<OpenFolderInProjectCommandDefinition>.Run(Command command)
        {
            if (command.Tag is ProjectItem item)
            {
                if (item.GetProjectItemFolder() is ProjectFolder parentFolder)
                {
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = parentFolder.Path,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }
            }

            return Task.CompletedTask;
        }

        void ICommandHandler<EditProjectFileFromProjectCommandDefinition>.Update(Command command) { }
        Task ICommandHandler<EditProjectFileFromProjectCommandDefinition>.Run(Command command)
        {
            if (command.Tag is ProjectItem item)
            {
                if (item.ProjectItemDefintion is FactorioProjectItemDefinition && item.Tag is FactorioProjectInfo project)
                {
                    Helper.OpenFileEditor(Path.Combine(project.Folder.Path, "info.json"));
                }
            }

            return Task.CompletedTask;
        }


        void ICommandHandler<CutItemFromProjectCommandDefinition>.Update(Command command)
        {
            command.Enabled = GetProjectItemFromCommand(command) is ProjectItem parent && (parent.ProjectItemDefintion is FileProjectItemDefinition || parent.ProjectItemDefintion is FolderProjectItemDefinition);
        }
        Task ICommandHandler<CutItemFromProjectCommandDefinition>.Run(Command command)
        {
            if (GetProjectItemFromCommand(command) is ProjectItem item)
            {
                if (item.Tag is ProjectFile file)
                {
                    new string[] { file.Path }.PutFilesOnClipboard(true);
                }
                else if (item.Tag is ProjectFolder folder)
                {
                    new string[] { folder.Path }.PutFilesOnClipboard(true);
                }
            }

            return Task.CompletedTask;
        }

        void ICommandHandler<CopyItemFromProjectCommandDefinition>.Update(Command command)
        {
            command.Enabled = GetProjectItemFromCommand(command) is ProjectItem parent && (parent.ProjectItemDefintion is FileProjectItemDefinition || parent.ProjectItemDefintion is FolderProjectItemDefinition);
        }
        Task ICommandHandler<CopyItemFromProjectCommandDefinition>.Run(Command command)
        {
            if (GetProjectItemFromCommand(command) is ProjectItem item)
            {
                if (item.Tag is ProjectFile file)
                {
                    new string[] { file.Path }.PutFilesOnClipboard();
                }
                else if (item.Tag is ProjectFolder folder)
                {
                    new string[] { folder.Path }.PutFilesOnClipboard();
                }
            }

            return Task.CompletedTask;
        }

        void ICommandHandler<PasteItemFromProjectCommandDefinition>.Update(Command command)
        {
            command.Enabled = GetProjectItemFromCommand(command) is ProjectItem parent && (parent.ProjectItemDefintion is FactorioProjectItemDefinition || parent.ProjectItemDefintion is FolderProjectItemDefinition);
        }
        async Task ICommandHandler<PasteItemFromProjectCommandDefinition>.Run(Command command)
        {
            if (GetProjectItemFromCommand(command) is ProjectItem parent)
            {
                var dropList = Clipboard.GetFileDropList()
                    .OfType<string>()
                    .Where(p => File.Exists(p) || Directory.Exists(p))
                    .Select(p => (Path: p, IsFile: (File.GetAttributes(p) & FileAttributes.Directory) == 0))
                    .ToList();

                if (parent.ProjectItemDefintion is FactorioProjectItemDefinition || parent.ProjectItemDefintion is FolderProjectItemDefinition)
                {
                    foreach (var drop in dropList)
                    {
                        if (drop.IsFile)
                            await AddExistingFile(parent, new FileFromPath(drop.Path)).ConfigureAwait(false);
                        else
                            await AddExistingFolder(parent, new FolderFromPath(drop.Path)).ConfigureAwait(false);
                    }
                }
            }
        }

        void ICommandHandler<ExcludeItemFromProjectCommandDefinition>.Update(Command command) { }
        Task ICommandHandler<ExcludeItemFromProjectCommandDefinition>.Run(Command command)
        {
            if (GetProjectItemFromCommand(command) is ProjectItem parent)
            {
                var projectParent = parent.GetItemProjectRoot();
                if (projectParent.Tag is FactorioProjectInfo project)
                {
                    if (parent.ProjectItemDefintion is FileProjectItemDefinition && parent.Tag is ProjectFile projectFile)
                        project.IgnoreFiles.Add(projectFile);
                    else if (parent.ProjectItemDefintion is FolderProjectItemDefinition && parent.Tag is ProjectFolder projectFolder)
                        project.IgnoreFolders.Add(projectFolder);
                }
            }

            return Task.CompletedTask;
        }

        void ICommandHandler<OpenDependencyFromProjectCommandDefinition>.Update(Command command) { }
        async Task ICommandHandler<OpenDependencyFromProjectCommandDefinition>.Run(Command command)
        {
            if (command.Tag is ProjectItem item)
            {
                if (item.ProjectItemDefintion is FactorioDependencyProjectItemDefinition && item.Tag is string jsonRaw)
                {
                    var dependency = new FactorioModificationDependency(jsonRaw);

                    var folder = new ModificationCacheFolder();

                    //if(folder.CheckExistsAsync())

                    FactorioAPI.UpdateToken();
                    var mod = FactorioAPI.GetModificationInfo(dependency.Name);
                    Release release = null;
                    switch (dependency.VersionRequirement)
                    {
                        case FactorioModificationDependency.VersionState.Exact:
                            release = mod.Releases
                                .FirstOrDefault(m => (Version.TryParse(m.Version, out var v) ? v : new Version()) == dependency.Version);
                            break;
                        case FactorioModificationDependency.VersionState.GreatherThan:
                            release = mod.Releases
                                .OrderByDescending(m => Version.TryParse(m.Version, out var v) ? v : new Version())
                                .FirstOrDefault(m => (Version.TryParse(m.Version, out var v) ? v : new Version()) > dependency.Version);
                            break;
                        case FactorioModificationDependency.VersionState.GreatherThanExact:
                            release = mod.Releases
                                .OrderByDescending(m => Version.TryParse(m.Version, out var v) ? v : new Version())
                                .FirstOrDefault(m => (Version.TryParse(m.Version, out var v) ? v : new Version()) >= dependency.Version);
                            break;
                        case FactorioModificationDependency.VersionState.LowerThan:
                            release = mod.Releases
                                .OrderBy(m => Version.TryParse(m.Version, out var v) ? v : new Version())
                                .FirstOrDefault(m => (Version.TryParse(m.Version, out var v) ? v : new Version()) < dependency.Version);
                            break;
                        case FactorioModificationDependency.VersionState.LowerThanExact:
                            release = mod.Releases
                                .OrderBy(m => Version.TryParse(m.Version, out var v) ? v : new Version())
                                .FirstOrDefault(m => (Version.TryParse(m.Version, out var v) ? v : new Version()) <= dependency.Version);
                            break;
                    }
                    if (release != null)
                    {
                        var modData = FactorioAPI.GetModification(release.DownloadUrl);
                        var file = await folder.CreateFileAsync(release.Filename, PCLExt.FileStorage.CreationCollisionOption.ReplaceExisting).ConfigureAwait(false);
                        await file.WriteAllBytesAsync(modData).ConfigureAwait(false);

                        Process.Start(new ProcessStartInfo()
                        {
                            FileName = file.Path,
                            UseShellExecute = true,
                            Verb = "open"
                        });
                    }
                }
            }
        }
    }
}