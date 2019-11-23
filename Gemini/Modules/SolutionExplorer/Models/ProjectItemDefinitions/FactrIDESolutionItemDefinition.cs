using FactrIDE.Gemini.Modules.SolutionExplorer.Commands;

using Gemini.Framework.Commands;

using Idealde.Framework.Commands;
using Idealde.Framework.ProjectExplorer.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Models
{
    [Export]
    public class FactrIDESolutionItemDefinition : ProjectItemDefinition
    {
        private readonly ICommandService _commandService;

        [ImportingConstructor]
        public FactrIDESolutionItemDefinition(ICommandService commandService)
        {
            _commandService = commandService;
        }

        public override IEnumerable<CommandDefinition> CommandDefinitions
        {
            get
            {
                yield return new FakeCommandDefinition("|Add");
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(NewFactorioProjectToSolutionCommandDefinition));
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(AddFactorioProjectToSolutionExplorerCommandDefinition));
                yield return new FakeCommandDefinition("");
                yield return new FakeCommandDefinition("|");
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(RenameItemFromProjectCommandDefinition));
            }
        }

        public override ICommand ActiveCommand => null;

        public override string GetTooltip(object tag) => "FactrIDE Solution";

        public override Uri GetIcon(bool isOpen, object tag) => new Uri("pack://application:,,,/Resources/VS17/FactrIDESolution_16x.png");
    }
}