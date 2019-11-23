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
    public class FactorioDependencyListProjectItemDefinition : ProjectItemDefinition
    {
        private readonly ICommandService _commandService;

        [ImportingConstructor]
        public FactorioDependencyListProjectItemDefinition(ICommandService commandService)
        {
            _commandService = commandService;
        }

        public override IEnumerable<CommandDefinition> CommandDefinitions => new List<CommandDefinition>();

        public override ICommand ActiveCommand => null;

        public override string GetTooltip(object tag) => "ADDTOOLTIP";

        public override Uri GetIcon(bool isOpen, object tag) =>
            new Uri("pack://application:,,,/Resources/VS17/Reference_16x.png");
        //public override Uri GetIcon(bool isOpen, object tag) =>
        //    new Uri("pack://application:,,,/Resources/VS17/ZipFile_16x.png");
    }
}