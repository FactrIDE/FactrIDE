using System;

using FactrIDE.Properties;

using Gemini.Framework.Commands;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandDefinition]
    public class ExcludeItemFromProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Project.Exclude";

        public override string Name => CommandName;
        public override string Text => Resources.ExcludeItemFromProjectText;
        public override string ToolTip => "";
        //public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/Rename_16x.png");
    }
}