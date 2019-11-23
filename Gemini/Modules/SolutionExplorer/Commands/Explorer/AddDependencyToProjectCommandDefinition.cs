using System;

using FactrIDE.Properties;

using Gemini.Framework.Commands;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandDefinition]
    public class AddDependencyToProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Project.Add.Dependency";

        public override string Name => CommandName;
        public override string Text => Resources.AddDependencyToProjectText;
        public override string ToolTip => "";
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Factorio/Icon.png");
    }
}