using System.Collections.Generic;
using System.Threading.Tasks;

using FactrIDE.Gemini.Modules.SolutionExplorer.Providers;

using Idealde.Framework.Projects;

using PCLExt.FileStorage;

namespace Idealde.Framework.ProjectExplorer.Models
{
    public interface IProjectProvider
    {
        string Name { get; }

        IEnumerable<ExtensionType> ExtensionTypes { get; }

        Task<IFile> Create(ProjectDataBase projectData, IFolder projectFolder);
        ProjectInfoBase Load(IFile projectFile);
        Task<ProjectInfoBase> LoadAsync(IFile projectFile);
        Task<IFile> Save(ProjectInfoBase info, IFolder projectFolder);
    }
}