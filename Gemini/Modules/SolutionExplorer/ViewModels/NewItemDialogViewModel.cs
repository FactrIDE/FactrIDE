using Caliburn.Micro;

using FactrIDE.Properties;

using System;
using System.ComponentModel.Composition;

using Xceed.Wpf.Toolkit;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.ViewModels
{
    [Export]
    public class NewItemDialogViewModel : Screen
    {
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                changed = true;
                if (value == _name) return;
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }
        private bool changed;

        private bool _closing;

        protected override void OnInitialize()
        {
            DisplayName = Resources.NewItemWindowTitle;

            base.OnInitialize();
        }

        protected override void OnViewAttached(object view, object context)
        {
            if (!changed)
                Name = string.Empty;
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
                _closing = dialogResult.Value;

            base.TryClose(dialogResult);
        }

        public override void CanClose(Action<bool> callback)
        {
            var canClose = true;

            if (_closing)
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    canClose = false;
                    MessageBox.Show(Resources.PleaseEnterItemNameText);
                }
            }

            callback(canClose);
        }
    }
}