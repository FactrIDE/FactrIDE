using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

using FactrIDE.Properties;

using Gemini.Framework.Commands;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandDefinition]
    public class RemoveItemFromProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Project.Remove";

        public override string Name => CommandName;
        public override string Text => Resources.RemoveItemFromProjectText;
        public override string ToolTip => "";
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/Cancel_16x.png");

        [Export]
        public static CommandKeyboardShortcut KeyGesture =
           new CommandKeyboardShortcut<RemoveItemFromProjectCommandDefinition>(new KeyGesture(Key.Delete));
    }
}