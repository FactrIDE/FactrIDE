using Gemini.Framework.Commands;

using System;

namespace FactrIDE.Gemini.Modules.GitExtensions.Commands
{
    [CommandDefinition]
    public sealed class CreateCommandDefinition : CommandDefinition
    {
        public const string CommandName = "GitExtensions.Create";

        public override string Name => CommandName;
        public override string Text => "Create";//Resources.ViewFactorioStandardOutputCommandText;
        public override string ToolTip => "Create Git Repository";//Resources.ViewFactorioStandardOutputCommandToolTip;
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/GitUI/star.png");
    }
}