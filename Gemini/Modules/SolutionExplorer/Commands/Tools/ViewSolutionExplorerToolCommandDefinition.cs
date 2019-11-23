using FactrIDE.Properties;

using Gemini.Framework.Commands;

using System;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandDefinition]
    public class ViewSolutionExplorerToolCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View.SolutionExplorer";

        public override string Name => CommandName;
        public override string ToolTip => Resources.ViewProjectExplorerCommandToolTip;
        public override string Text => Resources.ViewProjectExplorerCommadText;
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/Application_16x.png");
    }
}