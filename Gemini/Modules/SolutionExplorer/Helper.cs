using Caliburn.Micro;

using Gemini.Framework.Services;

using System.Linq;
using System.Windows;

namespace FactrIDE.Gemini.Modules.SolutionExplorer
{
    public static class Helper
    {
        public static void OpenFileEditor(string fullFilePath)
        {
            var provider = IoC.GetAllInstances(typeof(IEditorProvider))
                .Cast<IEditorProvider>()
                .FirstOrDefault(p => p.Handles(fullFilePath));
            if (provider == null) return;

            var editor = provider.Create();
            var viewAware = (IViewAware) editor;
            viewAware.ViewAttached += (_, e) =>
            {
                var frameworkElement = (FrameworkElement)e.View;
                async void loadedHandler(object sender2, RoutedEventArgs e2)
                {
                    frameworkElement.Loaded -= loadedHandler;
                    await provider.Open(editor, fullFilePath).ConfigureAwait(false);
                }
                frameworkElement.Loaded += loadedHandler;
            };
            var shell = IoC.Get<IShell>();
            shell.OpenDocument(editor);
        }
    }
}