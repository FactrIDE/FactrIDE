using FactrIDE.Gemini.Modules.SolutionExplorer.Extensions;

using Idealde.Framework.Projects;

using PCLExt.FileStorage;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Providers
{
    public abstract class SolutionInfoBase
    {
        private bool hasChanges;

        public virtual string Name { get; set; }
        public IFolder Folder { get; set; }
        protected IFile File { get; set; }

        public ISolutionProvider Provider { get; }

        public IList<ProjectInfoBase> Projects { get; } = new List<ProjectInfoBase>();

        public bool HasChanges
        {
            get { return hasChanges || Projects.Any(p => p.HasChanges); }
            protected set { hasChanges = value; }
        }

        protected SolutionInfoBase(IFolder solutionFolder, ISolutionProvider provider)
        {
            Folder = solutionFolder;
            Provider = provider;
        }

        public virtual async Task Create()
        {
            var data = new SolutionDataBase()
            {
                Name = Name,
                Projects = Projects.Select(p => new ProjectData() { Name = p.Name, Path = Folder.GetProjectPath(p.Folder) }).ToList()
            };
            await Provider.Create(data, Folder).ConfigureAwait(false);
        }

        public virtual async Task Save()
        {
            foreach (var projectInfo in Projects)
                await projectInfo.Save().ConfigureAwait(false);

            await Provider.Save(this, Folder).ConfigureAwait(false);
            HasChanges = false;
        }
    }
}