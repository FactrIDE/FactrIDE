using Gemini.Framework.Commands;

using System;

namespace FactrIDE.Gemini.Modules.GitExtensions.Commands
{
    [CommandDefinition]
    public sealed class PushCommandDefinition : CommandDefinition
    {
        public const string CommandName = "GitExtensions.Push";

        public override string Name => CommandName;
        public override string Text => "Push";//Resources.ViewFactorioStandardOutputCommandText;
        public override string ToolTip => "Push";//Resources.ViewFactorioStandardOutputCommandToolTip;
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/GitUI/Push.png");
    }
}