using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

using FactrIDE.Properties;

using Gemini.Framework.Commands;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandDefinition]
    public class CopyItemFromProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Project.Copy";

        public override string Name => CommandName;
        public override string Text => Resources.CopyItemFromProjectText;
        public override string ToolTip => "";
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/Copy_16x.png");

        [Export]
        public static CommandKeyboardShortcut KeyGesture =
            new CommandKeyboardShortcut<CopyItemFromProjectCommandDefinition>(new KeyGesture(Key.C, ModifierKeys.Control));
    }
}