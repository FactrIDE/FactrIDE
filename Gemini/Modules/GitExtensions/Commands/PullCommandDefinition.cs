using Gemini.Framework.Commands;

using System;

namespace FactrIDE.Gemini.Modules.GitExtensions.Commands
{
    [CommandDefinition]
    public sealed class PullCommandDefinition : CommandDefinition
    {
        public const string CommandName = "GitExtensions.Pull";

        public override string Name => CommandName;
        public override string Text => "Pull";//Resources.ViewFactorioStandardOutputCommandText;
        public override string ToolTip => "Pull";//Resources.ViewFactorioStandardOutputCommandToolTip;
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/GitUI/Pull.png");
    }
}