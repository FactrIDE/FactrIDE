using FactrIDE.Gemini.Modules.SolutionExplorer.Commands;
using FactrIDE.Properties;

using Gemini.Framework.Menus;

using System.ComponentModel.Composition;

namespace FactrIDE.Gemini.Modules.SolutionExplorer
{
    public static class MenuDefinitions
    {
        [Export]
        public static MenuItemDefinition ViewProjectExplorerMenuItem = new CommandMenuItemDefinition<ViewSolutionExplorerToolCommandDefinition>(
            global::Gemini.Modules.MainMenu.MenuDefinitions.ViewToolsMenuGroup, 1);

        //[Export]
        //public static MenuItemDefinition ViewProjectExplorerMenuIte1m = new CommandMenuItemDefinition<NewModificationCommandDefinition>(
        //    global::Gemini.Modules.Shell.MenuDefinitions.FileNewMenuItemList, 1);


        [Export]
        public static ExcludeMenuItemDefinition ExcludeFileNewMenuItemList = new ExcludeMenuItemDefinition(
            global::Gemini.Modules.Shell.MenuDefinitions.FileNewMenuItemList);

        [Export]
        public static ExcludeMenuItemDefinition ExcludeFileOpenMenuItem = new ExcludeMenuItemDefinition(
            global::Gemini.Modules.Shell.MenuDefinitions.FileOpenMenuItem);


        //[Export]
        //public static MenuItemDefinition FileNewMenuItemList0 = new CommandMenuItemDefinition<NewFactrIDESolutionMenuCommandDefinition>(
        //    global::Gemini.Modules.Shell.MenuDefinitions.FileNewCascadeGroup, 0);
        [Export]
        public static MenuItemDefinition FileNewMenuItemList1 = new CommandMenuItemDefinition<NewFactorioProjectMenuCommandDefinition>(
            global::Gemini.Modules.Shell.MenuDefinitions.FileNewCascadeGroup, 1);

        [Export]
        public static MenuItemDefinition FileOpenMenuItem = new TextMenuItemDefinition(
            global::Gemini.Modules.MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 1, Resources.FileOpenCommandText);
        [Export]
        public static MenuItemGroupDefinition FileOpenCascadeGroup = new MenuItemGroupDefinition(
            FileOpenMenuItem, 0);
        [Export]
        public static MenuItemDefinition OpennFactrIDESolutionMenuItem = new CommandMenuItemDefinition<OpenFactrIDESolutionMenuCommandDefinition>(
            FileOpenCascadeGroup, 0);
        [Export]
        public static MenuItemDefinition OpenFactorioProjectMenuItem = new CommandMenuItemDefinition<OpenFactorioProjectMenuCommandDefinition>(
            FileOpenCascadeGroup, 1);

        [Export]
        public static MenuItemDefinition FileRecentMenuItem = new TextMenuItemDefinition(
            global::Gemini.Modules.MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 2, Resources.FileRecentCommandText);
        [Export]
        public static MenuItemGroupDefinition FileRecentCascadeGroup = new MenuItemGroupDefinition(
            FileRecentMenuItem, 0);
        [Export]
        public static MenuItemDefinition RecentMenuItem = new CommandMenuItemDefinition<RecentMenuCommandDefinition>(
            FileRecentCascadeGroup, 0);


        [Export]
        public static MenuItemGroupDefinition EditRenameMenuGroup = new MenuItemGroupDefinition(
            global::Gemini.Modules.MainMenu.MenuDefinitions.EditMenu, 1);
        [Export]
        public static MenuItemDefinition RenameItemMenuItem = new CommandMenuItemDefinition<RenameItemFromProjectCommandDefinition>(
            EditRenameMenuGroup, 0);


        [Export]
        public static MenuItemGroupDefinition EditCopyCutPasteMenuGroup = new MenuItemGroupDefinition(
            global::Gemini.Modules.MainMenu.MenuDefinitions.EditMenu, 2);
        [Export]
        public static MenuItemDefinition CutItemMenuItem = new CommandMenuItemDefinition<CutItemFromProjectCommandDefinition>(
            EditCopyCutPasteMenuGroup, 0);
        [Export]
        public static MenuItemDefinition CopyItemMenuItem = new CommandMenuItemDefinition<CopyItemFromProjectCommandDefinition>(
            EditCopyCutPasteMenuGroup, 1);
        [Export]
        public static MenuItemDefinition PasteItemMenuItem = new CommandMenuItemDefinition<PasteItemFromProjectCommandDefinition>(
            EditCopyCutPasteMenuGroup, 2);
    }
}