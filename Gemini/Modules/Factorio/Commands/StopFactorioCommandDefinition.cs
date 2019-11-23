using FactrIDE.Properties;

using Gemini.Framework.Commands;

using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace FactrIDE.Gemini.Modules.Factorio.Commands
{
    [CommandDefinition]
    public class StopFactorioCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Shell.StopFactorio";

        public override string Name => CommandName;
        public override string Text => Resources.StopFactorioCommandText;
        public override string ToolTip => Resources.StopFactorioCommandToolTip;
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/Stop_16x.png");

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<StopFactorioCommandDefinition>(new KeyGesture(Key.F5, ModifierKeys.Shift));
    }
}