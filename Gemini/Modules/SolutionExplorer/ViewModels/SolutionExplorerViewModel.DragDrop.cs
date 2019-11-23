using FactrIDE.Gemini.Modules.SolutionExplorer.Extensions;
using FactrIDE.Gemini.Modules.SolutionExplorer.Models;

using GongSolutions.Wpf.DragDrop;

using Idealde.Modules.ProjectExplorer.Models;

using PCLExt.FileStorage.Files;
using PCLExt.FileStorage.Folders;

using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.ViewModels
{
    public partial class SolutionExplorerViewModel : IDropTarget
    {
        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.TargetItem is ProjectItem parent)
            {
                if (dropInfo.Data is DataObject dataObject && dataObject.ContainsFileDropList())
                {
                    var dropList = dataObject.GetFileDropList().OfType<string>().ToList();

                    if (parent.ProjectItemDefintion is FactorioProjectItemDefinition || parent.ProjectItemDefintion is FolderProjectItemDefinition)
                    {
                        dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                        dropInfo.Effects = DragDropEffects.Copy;
                    }
                    else if (parent.ProjectItemDefintion is FactorioDependencyRequiredListProjectItemDefinition
                          || parent.ProjectItemDefintion is FactorioDependencyOptionalListProjectItemDefinition
                          || parent.ProjectItemDefintion is FactorioDependencyNotCompatibleListProjectItemDefinition)
                    {
                        // Is archive list
                        if (dropList.All(p =>
                             Path.GetExtension(p).Equals(".zip", StringComparison.OrdinalIgnoreCase) &&
                             (File.GetAttributes(p) & FileAttributes.Directory) == 0))
                        {
                            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                            dropInfo.Effects = DragDropEffects.Copy;
                        }
                    }
                }

                if (dropInfo.Data is ProjectItem data)
                {
                    if(parent.Equals(data))
                    {
                        dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                        dropInfo.Effects = DragDropEffects.None;
                    }
                    else
                    {
                        if(data.ProjectItemDefintion is FileProjectItemDefinition || data.ProjectItemDefintion is FolderProjectItemDefinition)
                        {
                            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                            dropInfo.Effects = DragDropEffects.Copy;
                        }
                    }
                }
            }
        }

        async void IDropTarget.Drop(IDropInfo dropInfo)
        {
            if (dropInfo.TargetItem is ProjectItem parent)
            {
                if (dropInfo.Data is DataObject dataObject && dataObject.ContainsFileDropList())
                {
                    var dropList = dataObject.GetFileDropList()
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
                    // TODO
                    else if (parent.ProjectItemDefintion is FactorioDependencyRequiredListProjectItemDefinition
                          || parent.ProjectItemDefintion is FactorioDependencyOptionalListProjectItemDefinition
                          || parent.ProjectItemDefintion is FactorioDependencyNotCompatibleListProjectItemDefinition)
                    {
                        foreach (var drop in dropList)
                        {
                            if (drop.IsFile)
                                AddDependency(parent, new FileFromPath(drop.Path));
                            //else
                            //    ; // WUT
                        }
                    }
                }
                if (dropInfo.Data is ProjectItem data)
                {
                    if (parent.GetProjectItemFolder() is ProjectFolder parentFolder)
                    {
                        if (data.ProjectItemDefintion is FileProjectItemDefinition && data.Tag is ProjectFile file)
                            await AddExistingFile(parent, file, true).ConfigureAwait(false);
                        if (data.ProjectItemDefintion is FolderProjectItemDefinition && data.Tag is ProjectFolder folder)
                            await AddExistingFolder(parent, folder, true).ConfigureAwait(false);
                    }
                }
            }
        }
    }
}