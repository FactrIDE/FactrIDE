using Gemini.Framework.Commands;

using System;

namespace FactrIDE.Gemini.Modules.GitExtensions.Commands
{
    [CommandDefinition]
    public sealed class CommitCommandDefinition : CommandDefinition
    {
        public const string CommandName = "GitExtensions.Commit";

        public override string Name => CommandName;
        public override string Text => "Commit";//Resources.ViewFactorioStandardOutputCommandText;
        public override string ToolTip => "Commit";//Resources.ViewFactorioStandardOutputCommandToolTip;
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/GitUI/RepoStateClean.png");
    }
}