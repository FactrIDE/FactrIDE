using Caliburn.Micro;

using FactrIDE.Gemini.Modules.SolutionExplorer;
using FactrIDE.Gemini.Modules.SolutionExplorer.Providers;

using Idealde.Framework.ProjectExplorer.Models;

using PCLExt.FileStorage;

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Idealde.Framework.Projects
{
    public abstract class ProjectInfoBase : PropertyChangedBase
    {
        private string name;
        private Version version;

        public virtual event System.Action OnProjectChange;

        public virtual string Name
        {
            get { return name; }
            set { name = value; NotifyOfPropertyChange(() => Name); }
        }
        public virtual Version Version
        {
            get { return version; }
            set { version = value; NotifyOfPropertyChange(() => Version); }
        }
        public IFolder Folder { get; set; }
        protected IFile File { get; set; }

        protected IProjectProvider Provider { get; }

        public virtual ObservableCollection<ProjectFile> Files { get; } = new ObservableCollection<ProjectFile>();
        public virtual ObservableCollection<ProjectFolder> Folders { get; } = new ObservableCollection<ProjectFolder>();
        public virtual ObservableCollection<ProjectFile> IgnoreFiles { get; } = new ObservableCollection<ProjectFile>();
        public virtual ObservableCollection<ProjectFolder> IgnoreFolders { get; } = new ObservableCollection<ProjectFolder>();
        public virtual ObservableCollection<DependencyBase> Dependencies { get; } = new ObservableCollection<DependencyBase>();

        public bool HasChanges { get; protected set; }

        protected ProjectInfoBase(IFolder projectFolder, IProjectProvider provider)
        {
            Folder = projectFolder;
            Provider = provider;

            Files.CollectionChanged += CollectionChanged;
            Folders.CollectionChanged += CollectionChanged;
            IgnoreFiles.CollectionChanged += CollectionChanged;
            IgnoreFolders.CollectionChanged += CollectionChanged;
            Dependencies.CollectionChanged += CollectionChanged;
        }

        protected virtual void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            HasChanges = true;
        }

        public virtual async Task Create()
        {
            await Provider.Create(new ProjectDataBase() { Name = Name, Version = Version }, Folder).ConfigureAwait(false);
        }

        public virtual async Task Save()
        {
            File = await Provider.Save(this, Folder).ConfigureAwait(false);
            HasChanges = false;
        }
        public virtual async Task Load()
        {
            await Task.CompletedTask.ConfigureAwait(false); // Really, warning?
            //this = await Provider.Load(File);
        }
    }
}