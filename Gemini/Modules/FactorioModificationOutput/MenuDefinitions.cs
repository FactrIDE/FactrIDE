using FactrIDE.Gemini.Modules.FactorioModificationOutput.Commands;

using Gemini.Framework.Menus;

using System.ComponentModel.Composition;

namespace FactrIDE.Gemini.Modules.FactorioModificationOutput
{
    public static class MenuDefinitions
    {
        [Export]
        public static MenuItemDefinition ViewFactorioModificationOutputMenuItem = new CommandMenuItemDefinition<ViewFactorioModOutputCommandDefinition>(
            global::FactrIDE.Gemini.Modules.Factorio.MenuDefinitions.ViewOutputToolsMenuGroup, 2);
            //global::Gemini.Modules.MainMenu.MenuDefinitions.ViewToolsMenuGroup, 1);
    }
}