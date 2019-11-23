using FactrIDE.Gemini.Modules.Factorio.Commands;

using Gemini.Framework.ToolBars;

using System.ComponentModel.Composition;

namespace FactrIDE.Gemini.Modules.Factorio
{
    public static class ToolBarDefinitions
    {
        [Export]
        public static ToolBarDefinition FactorioToolBar = new ToolBarDefinition(8, "Factorio");

        [Export]
        public static ToolBarItemGroupDefinition FactorioToolBarGroup = new ToolBarItemGroupDefinition(
            FactorioToolBar, 8);

        [Export]
        public static ToolBarItemDefinition RunFactorioViewPanelToolBarItem = new CommandToolBarItemDefinition<RunFactorioCommandDefinition>(
            FactorioToolBarGroup, 0, ToolBarItemDisplay.IconAndText);

        [Export]
        public static ToolBarItemDefinition StopFactorioViewPanelToolBarItem = new CommandToolBarItemDefinition<StopFactorioCommandDefinition>(
            FactorioToolBarGroup, 1);
    }
}