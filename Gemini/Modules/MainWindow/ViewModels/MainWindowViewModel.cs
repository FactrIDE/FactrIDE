using Caliburn.Micro;

using FactrIDE.Properties;

using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Modules.MainWindow.Views;

using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace FactrIDE.Gemini.Modules.MainWindow.ViewModels
{
    [Export(typeof(IMainWindow))]
    public class MainWindowViewModel : Conductor<IShell>, IMainWindow, IPartImportsSatisfiedNotification
    {
        [Import]
        private IResourceManager _resourceManager;

        [Import]
        private ICommandKeyGestureService _commandKeyGestureService;

        [Import]
        private IShell _shell;
        public IShell Shell => _shell;

        private WindowState _windowState = WindowState.Normal;
        public WindowState WindowState
        {
            get => _windowState;
            set { _windowState = value; NotifyOfPropertyChange(() => WindowState); }
        }

        private double _width = 1000.0;
        public double Width
        {
            get => _width;
            set { _width = value; NotifyOfPropertyChange(() => Width); }
        }

        private double _height = 800.0;
        public double Height
        {
            get => _height;
            set { _height = value; NotifyOfPropertyChange(() => Height); }
        }

        private string _title = Resources.MainWindowTitle;
        public string Title
        {
            get => _title;
            set { _title = value; NotifyOfPropertyChange(() => Title); }
        }

        private ImageSource _icon;
        public ImageSource Icon
        {
            get => _icon;
            set { _icon = value; NotifyOfPropertyChange(() => Icon); }
        }

        public MainWindowViewModel()
        {
            ViewLocator.AddNamespaceMapping(typeof(MainWindowViewModel).Namespace, typeof(MainWindowView).Namespace);
        }

        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            if (_icon == null)
                _icon = _resourceManager.GetBitmap("Resources/Icons/Icon_256x.png", Assembly.GetExecutingAssembly().GetAssemblyName());
            ActivateItem(_shell);
        }

        protected override void OnViewLoaded(object view)
        {
            if(view is MainWindowView mainWindowView)
            {
#if !DEBUG
                mainWindowView.Topmost = true;
#endif
            }

            _commandKeyGestureService.BindKeyGestures((UIElement) view);
            base.OnViewLoaded(view);
        }
    }
}