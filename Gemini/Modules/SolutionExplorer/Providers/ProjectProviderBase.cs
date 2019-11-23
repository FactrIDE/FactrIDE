using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using FactrIDE.Gemini.Modules.SolutionExplorer.Providers;

using Idealde.Framework.Projects;

using PCLExt.FileStorage;

namespace Idealde.Framework.ProjectExplorer.Models
{
    public abstract class ProjectProviderBase : IProjectProvider
    {
        public abstract string Name { get; }

        public abstract IEnumerable<ExtensionType> ExtensionTypes { get; }

        public abstract Task<IFile> Create(ProjectDataBase projectData, IFolder projectFolder);
        public abstract ProjectInfoBase Load(IFile projectFile);
        public abstract Task<ProjectInfoBase> LoadAsync(IFile projectFile);
        public abstract Task<IFile> Save(ProjectInfoBase info, IFolder projectFolder);

        [DebuggerStepThrough]
        protected bool ValidateExtension(IFile file)
        {
            var extension = Path.GetExtension(file.Name);
            foreach (var projectType in ExtensionTypes)
            {
                if (string.Equals(projectType.Extension, extension, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}