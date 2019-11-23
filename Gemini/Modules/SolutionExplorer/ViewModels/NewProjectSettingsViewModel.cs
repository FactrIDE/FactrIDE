using Caliburn.Micro;

using FactrIDE.Properties;

using System;
using System.ComponentModel.Composition;
using System.IO;

using Xceed.Wpf.Toolkit;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.ViewModels
{
    [Export]
    public class NewProjectSettingsViewModel : Screen
    {
        private string _solutionName;
        public string SolutionName
        {
            get { return _solutionName; }
            set
            {
                changed = true;
                if (value == _solutionName) return;
                _solutionName = value;
                NotifyOfPropertyChange(() => SolutionName);
            }
        }

        private string _projectName;
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                changed = true;
                if (value == _projectName) return;
                _projectName = value;
                NotifyOfPropertyChange(() => ProjectName);
            }
        }

        private string _rootLocation;
        public string RootLocation
        {
            get { return _rootLocation; }
            set
            {
                changed = true;
                if (value == _rootLocation) return;
                _rootLocation = value;
                NotifyOfPropertyChange(() => RootLocation);
            }
        }

        private bool _projectInRoot;
        public bool ProjectInRoot
        {
            get { return _projectInRoot; }
            set
            {
                changed = true;
                if (value == _projectInRoot) return;
                _projectInRoot = value;
                NotifyOfPropertyChange(() => ProjectInRoot);
            }
        }
        private bool changed;

        private bool _closing;

        protected override void OnInitialize()
        {
            DisplayName = Resources.NewProjectWindowTitle;

            SolutionName = string.Empty;
            ProjectName = string.Empty;
            RootLocation = string.Empty;
            ProjectInRoot = false;

            _closing = false;

            base.OnInitialize();
        }

        protected override void OnViewAttached(object view, object context)
        {
            if (!changed)
            {
                SolutionName = string.Empty;
                ProjectName = string.Empty;
                RootLocation = string.Empty;
                ProjectInRoot = false;
            }

            base.OnViewAttached(view, context);
        }
        protected override void OnDeactivate(bool close)
        {
            changed = false;
            base.OnDeactivate(close);
        }

        public override void TryClose(bool? dialogResult = default)
        {
            if (dialogResult != null)
            {
                _closing = dialogResult.Value;
            }

            base.TryClose(dialogResult);
        }

        public override void CanClose(Action<bool> callback)
        {
            var canClose = true;

            if (_closing)
            {
                if (string.IsNullOrWhiteSpace(ProjectName))
                {
                    canClose = false;
                    MessageBox.Show(Resources.PleaseEnterProjectNameText);
                }
                else if (!Directory.Exists(RootLocation))
                {
                    canClose = false;
                    MessageBox.Show(Resources.ProjectRootDirectoryNotExistText);
                }
            }

            callback(canClose);
        }
    }
}