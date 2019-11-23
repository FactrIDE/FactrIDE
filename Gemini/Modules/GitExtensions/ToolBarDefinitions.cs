using FactrIDE.Gemini.Modules.GitExtensions.Commands;

using Gemini.Framework.ToolBars;

using System.ComponentModel.Composition;

namespace FactrIDE.Gemini.Modules.GitExtensions
{
    public static class ToolBarDefinitions
    {
        [Export]
        public static ToolBarDefinition GitExtensionsToolBar = new ToolBarDefinition(9, "GitExtensions");

        [Export]
        public static ToolBarItemGroupDefinition GitExtensionsToolBarGroup = new ToolBarItemGroupDefinition(
            GitExtensionsToolBar, 0);
        [Export]
        public static ToolBarItemGroupDefinition GitExtensions2ToolBarGroup = new ToolBarItemGroupDefinition(
            GitExtensionsToolBar, 1);

        [Export]
        public static ToolBarItemDefinition CreateViewPanelToolBarItem = new CommandToolBarItemDefinition<CreateCommandDefinition>(
            GitExtensionsToolBarGroup, 0);
        [Export]
        public static ToolBarItemDefinition CloneViewPanelToolBarItem = new CommandToolBarItemDefinition<CloneCommandDefinition>(
            GitExtensionsToolBarGroup, 1);
        [Export]
        public static ToolBarItemDefinition BrowseViewPanelToolBarItem = new CommandToolBarItemDefinition<BrowseCommandDefinition>(
            GitExtensionsToolBarGroup, 2);

        [Export]
        public static ToolBarItemDefinition PullViewPanelToolBarItem = new CommandToolBarItemDefinition<PullCommandDefinition>(
            GitExtensions2ToolBarGroup, 0);
        [Export]
        public static ToolBarItemDefinition PushViewPanelToolBarItem = new CommandToolBarItemDefinition<PushCommandDefinition>(
            GitExtensions2ToolBarGroup, 1);
        [Export]
        public static ToolBarItemDefinition CommitViewPanelToolBarItem = new CommandToolBarItemDefinition<CommitCommandDefinition>(
            GitExtensions2ToolBarGroup, 2, ToolBarItemDisplay.IconAndText);
    }
}