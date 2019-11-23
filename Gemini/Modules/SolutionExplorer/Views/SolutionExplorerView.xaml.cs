using System.Windows.Controls;
using System.Windows.Input;

using Idealde.Modules.ProjectExplorer.Models;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Views
{
    /// <summary>
    /// Interaction logic for SolutionExplorerView.xaml
    /// </summary>
    public partial class SolutionExplorerView : UserControl
    {
        public SolutionExplorerView()
        {
            InitializeComponent();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) { }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as TreeViewItem;
            var projectItem = item?.DataContext as ProjectItemBase;
            if (projectItem?.ActiveCommand == null) return;
            if (!projectItem.ActiveCommand.CanExecute(projectItem)) return;
            projectItem.ActiveCommand.Execute(projectItem);
        }

        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as TreeViewItem;
            item?.Focus();
            e.Handled = true;
        }
    }
}