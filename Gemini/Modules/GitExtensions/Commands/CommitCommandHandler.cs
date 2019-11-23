using FactrIDE.Gemini.Modules.GitExtensions.Git;
using FactrIDE.Gemini.Modules.SolutionExplorer;

using Gemini.Framework.Commands;
using Gemini.Framework.Services;

using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.GitExtensions.Commands
{
    [CommandHandler]
    public class CommitCommandHandler : Git1CommandHandlerBase<CommitCommandDefinition>
    {
        private static DateTime lastBranchCheck;
        private static string lastFile;
        private static string _lastUpdatedCaption;

        [ImportingConstructor]
        public CommitCommandHandler(IShell shell, ISolutionExplorer solutionExplorer) : base(shell, solutionExplorer) { }

        public override void Update(Command command)
        {
            var fileName = GetFileName(SolutionExplorer);
            if (!string.Equals(fileName, lastFile, StringComparison.InvariantCulture) || DateTime.Now - lastBranchCheck > TimeSpan.FromSeconds(2))
            {
                var newCaption = "Commit";
                if (true)
                {
                    var showCurrentBranch = GitCommands.GetShowCurrentBranchSetting();
                    if (showCurrentBranch && !string.IsNullOrEmpty(fileName))
                    {
                        var head = GitCommands.GetCurrentBranch(fileName);
                        if (!string.IsNullOrEmpty(head))
                        {
                            var headShort = head.Length > 27 ? $"...{head.Substring(head.Length - 23)}" : head;
                            newCaption = $"Commit ({headShort})";
                        }
                    }

                    lastBranchCheck = DateTime.Now;
                    lastFile = fileName;
                }

                // This guard required not only for performance, but also for prevent StackOverflowException.
                // IDE.QueryStatus -> Commit.IsEnabled -> Plugin.UpdateCaption -> IDE.QueryStatus ...
                if (!string.Equals(_lastUpdatedCaption, newCaption, StringComparison.InvariantCulture))
                {
                    _lastUpdatedCaption = newCaption;

                    command.Text = newCaption;
                    command.ToolTip = "Commit changes";
                }
            }

            base.Update(command);
        }

        public override Task Run(string filePath, string projectPath)
        {
            //Save all
            RunGitEx("commit", projectPath);
            return Task.CompletedTask;
        }
    }
}