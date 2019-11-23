using FactrIDE.Properties;

using Gemini.Framework.Commands;

using System;

namespace FactrIDE.Gemini.Modules.FactorioModificationOutput.Commands
{
    [CommandDefinition]
    public class ViewFactorioModOutputCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View.FactorioModOutput";

        public override string Name => CommandName;
        public override string Text => Resources.ViewFactorioModOutputCommandText;
        public override string ToolTip => Resources.ViewFactorioModOutputCommandToolTip;
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/Log_16x.png");
    }
}