using FactrIDE.Properties;

using Gemini.Framework.Commands;

using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace FactrIDE.Gemini.Modules.Factorio.Commands
{
    [CommandDefinition]
    public class RunFactorioCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Shell.RunFactorio";

        public override string Name => CommandName;
        public override string Text => Resources.RunFactorioCommandText;
        public override string ToolTip => Resources.RunFactorioCommandToolTip;
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/Run_16x.png");

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<RunFactorioCommandDefinition>(new KeyGesture(Key.F5));
    }
}