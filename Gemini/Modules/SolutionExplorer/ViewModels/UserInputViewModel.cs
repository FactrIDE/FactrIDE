using Caliburn.Micro;

using Gemini.Framework;

using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Input;

using Xceed.Wpf.Toolkit;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.ViewModels
{
    [Export]
    public class UserInputViewModel : Screen
    {
        public string FileExtension { get; }

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
        public string Title { get; }

        private string _fileName = string.Empty;
        public string FileName
        {
            get => _fileName;
            set
            {
                if (Equals(value, _fileName)) return;
                _fileName = value;
                NotifyOfPropertyChange(() => FileName);
            }
        }

        public string FilePath { get; }

        public UserInputViewModel(string title, string filePath, string fileFileExtension)
        {
            OkCommand = new RelayCommand(_ => OkPressed());
            CancelCommand = new RelayCommand(_ => CancelPressed());
            Title = title;
            FilePath = filePath;
            FileExtension = fileFileExtension;
        }

        private void OkPressed()
        {
            if (File.Exists(Path.Combine(FilePath, FileName + FileExtension)))
            {
                MessageBox.Show($"File name {FileName}{FileExtension} already exists");
                return;
            }
            TryClose(true);
        }

        private void CancelPressed()
        {
            FileName = "";
            TryClose(true);
        }
    }
}