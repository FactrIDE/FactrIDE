using System;
using System.Collections.Generic;
using System.Windows.Input;

using Caliburn.Micro;

using Gemini.Framework.Commands;

namespace Idealde.Modules.ProjectExplorer.Models
{
    public abstract class ProjectItemBase : PropertyChangedBase
    {
        public ProjectItemBase Parent { get; set; }

        public abstract object Tag { get; set; }

        public abstract Uri IconSource { get; }

        public abstract string ToolTip { get; }

        public abstract ICommand ActiveCommand { get; }

        public abstract IEnumerable<Command> OptionCommands { get; }

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                if (value == _text) return;
                _text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }

        private bool _isOpen;
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (value == _isOpen || Children.Count == 0) return;
                _isOpen = value;
                NotifyOfPropertyChange(() => IsOpen);
                NotifyOfPropertyChange(() => IconSource);
            }
        }

        public IObservableCollection<ProjectItemBase> Children { get; set; }

        protected ProjectItemBase()
        {
            Children = new BindableCollection<ProjectItemBase>();
            IsOpen = false;
        }

        public void AddChild(ProjectItemBase child)
        {
            if (child == null) return;

            Children.Add(child);
            child.Parent = this;
        }
    }
}