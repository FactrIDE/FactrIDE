using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FactrIDE.Gemini.Modules.FactorioTranslator
{
    /// <summary>
    /// Interaction logic for Translator.xaml
    /// </summary>
    public partial class Translator : UserControl
    {
        public Translator()
        {
            InitializeComponent();

            /*
            using (Image<Rgba32> image = Image.Load("foo.jpg"))
            {
                image.Mutate(x => x
                     .Resize(image.Width / 2, image.Height / 2)
                     .Crop(
                     .Grayscale());
                image.Save("bar.jpg"); // Automatic encoder selected based on extension.
            }
            */
        }
    }
}
