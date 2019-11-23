using Gemini.Framework.Commands;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandDefinition]
    public class RecentMenuCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.Recent";

        public override string Name => CommandName;
        public override string Text => "...";
        public override string ToolTip => "";
        //public override Uri IconSource => new Uri("pack://application:,,,/Resources/Factorio/Icon.png");
    }
}