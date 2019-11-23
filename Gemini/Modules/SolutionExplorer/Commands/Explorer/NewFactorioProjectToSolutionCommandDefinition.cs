using System;

using FactrIDE.Properties;

using Gemini.Framework.Commands;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandDefinition]
    public class NewFactorioProjectToSolutionCommandDefinition : CommandDefinition
    {
        public const string CommandName = "SolutionExplorer.Add.NewFactorioProject";

        public override string Name => CommandName;
        public override string Text => Resources.AddNewFactorioProjectToSolutionExplorerText;
        public override string ToolTip => "";
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/FactorioProject_16x.png");
    }
}