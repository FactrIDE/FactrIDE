using FactrIDE.Properties;

using Gemini.Framework.Commands;

using System;

namespace FactrIDE.Gemini.Modules.FactorioLogOutput.Commands
{
    [CommandDefinition]
    public class ViewFactorioLogOutputCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View.FactorioLogOutput";

        public override string Name => CommandName;
        public override string Text => Resources.ViewFactorioLogOutputCommandText;
        public override string ToolTip => Resources.ViewFactorioLogOutputCommandToolTip;
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/Log_16x.png");
    }
}