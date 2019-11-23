using Gemini.Modules.Inspector.Inspectors;

using System;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Inspectors
{
    public class VersionEditorViewModel : EditorBase<Version>, ILabelledInspector
    {
        public int Major
        {
            get => Value.Major;
            set
            {
                Value = new Version(value, Value.Minor, Value.Build);
                NotifyOfPropertyChange(() => Major);
            }
        }

        public int Minor
        {
            get => Value.Minor;
            set
            {
                Value = new Version(Value.Major, value, Value.Build);
                NotifyOfPropertyChange(() => Minor);
            }
        }

        public int Build
        {
            get => Value.Build;
            set
            {
                Value = new Version(Value.Major, Value.Minor, value);
                NotifyOfPropertyChange(() => Build);
            }
        }

        public override void NotifyOfPropertyChange(string propertyName)
        {
            if (string.Equals(propertyName, nameof(Value), StringComparison.InvariantCulture))
            {
                NotifyOfPropertyChange(() => Major);
                NotifyOfPropertyChange(() => Minor);
                NotifyOfPropertyChange(() => Build);
            }
            base.NotifyOfPropertyChange(propertyName);
        }
    }
}