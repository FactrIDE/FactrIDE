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
    public class FactorioProjectItemDefinition : ProjectItemDefinition
    {
        private readonly ICommandService _commandService;

        [ImportingConstructor]
        public FactorioProjectItemDefinition(ICommandService commandService)
        {
            _commandService = commandService;
        }

        public override IEnumerable<CommandDefinition> CommandDefinitions
        {
            get
            {
                yield return new FakeCommandDefinition("|Add");
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(AddFileToProjectCommandDefinition));
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(AddFolderToProjectCommandDefinition));
                yield return new FakeCommandDefinition("|");
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(NewLuaFileToProjectCommandDefinition));
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(NewLocalizationFileToProjectCommandDefinition));
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(NewFolderToProjectCommandDefinition));
                yield return new FakeCommandDefinition("");
                yield return new FakeCommandDefinition("|");
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(EditProjectFileFromProjectCommandDefinition));
                yield return new FakeCommandDefinition("|");
                //yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(CutItemFromProjectCommandDefinition));
                //yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(CopyItemFromProjectCommandDefinition));
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(PasteItemFromProjectCommandDefinition));
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(RemoveItemFromProjectCommandDefinition));
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(RenameItemFromProjectCommandDefinition));
                yield return new FakeCommandDefinition("|");
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(OpenFolderInProjectCommandDefinition));
                yield return new FakeCommandDefinition("|");
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(ViewItemPropertiesFromProjectCommandDefinition));

            }
        }

        public override ICommand ActiveCommand => null;

        public override string GetTooltip(object tag) => "Factorio Modification Project";

        public override Uri GetIcon(bool isOpen, object tag) => new Uri("pack://application:,,,/Resources/VS17/FactorioProject_16x.png");
    }
}