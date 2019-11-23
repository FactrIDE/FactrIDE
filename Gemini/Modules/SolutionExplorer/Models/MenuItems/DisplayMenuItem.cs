using Gemini.Modules.MainMenu.Models;

using System;
using System.Windows.Input;

namespace Idealde.Modules.MainMenu.Models
{
    public class DisplayMenuItem : StandardMenuItem
    {
        public override string Text { get; }
        public string ToolTip { get; }
        public override Uri IconSource { get; }
        public override string InputGestureText => null;
        public override ICommand Command => null;
        public override bool IsChecked { get; }
        public override bool IsVisible { get; }

        public DisplayMenuItem(string text, string toolTip = "", Uri iconSource = null)
        {
            Text = text;
            ToolTip = toolTip;
            IconSource = iconSource;
        }
    }
}