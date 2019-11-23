using FactrIDE.Gemini.Modules.SolutionExplorer.Extensions;

using PCLExt.FileStorage;
using PCLExt.FileStorage.Folders;

using System;
using System.Diagnostics;

namespace FactrIDE.Gemini.Modules.SolutionExplorer
{
    public class ProjectFolder : BaseFolder, IProjectEntity
    {
        public IFolder Project { get; }

        public string ProjectLocalPath
        {
            get
            {
                var folderPathUri = new Uri(Path.GetFullPathWithEndingSlashes(), UriKind.Absolute);
                var projectPathUri = new Uri(Project.Path.GetFullPathWithEndingSlashes(), UriKind.Absolute);
                return projectPathUri.MakeRelativeUri(folderPathUri).ToString();
            }
        }

        [DebuggerStepThrough]
        public ProjectFolder(IFolder projectFolder, string localPath) : base(new FolderFromPath(System.IO.Path.Combine(projectFolder.Path, localPath)))
        {
            Project = projectFolder;
        }
        [DebuggerStepThrough]
        public ProjectFolder(IFolder projectFolder, IFolder folder) : base(folder)
        {
            Project = projectFolder;
        }

        public override string ToString() => Path;
    }
}