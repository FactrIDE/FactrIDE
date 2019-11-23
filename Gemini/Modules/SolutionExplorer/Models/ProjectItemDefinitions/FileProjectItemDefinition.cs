using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows;
using System.Windows.Input;

using Caliburn.Micro;

using FactrIDE.Gemini.Modules.SolutionExplorer;
using FactrIDE.Gemini.Modules.SolutionExplorer.Commands;

using Gemini.Framework;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;

using Idealde.Framework.Commands;
using Idealde.Framework.ProjectExplorer.Models;

namespace Idealde.Modules.ProjectExplorer.Models
{
    [Export]
    public class FileProjectItemDefinition : ProjectItemDefinition
    {
        private readonly ICommandService _commandService;

        public override IEnumerable<CommandDefinition> CommandDefinitions
        {
            get
            {
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(ExcludeItemFromProjectCommandDefinition));
                yield return new FakeCommandDefinition("|");
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(CutItemFromProjectCommandDefinition));
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(CopyItemFromProjectCommandDefinition));
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(RemoveItemFromProjectCommandDefinition));
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(RenameItemFromProjectCommandDefinition));
            }
        }

        public override ICommand ActiveCommand { get; }

        [ImportingConstructor]
        public FileProjectItemDefinition(ICommandService commandService)
        {
            _commandService = commandService;
            ActiveCommand = new RelayCommand(Active, CanActive);
        }

        public override string GetTooltip(object tag)
        {
            var itemTag = tag as ProjectFile;
            var path = itemTag.Path;
            return !File.Exists(path) ? "Resources.FileNotExistText" : $"{Path.GetExtension(path).ToLower()} file";
        }

        public override Uri GetIcon(bool isOpen, object tag)
        {
            var itemTag = tag as ProjectFile;

            string iconSource;
            if (!itemTag.Exists)
                iconSource = "pack://application:,,,/Resources/VS17/FileWarning_16x.png";
            else
            {
                switch (Path.GetExtension(itemTag.Path).ToLower())
                {
                    case "":
                    case ".txt":
                    case ".cfg":
                        iconSource = "pack://application:,,,/Resources/VS17/TextFile_16x.png";
                        break;
                    case ".md":
                        iconSource = "pack://application:,,,/Resources/VS17/MarkdownFile_16x.png";
                        break;
                    case ".json":
                        iconSource = "pack://application:,,,/Resources/VS17/JSONFile_16x.png";
                        break;
                    case ".lua":
                        iconSource = "pack://application:,,,/Resources/VS17/LuaFile_16x.png";
                        break;
                    case ".png":
                    case ".bmp":
                    case ".jpg":
                    case ".jpeg":
                        iconSource = "pack://application:,,,/Resources/VS17/Image_16x.png";
                        break;
                    default:
                        iconSource = "pack://application:,,,/Resources/VS17/File_16x.png";
                        break;
                }
            }

            return new Uri(iconSource, UriKind.Absolute);
        }

        private bool CanActive(object projectItemObject) => projectItemObject is ProjectItemBase projectItem
                && projectItem.Tag is ProjectFile itemTag
                && itemTag.Exists;

        private void Active(object projectItem)
        {
            var shell = IoC.Get<IShell>();

            var itemTag = ((ProjectItemBase) projectItem)?.Tag as ProjectFile;

            if (!itemTag.Exists)
            {
                MessageBox.Show("Resources.FileNotExistText", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (var document in shell.Documents)
            {
                if (document is IPersistedDocument persistedDocument)
                {
                    var editorFilePath = itemTag.Path.Trim().TrimEnd('\\').ToLower();
                    var persistedDocumentFilePath = persistedDocument.FilePath.Trim().TrimEnd('\\').ToLower();

                    if (string.CompareOrdinal(editorFilePath, persistedDocumentFilePath) != 0) continue;
                    shell.OpenDocument(persistedDocument);
                    return;
                }
                else
                    continue;
            }

            Helper.OpenFileEditor(itemTag.Path);
        }
    }
}