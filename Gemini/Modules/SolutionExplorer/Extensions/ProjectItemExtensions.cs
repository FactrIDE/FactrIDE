using FactrIDE.Gemini.Modules.SolutionExplorer.Models;
using FactrIDE.Gemini.Modules.SolutionExplorer.Providers;

using Idealde.Modules.ProjectExplorer.Models;

using System;
using System.Linq;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Extensions
{
    public static class ProjectItemExtensions
    {
        public static bool ProjectItemExistInChindren(this ProjectItem parent, string projectItemText) =>
            parent.Children.Any(p => p.Text.Equals(projectItemText, StringComparison.InvariantCulture));

        public static ProjectItem GetItemProjectRoot(this ProjectItem item)
        {
            var projectRoot = item;
            while (!(projectRoot.ProjectItemDefintion is FactorioProjectItemDefinition))
                projectRoot = (ProjectItem)projectRoot.Parent;
            return projectRoot;
        }

        /// <summary>
        /// If this ProjectItem represents a folder - either a solution, project or an actual folder, return it.
        /// </summary>
        public static ProjectFolder GetProjectItemFolder(this ProjectItemBase item) => item.Tag switch
        {
            ProjectFolder f => f,

            FactorioProjectInfo p => new ProjectFolder(p.Folder, p.Folder),

            FactrIDESolutionInfo s => new ProjectFolder(s.Folder, s.Folder),

            _ => null,
        };
    }
}