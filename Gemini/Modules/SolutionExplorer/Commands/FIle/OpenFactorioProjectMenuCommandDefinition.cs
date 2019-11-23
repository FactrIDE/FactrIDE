using System;

using FactrIDE.Properties;

using Gemini.Framework.Commands;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandDefinition]
    public class OpenFactorioProjectMenuCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.Open.FactorioModification";

        public override string Name => CommandName;
        public override string Text => Resources.FactorioModificationText;
        public override string ToolTip => Resources.FactorioModificationToolTip;
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/FactorioProject_16x.png");
    }
}