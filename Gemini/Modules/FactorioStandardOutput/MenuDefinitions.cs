using FactrIDE.Gemini.Modules.FactorioStandardOutput.Commands;

using Gemini.Framework.Menus;

using System.ComponentModel.Composition;

namespace FactrIDE.Gemini.Modules.FactorioStandardOutput
{
    public static class MenuDefinitions
    {
        [Export]
        public static MenuItemDefinition ViewFactorioStandardOutputMenuItem = new CommandMenuItemDefinition<ViewFactorioStandardOutputCommandDefinition>(
            global::FactrIDE.Gemini.Modules.Factorio.MenuDefinitions.ViewOutputToolsMenuGroup, 0);
            //global::Gemini.Modules.MainMenu.MenuDefinitions.ViewToolsMenuGroup, 1);
    }
}