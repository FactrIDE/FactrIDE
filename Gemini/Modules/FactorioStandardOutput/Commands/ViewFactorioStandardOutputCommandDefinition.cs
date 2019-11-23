using FactrIDE.Properties;

using Gemini.Framework.Commands;

using System;

namespace FactrIDE.Gemini.Modules.FactorioStandardOutput.Commands
{
    [CommandDefinition]
    public class ViewFactorioStandardOutputCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View.FactorioStandardOutput";

        public override string Name => CommandName;
        public override string Text => Resources.ViewFactorioStandardOutputCommandText;
        public override string ToolTip => Resources.ViewFactorioStandardOutputCommandToolTip;
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/Output_16x.png");
    }
}