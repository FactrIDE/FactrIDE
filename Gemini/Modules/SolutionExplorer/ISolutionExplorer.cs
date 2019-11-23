using Caliburn.Micro;

using FactrIDE.Gemini.Modules.SolutionExplorer.Providers;

using Gemini.Framework;

using Idealde.Framework.Projects;
using Idealde.Modules.ProjectExplorer.Models;

namespace FactrIDE.Gemini.Modules.SolutionExplorer
{
    public interface ISolutionExplorer : ITool
    {
        SolutionInfoBase SolutionInfo { get; set; }
        ProjectInfoBase SelectedProject { get; }

        IObservableCollection<ProjectItemBase> ItemTree { get; }
        ProjectItemBase SelectedItem { get; }

        void Load(SolutionInfoBase solutionInfo);
        void Load(ProjectInfoBase projectInfo);

        void Run();

        void Close();
    }
}