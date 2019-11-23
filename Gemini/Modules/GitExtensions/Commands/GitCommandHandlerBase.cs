using System.Threading.Tasks;

using FactrIDE.Gemini.Modules.GitExtensions.Git;
using FactrIDE.Gemini.Modules.SolutionExplorer;
using FactrIDE.Gemini.Modules.SolutionExplorer.Providers;

using Gemini.Framework.Commands;
using Gemini.Framework.Services;

using Idealde.Framework.Projects;

namespace FactrIDE.Gemini.Modules.GitExtensions.Commands
{
    public abstract class Git1CommandHandlerBase<TCommandDefinition> : GitCommandHandlerBase<TCommandDefinition> where TCommandDefinition : CommandDefinition
    {
        protected Git1CommandHandlerBase(IShell shell, ISolutionExplorer solutionExplorer) : base(shell, solutionExplorer) { }

        public override void Update(Command command)
        {
            base.Update(command);

            if (SolutionExplorer?.SelectedProject?.Folder != null)
                command.Enabled = SolutionExplorer.SelectedProject.Folder.CheckExists(".git") == PCLExt.FileStorage.ExistenceCheckResult.FolderExists;
        }
    }

    public abstract class Git2CommandHandlerBase<TCommandDefinition> : GitCommandHandlerBase<TCommandDefinition> where TCommandDefinition : CommandDefinition
    {
        protected Git2CommandHandlerBase(IShell shell, ISolutionExplorer solutionExplorer) : base(shell, solutionExplorer) { }

        public override void Update(Command command)
        {
            base.Update(command);

            if(SolutionExplorer?.SelectedProject?.Folder != null)
                command.Enabled = SolutionExplorer.SelectedProject.Folder.CheckExists(".git") != PCLExt.FileStorage.ExistenceCheckResult.FolderExists;
        }
    }

    public abstract class GitCommandHandlerBase<TCommandDefinition> : CommandHandlerBase<TCommandDefinition> where TCommandDefinition : CommandDefinition
    {
        protected static void RunGitEx(string command, string filename, string[] arguments = null)
        {
            GitCommands.RunGitEx(command, filename, arguments);
        }

        protected static string GetFileName(ISolutionExplorer solutionExplorer) => (solutionExplorer.SelectedItem?.Tag) switch
        {
            SolutionInfoBase solution => solution.Folder.Path,
            ProjectInfoBase project => project.Folder.Path,
            ProjectFile file => file.Path,
            ProjectFolder folder => folder.Path,
            _ => string.Empty,
        };

        protected IShell Shell { get; }
        protected ISolutionExplorer SolutionExplorer { get; }

        protected GitCommandHandlerBase(IShell shell, ISolutionExplorer solutionExplorer)
        {
            Shell = shell;
            SolutionExplorer = solutionExplorer;
        }

        public override void Update(Command command)
        {
            command.Enabled = SolutionExplorer.SelectedItem != null;

            base.Update(command);
        }

        public override async Task Run(Command command)
        {
            var fileName = GetFileName(SolutionExplorer);
            if (!string.IsNullOrEmpty(fileName))
                await Run(fileName, SolutionExplorer.SelectedProject.Folder.Path).ConfigureAwait(false);
        }

        public virtual Task Run(string filePath, string projectPath) => Task.CompletedTask;
    }
}