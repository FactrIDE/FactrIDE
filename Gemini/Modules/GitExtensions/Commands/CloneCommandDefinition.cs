using Gemini.Framework.Commands;

using System;

namespace FactrIDE.Gemini.Modules.GitExtensions.Commands
{
    [CommandDefinition]
    public sealed class CloneCommandDefinition : CommandDefinition
    {
        public const string CommandName = "GitExtensions.Clone";

        public override string Name => CommandName;
        public override string Text => "Clone";//Resources.ViewFactorioStandardOutputCommandText;
        public override string ToolTip => "Clone Git Repository";//Resources.ViewFactorioStandardOutputCommandToolTip;
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/GitUI/CloneRepoGit.png");
    }
}