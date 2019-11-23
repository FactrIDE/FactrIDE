using System.Windows;
using System.Windows.Controls;

using Gemini.Modules.MainMenu.Models;

namespace Idealde.Modules.MainMenu.Controls
{
    public class MenuItemStyleSelector : StyleSelector
    {
        public Style SeparatorStyle { get; set; }
        public Style MenuItemStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container) => item switch
        {
            MenuItemSeparator _ => SeparatorStyle,
            MenuItemBase _ => MenuItemStyle,
            _ => base.SelectStyle(item, container),
        };
    }
}