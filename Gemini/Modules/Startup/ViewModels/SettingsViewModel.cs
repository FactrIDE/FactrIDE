using Caliburn.Micro;

using Gemini.Framework;
using Gemini.Modules.Settings;
using Gemini.Modules.Settings.ViewModels;
using Gemini.Modules.Settings.Views;

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;

namespace FactrIDE.Gemini.Modules.Startup.ViewModels
{
    [Export(typeof(global::Gemini.Modules.Settings.ViewModels.SettingsViewModel))]
    public class SettingsViewModel : global::Gemini.Modules.Settings.ViewModels.SettingsViewModel
    {
        private IEnumerable<ISettingsEditor> _settingsEditors;

        public new List<SettingsPageViewModel> Pages { get; private set; }

        private SettingsPageViewModel _selectedPage;
        public new SettingsPageViewModel SelectedPage
        {
            get { return _selectedPage; }
            set
            {
                _selectedPage = value;
                NotifyOfPropertyChange(() => SelectedPage);
            }
        }

        public new ICommand CancelCommand { get; }
        public new ICommand OkCommand { get; }

        public SettingsViewModel()
        {
            ViewLocator.AddNamespaceMapping(typeof(SettingsViewModel).Namespace, typeof(SettingsView).Namespace);

            CancelCommand = new RelayCommand(_ => TryClose(false));
            OkCommand = new RelayCommand(SaveChanges);

            DisplayName = GeminiResources.SettingsDisplayName;
        }

        protected override void OnInitialize()
        {
            var pages = new List<SettingsPageViewModel>();
            _settingsEditors = IoC.GetAll<ISettingsEditor>();

            foreach (var settingsEditor in _settingsEditors)
            {
                var parentCollection = GetParentCollection(settingsEditor, pages);

                var page = parentCollection.Find(m => m.Name == settingsEditor.SettingsPageName);
                if (page == null)
                {
                    page = new SettingsPageViewModel { Name = settingsEditor.SettingsPageName };
                    parentCollection.Add(page);
                }

                page.Editors.Add(settingsEditor);
            }

            Pages = pages.OrderBy(se => se.Name).ToList(); // to sort main names
            foreach (var page in pages) // sort shildren
            {
                var children = page.Children;
                children = children.OrderBy(se => se.Name).ToList();
                page.Children.Clear();
                page.Children.AddRange(children);
                children.Clear();
            }
            SelectedPage = GetFirstLeafPageRecursive(Pages);
        }

        private static SettingsPageViewModel GetFirstLeafPageRecursive(List<SettingsPageViewModel> pages)
        {
            if (pages.Count == 0)
                return null;

            var firstPage = pages[0];
            if (firstPage.Children.Count == 0)
                return firstPage;

            return GetFirstLeafPageRecursive(firstPage.Children);
        }

        private List<SettingsPageViewModel> GetParentCollection(ISettingsEditor settingsEditor, List<SettingsPageViewModel> pages)
        {
            if (string.IsNullOrEmpty(settingsEditor.SettingsPagePath))
            {
                return pages;
            }

            foreach (string pathElement in settingsEditor.SettingsPagePath.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var page = pages.Find(s => s.Name == pathElement);
                if (page == null)
                {
                    page = new SettingsPageViewModel { Name = pathElement };
                    pages.Add(page);
                }

                pages = page.Children;
            }

            return pages;
        }

        private void SaveChanges(object obj)
        {
            foreach (var settingsEditor in _settingsEditors)
            {
                settingsEditor.ApplyChanges();
            }

            TryClose(true);
        }
    }
}