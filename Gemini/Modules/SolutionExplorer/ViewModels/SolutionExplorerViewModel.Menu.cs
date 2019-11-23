using Caliburn.Micro;

using Gemini.Modules.MainMenu.Models;

using Idealde.Framework.Commands;
using Idealde.Modules.MainMenu.Models;
using Idealde.Modules.ProjectExplorer.Models;

using System;
using System.Linq;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.ViewModels
{
    public partial class SolutionExplorerViewModel
    {
        public IObservableCollection<MenuItemBase> MenuItems { get; } = new BindableCollection<MenuItemBase>();

        public void PopulateMenu(ProjectItemBase item)
        {
            if (item == null) return;

            MenuItems.Clear();
            MenuItemBase parentMenuItem = null;

            foreach (var command in item.OptionCommands)
            {
                MenuItemBase newMenuItem = null;

                // fake command definition
                if (command.CommandDefinition is FakeCommandDefinition)
                {
                    // reset parent
                    if (string.IsNullOrEmpty(command.CommandDefinition.Name))
                        parentMenuItem = null;
                    // fake to add separator
                    else if (command.CommandDefinition.Name == "|")
                        newMenuItem = new MenuItemSeparator();
                    // fake to set parent
                    else if (command.CommandDefinition.Name.Contains('|'))
                    {
                        // extract ancestors
                        var parentNames = command.CommandDefinition.Name.Trim().Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                        // reset parent
                        if (parentNames.Length == 0)
                            parentMenuItem = null;
                        // find parent
                        else
                        {
                            parentMenuItem = MenuItems.OfType<StandardMenuItem>().FirstOrDefault(p => p.Text == parentNames[0]);
                            if (parentMenuItem == null)
                            {
                                parentMenuItem = new DisplayMenuItem(parentNames[0]);
                                MenuItems.Add(parentMenuItem);
                            }

                            for (var i = 1; i < parentNames.Length; i++)
                            {
                                var nextMenuItem = parentMenuItem.Children.OfType<StandardMenuItem>().FirstOrDefault(p => p.Text == parentNames[i]);
                                if (nextMenuItem == null)
                                {
                                    nextMenuItem = new DisplayMenuItem(parentNames[i]);
                                    parentMenuItem.Children.Add(nextMenuItem);
                                }
                                parentMenuItem = nextMenuItem;
                            }
                        }
                    }
                }
                else
                {
                    newMenuItem = new Idealde.Modules.MainMenu.Models.CommandMenuItem(command);
                    command.Tag = item;
                }

                if (newMenuItem == null) continue;

                if (parentMenuItem != null)
                    parentMenuItem.Children.Add(newMenuItem);
                else
                    MenuItems.Add(newMenuItem);
            }
        }
    }
}