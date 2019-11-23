using FactrIDE.Gemini.Modules.FactorioLogOutput.Commands;

using Gemini.Framework.Menus;

using System.ComponentModel.Composition;

namespace FactrIDE.Gemini.Modules.FactorioLogOutput
{
    public static class MenuDefinitions
    {
        [Export]
        public static MenuItemDefinition ViewFactorioLogOutputMenuItem = new CommandMenuItemDefinition<ViewFactorioLogOutputCommandDefinition>(
            global::FactrIDE.Gemini.Modules.Factorio.MenuDefinitions.ViewOutputToolsMenuGroup, 1);
            //global::Gemini.Modules.MainMenu.MenuDefinitions.ViewToolsMenuGroup, 1);
    }
}