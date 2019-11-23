using Gemini.Framework.Menus;

using System.ComponentModel.Composition;

namespace FactrIDE.Gemini.Modules.Startup
{
    public static class MenuDefinitions
    {
        [Export]
        public static ExcludeMenuItemDefinition ExcludeHistoryMenuItem =
            new ExcludeMenuItemDefinition(global::Gemini.Modules.UndoRedo.MenuDefinitions.ViewHistoryMenuItem);

        [Export]
        public static ExcludeMenuItemDefinition ExcludeOpenMenuItem =
            new ExcludeMenuItemDefinition(global::Gemini.Modules.Output.MenuDefinitions.ViewOutputMenuItem);

        [Export]
        public static ExcludeMenuItemDefinition ExcludeToolboxMenuItem =
            new ExcludeMenuItemDefinition(global::Gemini.Modules.Toolbox.MenuDefinitions.ViewToolboxMenuItem);

        [Export]
        public static ExcludeMenuItemDefinition ExcludeInspectorMenuItem =
            new ExcludeMenuItemDefinition(global::Gemini.Modules.Inspector.MenuDefinitions.ViewInspectorMenuItem);

        //[Export]
        //public static ExcludeMenuItemDefinition ExcludePropertyGridMenuItem =
        //    new ExcludeMenuItemDefinition(global::Gemini.Modules.PropertyGrid.MenuDefinitions.ViewPropertyGridMenuItem);

        [Export]
        public static ExcludeMenuItemGroupDefinition ExcludePropertiesMenuGroup =
            new ExcludeMenuItemGroupDefinition(global::Gemini.Modules.MainMenu.MenuDefinitions.ViewPropertiesMenuGroup);
        [Export]
        public static ExcludeMenuItemDefinition ExcludeFullscreenMenuItem =
            new ExcludeMenuItemDefinition(global::Gemini.Modules.Shell.MenuDefinitions.ViewFullscreenItem);
    }
}