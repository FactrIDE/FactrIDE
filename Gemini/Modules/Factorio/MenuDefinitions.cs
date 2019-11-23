using Gemini.Framework.Menus;

using System.ComponentModel.Composition;

namespace FactrIDE.Gemini.Modules.Factorio
{
    public static class MenuDefinitions
    {
        [Export]
        public static MenuItemGroupDefinition ViewOutputToolsMenuGroup =
            new MenuItemGroupDefinition(global::Gemini.Modules.MainMenu.MenuDefinitions.ViewMenu, 0);
    }
}