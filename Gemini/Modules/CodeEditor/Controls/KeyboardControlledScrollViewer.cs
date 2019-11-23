using System.Windows.Controls;
using System.Windows.Input;

namespace FactrIDE.Gemini.Modules.CodeEditor.Controls
{
    public class KeyboardControlledScrollViewer : ScrollViewer
    {
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                e.Handled = true;
                return;
            }

            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                if (e.Delta > 0)
                    ScrollInfo.MouseWheelLeft();
                else
                    ScrollInfo.MouseWheelRight();

                e.Handled = true;
            }

            base.OnMouseWheel(e);
        }
    }
}