using Caliburn.Micro;

using Gemini.Framework.Commands;
using Gemini.Modules.MainMenu.Models;

using System;
using System.Windows.Input;

namespace Idealde.Modules.MainMenu.Models
{
    public class CommandMenuItem : StandardMenuItem
    {
        private readonly ICommandService _commandService;

        private readonly Command _command;

        public override string Text => _command.Text;
        public string ToolTip => _command.ToolTip;
        public override Uri IconSource => _command.IconSource;
        public override ICommand Command => _commandService.GetTargetableCommand(_command);
        public override string InputGestureText => null;
        public override bool IsChecked { get; }
        public override bool IsVisible { get; }

        public CommandMenuItem(Command command)
        {
            _commandService = IoC.Get<ICommandService>();
            _command = command;
            _command.PropertyChanged += (s, e) => NotifyOfPropertyChange(e.PropertyName);
        }

        public CommandMenuItem(CommandDefinition commandDefinition)
        {
            _commandService = IoC.Get<ICommandService>();
            _command = _commandService.GetCommand(commandDefinition);
            _command.PropertyChanged += (s, e) => NotifyOfPropertyChange(e.PropertyName);
        }

        protected CommandMenuItem(Type commandDefinitionType)
        {
            _commandService = IoC.Get<ICommandService>();
            _command = _commandService.GetCommand(_commandService.GetCommandDefinition(commandDefinitionType));
            _command.PropertyChanged += (s, e) => NotifyOfPropertyChange(e.PropertyName);
        }
    }
}