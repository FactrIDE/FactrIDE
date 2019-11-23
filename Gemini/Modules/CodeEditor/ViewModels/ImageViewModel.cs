using FactrIDE.Gemini.Modules.CodeEditor.Views;

using Gemini.Framework;

using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FactrIDE.Gemini.Modules.CodeEditor.ViewModels
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ImageViewModel : PersistedDocument
    {
        private ImageView _view;
        private byte[] _imageData;

        protected override Task DoNew()
        {
            _imageData = Array.Empty<byte>();
            LoadImage();
            return Task.CompletedTask;
        }

        protected override Task DoLoad(string filePath)
        {
            _imageData = File.ReadAllBytes(filePath);
            LoadImage();
            var bmp = _view.image.Source as BitmapFrame;
            DisplayName = $"{Path.GetFileName(filePath)} ({bmp.PixelWidth}x{bmp.PixelHeight})";
            return Task.CompletedTask;
        }

        protected override Task DoSave(string filePath)
        {
            //var newText = _view.image.Source;
            //File.WriteAllBytes(filePath, newText);
            //_imageData = newText;
            File.WriteAllBytes(filePath, _imageData);
            return Task.CompletedTask;
        }

        private void LoadImage()
        {
            using var stream = new MemoryStream(_imageData);
            _view.image.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
        }

        protected override void OnViewLoaded(object view)
        {
            _view = (ImageView) view;
        }

        public override bool Equals(object obj)
        {
            return obj is ImageViewModel other
                && string.Equals(FilePath, other.FilePath, StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(FileName, other.FileName, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}