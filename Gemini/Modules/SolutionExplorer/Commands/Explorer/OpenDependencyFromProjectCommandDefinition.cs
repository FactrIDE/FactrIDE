using System;

using FactrIDE.Properties;

using Gemini.Framework.Commands;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandDefinition]
    public class OpenDependencyFromProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Project.Dependencies.Open";

        public override string Name => CommandName;
        public override string Text => Resources.OpenDependencyCommandText;
        public override string ToolTip => "";
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/Property_16x.png");
    }
}