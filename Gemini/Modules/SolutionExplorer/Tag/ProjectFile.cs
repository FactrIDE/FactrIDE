using FactrIDE.Gemini.Modules.SolutionExplorer.Extensions;

using PCLExt.FileStorage;
using PCLExt.FileStorage.Files;

using System;
using System.Diagnostics;

namespace FactrIDE.Gemini.Modules.SolutionExplorer
{
    public class ProjectFile : BaseFile, IProjectEntity
    {
        public IFolder Project { get; }

        public string ProjectLocalPath
        {
            get
            {
                var filePathUri = new Uri(Path, UriKind.Absolute);
                var projectPathUri = new Uri(Project.Path.GetFullPathWithEndingSlashes(), UriKind.Absolute);
                return projectPathUri.MakeRelativeUri(filePathUri).ToString();
            }
        }

        [DebuggerStepThrough]
        public ProjectFile(IFolder projectFolder, string localPath) : base(new FileFromPath(System.IO.Path.Combine(projectFolder.Path, localPath)))
        {
            Project = projectFolder;
        }
        [DebuggerStepThrough]
        public ProjectFile(IFolder projectFolder, IFile file) : base(file)
        {
            Project = projectFolder;
        }

        public override string ToString() => ProjectLocalPath;
    }
}