using Caliburn.Micro;

using System;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Providers
{
    public abstract class DependencyBase : PropertyChangedBase
    {
        private string name;
        private Version version;

        public virtual string Name
        {
            get => name;
            set { name = value; NotifyOfPropertyChange(() => Name); }
        }
        public virtual Version Version
        {
            get => version;
            set { version = value; NotifyOfPropertyChange(() => Version); }
        }

        public virtual string DisplayText { get; protected set; }
    }
}