using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Inspectors
{
    /// <summary>
    /// Interaction logic for VersionEditorView.xaml
    /// </summary>
    public partial class VersionEditorView : UserControl
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+", RegexOptions.Compiled);
        private static bool IsTextAllowed(string text) => _regex.IsMatch(text);

        public VersionEditorView()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowed(e.Text);
        }
    }
}