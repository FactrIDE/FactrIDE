using System;

using FactrIDE.Properties;

using Gemini.Framework.Commands;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandDefinition]
    public class NewLocalizationFileToProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Project.Add.NewLocalizationFile";

        public override string Name => CommandName;
        public override string Text => Resources.AddNewLocalizationFileToProjectText;
        public override string ToolTip => "";
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/NewFile_16x.png");
    }
}