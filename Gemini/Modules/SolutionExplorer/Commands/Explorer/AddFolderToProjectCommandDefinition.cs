using System;

using FactrIDE.Properties;

using Gemini.Framework.Commands;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandDefinition]
    public class AddFolderToProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Project.Add.ExistingFolder";

        public override string Name => CommandName;
        public override string Text => Resources.AddExistingFolderToProjectText;
        public override string ToolTip => "";
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/AddFolder_16x.png");
    }
}