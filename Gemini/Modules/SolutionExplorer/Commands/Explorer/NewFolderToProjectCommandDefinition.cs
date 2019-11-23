using System;

using FactrIDE.Properties;

using Gemini.Framework.Commands;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandDefinition]
    public class NewFolderToProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Project.Add.NewFolder";

        public override string Name => CommandName;
        public override string Text => Resources.AddNewFolderToProjectText;
        public override string ToolTip => "";
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/NewSolutionFolder_16x.png");
    }
}