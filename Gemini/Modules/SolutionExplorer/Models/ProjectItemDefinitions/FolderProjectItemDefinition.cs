using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Input;

using FactrIDE.Gemini.Modules.SolutionExplorer.Commands;

using Gemini.Framework.Commands;

using Idealde.Framework.Commands;
using Idealde.Framework.ProjectExplorer.Models;

namespace Idealde.Modules.ProjectExplorer.Models
{
    [Export]
    public class FolderProjectItemDefinition : ProjectItemDefinition
    {
        private readonly ICommandService _commandService;

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
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(ExcludeItemFromProjectCommandDefinition));
                yield return new FakeCommandDefinition("|");
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(CutItemFromProjectCommandDefinition));
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(CopyItemFromProjectCommandDefinition));
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(PasteItemFromProjectCommandDefinition));
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(RemoveItemFromProjectCommandDefinition));
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(RenameItemFromProjectCommandDefinition));
                yield return new FakeCommandDefinition("|");
                yield return (CommandDefinition)_commandService.GetCommandDefinition(typeof(OpenFolderInProjectCommandDefinition));
            }
        }

        public override ICommand ActiveCommand { get; }

        [ImportingConstructor]
        public FolderProjectItemDefinition(ICommandService commandService)
        {
            _commandService = commandService;
        }

        public override string GetTooltip(object tag) => string.Empty;

        public override Uri GetIcon(bool isOpen, object tag) => isOpen
                ? new Uri("pack://application:,,,/Resources/VS17/FolderOpen_16x.png", UriKind.Absolute)
                : new Uri("pack://application:,,,/Resources/VS17/Folder_16x.png", UriKind.Absolute);
    }
}