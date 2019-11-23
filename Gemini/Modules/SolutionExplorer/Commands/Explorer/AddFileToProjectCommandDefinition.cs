using System;

using FactrIDE.Properties;

using Gemini.Framework.Commands;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandDefinition]
    public class AddFileToProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Project.Add.ExistingFile";

        public override string Name => CommandName;
        public override string Text => Resources.AddExistingFileToProjectText;
        public override string ToolTip => "";
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/AddFile_16x.png");
    }
}