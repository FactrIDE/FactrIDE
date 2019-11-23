using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FactrIDE.Gemini.Modules.CodeEditor.Controls
{
    public class ScalableImage : Image
    {
        private TransformGroup TransformGroup { get; } = new TransformGroup();
        private ScaleTransform ScaleTransform { get; } = new ScaleTransform(1, 1);
        private TranslateTransform TranslateTransform { get; } = new TranslateTransform(0, 0);

        private int zoomIn, zoomOut;

        public ScalableImage()
        {
            TransformGroup.Children.Add(ScaleTransform);
            TransformGroup.Children.Add(TranslateTransform);
            LayoutTransform = TransformGroup;
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            const int step = 1;
            // No idea why it;s not transforming correctly
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                // Too lazy to combine blocks
                if (e.Delta > 0 && zoomIn < 40)
                {
                    var mouseRelativetoImage = e.GetPosition(this);
                    var abosuluteX = (mouseRelativetoImage.X * ScaleTransform.ScaleX) + TranslateTransform.X;
                    var abosuluteY = (mouseRelativetoImage.Y * ScaleTransform.ScaleY) + TranslateTransform.Y;

                    zoomIn += step;
                    zoomOut -= step;
                    ScaleTransform.ScaleX += 0.2;
                    ScaleTransform.ScaleY += 0.2;

                    TranslateTransform.X = abosuluteX - (mouseRelativetoImage.X * ScaleTransform.ScaleX);
                    TranslateTransform.Y = abosuluteY - (mouseRelativetoImage.Y * ScaleTransform.ScaleY);
                }
                if (e.Delta < 0 && zoomOut < 0)
                {
                    var mouseRelativetoImage = e.GetPosition(this);
                    var abosuluteX = (mouseRelativetoImage.X * ScaleTransform.ScaleX) + TranslateTransform.X;
                    var abosuluteY = (mouseRelativetoImage.Y * ScaleTransform.ScaleY) + TranslateTransform.Y;

                    zoomIn -= step;
                    zoomOut += step;
                    ScaleTransform.ScaleX -= 0.2;
                    ScaleTransform.ScaleY -= 0.2;

                    TranslateTransform.X = abosuluteX - (mouseRelativetoImage.X * ScaleTransform.ScaleX);
                    TranslateTransform.Y = abosuluteY - (mouseRelativetoImage.Y * ScaleTransform.ScaleY);
                }

                e.Handled = true;
            }

            base.OnMouseWheel(e);
        }

        protected override void OnRender(DrawingContext dc)
        {
            VisualBitmapScalingMode = BitmapScalingMode.NearestNeighbor;

            base.OnRender(dc);
        }
    }
}