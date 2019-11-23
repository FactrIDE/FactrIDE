using System;

using FactrIDE.Properties;

using Gemini.Framework.Commands;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandDefinition]
    public class OpenFactrIDESolutionMenuCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.Open.FactrIDESolution";

        public override string Name => CommandName;
        public override string Text => Resources.FactrIDESolutionText;
        public override string ToolTip => Resources.FactrIDESolutionToolTip;
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/FactrIDESolution_16x.png");
    }
}