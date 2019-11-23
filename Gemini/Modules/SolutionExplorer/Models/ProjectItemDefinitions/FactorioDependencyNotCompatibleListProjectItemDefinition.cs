using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Input;

using FactrIDE.Gemini.Modules.SolutionExplorer.Commands;

using Gemini.Framework.Commands;

using Idealde.Framework.Commands;
using Idealde.Framework.ProjectExplorer.Models;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Models
{
    [Export]
    public class FactorioDependencyNotCompatibleListProjectItemDefinition : ProjectItemDefinition
    {
        private readonly ICommandService _commandService;

        [ImportingConstructor]
        public FactorioDependencyNotCompatibleListProjectItemDefinition(ICommandService commandService)
        {
            _commandService = commandService;
        }

        public override IEnumerable<CommandDefinition> CommandDefinitions
        {
            get
            {
                yield return new FakeCommandDefinition("|Add");
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(AddDependencyToProjectCommandDefinition));
            }
        }

        public override ICommand ActiveCommand => null;

        public override string GetTooltip(object tag) => "ADDTOOLTIP";

        public override Uri GetIcon(bool isOpen, object tag) =>
            new Uri("pack://application:,,,/Resources/VS17/ZipFileError_16x.png");
    }
}