using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

using FactrIDE.Properties;

using Gemini.Framework.Commands;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandDefinition]
    public class RenameItemFromProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Project.Rename";

        public override string Name => CommandName;
        public override string Text => Resources.RenameItemFromProjectText;
        public override string ToolTip => "";
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/Rename_16x.png");

        [Export]
        public static CommandKeyboardShortcut KeyGesture =
            new CommandKeyboardShortcut<RenameItemFromProjectCommandDefinition>(new KeyGesture(Key.F2, ModifierKeys.None));
    }
}