using Gemini.Framework.Commands;

using System;

namespace FactrIDE.Gemini.Modules.GitExtensions.Commands
{
    [CommandDefinition]
    public sealed class BrowseCommandDefinition : CommandDefinition
    {
        public const string CommandName = "GitExtensions.Browse";

        public override string Name => CommandName;
        public override string Text => "Browse";//Resources.ViewFactorioStandardOutputCommandText;
        public override string ToolTip => "Browse";//Resources.ViewFactorioStandardOutputCommandToolTip;
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/GitUI/BrowseFileExplorer.png");
    }
}