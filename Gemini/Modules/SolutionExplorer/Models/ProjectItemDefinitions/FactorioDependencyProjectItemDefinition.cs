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
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class FactorioDependencyProjectItemDefinition : ProjectItemDefinition
    {
        private readonly ICommandService _commandService;

        [ImportingConstructor]
        public FactorioDependencyProjectItemDefinition(ICommandService commandService)
        {
            _commandService = commandService;
        }

        public override IEnumerable<CommandDefinition> CommandDefinitions
        {
            get
            {
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(RemoveItemFromProjectCommandDefinition));
                yield return new FakeCommandDefinition("|");
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(OpenDependencyFromProjectCommandDefinition));
                yield return new FakeCommandDefinition("|");
                yield return (CommandDefinition) _commandService.GetCommandDefinition(typeof(ViewItemPropertiesFromProjectCommandDefinition));
            }
        }

        public override ICommand ActiveCommand => null;

        public override string GetTooltip(object tag) => "ADDTOOLTIP";

        public override Uri GetIcon(bool isOpen, object tag)
        {
            return new Uri("pack://application:,,,/Resources/VS17/ZipFile_16x.png");
            /*
            if (tag is string jsonRaw)
            {
                var dependency = new FactorioModificationDependency(jsonRaw);
                if (dependency.IsNotCompatible)
                    return new Uri("pack://application:,,,/Resources/VS17/ZipFileError_16x.png");
                if (dependency.IsOptional)
                    return new Uri("pack://application:,,,/Resources/VS17/ZipFileWarning_16x.png");
                return new Uri("pack://application:,,,/Resources/VS17/ZipFile_16x.png");
            }
            else if (tag is FactorioModificationDependency dependency)
            {
                if (dependency.IsNotCompatible)
                    return new Uri("pack://application:,,,/Resources/VS17/ZipFileError_16x.png");
                if (dependency.IsOptional)
                    return new Uri("pack://application:,,,/Resources/VS17/ZipFileWarning_16x.png");
                return new Uri("pack://application:,,,/Resources/VS17/ZipFile_16x.png");
            }
            return new Uri("pack://application:,,,/Resources/VS17/Cancel_16x.png");
            */
            /*
            if(Dependency.IsNotCompatible)
                return new Uri("pack://application:,,,/Resources/VS17/Cancel_16x.png");
            if (Dependency.IsOptional)
                return new Uri("pack://application:,,,/Resources/VS17/Exclamation_grey_16x.png");
            return new Uri("pack://application:,,,/Resources/VS17/Checkmark_16x.png");
            */
        }
    }
}