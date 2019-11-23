using Caliburn.Micro;

using Gemini.Framework.Services;
using Gemini.Modules.Shell.Views;

using System.ComponentModel.Composition;
using System.Reflection;

using Xceed.Wpf.AvalonDock;

namespace FactrIDE.Gemini.Modules.Startup.ViewModels
{
    // https://github.com/tgjones/gemini/issues/281
    [Export(typeof(IShell))]
    public class ShellViewModel : global::Gemini.Modules.Shell.ViewModels.ShellViewModel
    {
        public ShellViewModel()
        {
            ViewLocator.AddNamespaceMapping(typeof(ShellViewModel).Namespace, typeof(ShellView).Namespace);
        }

        internal static DockingManager GetDockingManager(IShellView view)
        {
            if(view is ShellView shellView)
            {
                var managerProperty = shellView.GetType().GetField("Manager", BindingFlags.NonPublic | BindingFlags.Instance);
                return (DockingManager) managerProperty.GetValue(shellView);
            }
            return null;
        }

        protected override void OnViewLoaded(object view)
        {
            var dockingManager = GetDockingManager(view as IShellView);
            dockingManager.LayoutUpdateStrategy = new HackedLayoutInitializer();
            base.OnViewLoaded(view);
        }
    }
}