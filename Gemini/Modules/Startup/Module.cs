using FactrIDE.Properties;

using Gemini.Framework;

using System.ComponentModel.Composition;
using System.Drawing;

namespace FactrIDE.Gemini.Modules.Startup
{
    // Dirkster.AvalonDock.Themes.VS2013 alternative
    [Export(typeof(IModule))]
    public class Module : ModuleBase
    {
        public override void Initialize()
        {
            Shell.ToolBars.Visible = true;

            MainWindow.Title = Resources.MainWindowTitle;

            // No idea how to use Gemini's resource manager
            //var icon = (Icon) new System.Resources.ResourceManager(typeof(Resources)).GetObject("Icon_256x");
            //MainWindow.Icon = icon.ToImageSource();
            //icon?.Dispose();
        }
    }
}