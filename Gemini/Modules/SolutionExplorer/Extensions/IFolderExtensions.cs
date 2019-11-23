using PCLExt.FileStorage;

using System;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Extensions
{
    public static class IFolderExtensions
    {
        public static string GetProjectPath(this IFolder solutionFolder, IFolder projectFolder)
        {
            var projectFolderPathUri = new Uri(projectFolder.Path.GetFullPathWithEndingSlashes(), UriKind.Absolute);
            var solutionFolderPathUri = new Uri(solutionFolder.Path.GetFullPathWithEndingSlashes(), UriKind.Absolute);

            try { return solutionFolderPathUri.MakeRelativeUri(projectFolderPathUri).ToString(); }
            catch (Exception e) when (e is InvalidOperationException) { return projectFolder.Path; }
        }
    }
}