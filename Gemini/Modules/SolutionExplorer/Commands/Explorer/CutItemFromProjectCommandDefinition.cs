using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

using FactrIDE.Properties;

using Gemini.Framework.Commands;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    [CommandDefinition]
    public class CutItemFromProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Project.Cut";

        public override string Name => CommandName;
        public override string Text => Resources.CutItemFromProjectText;
        public override string ToolTip => "";
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/Cut_16x.png");

        [Export]
        public static CommandKeyboardShortcut KeyGesture =
            new CommandKeyboardShortcut<CutItemFromProjectCommandDefinition>(new KeyGesture(Key.X, ModifierKeys.Control));
    }
}