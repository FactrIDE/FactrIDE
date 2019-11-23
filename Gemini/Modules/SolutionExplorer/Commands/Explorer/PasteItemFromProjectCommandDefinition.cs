using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

using FactrIDE.Properties;

using Gemini.Framework.Commands;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandDefinition]
    public class PasteItemFromProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Project.Paste";

        public override string Name => CommandName;
        public override string Text => Resources.PasteItemFromProjectText;
        public override string ToolTip => "";
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/Paste_16x.png");

        [Export]
        public static CommandKeyboardShortcut KeyGesture =
            new CommandKeyboardShortcut<PasteItemFromProjectCommandDefinition>(new KeyGesture(Key.V, ModifierKeys.Control));
    }
}