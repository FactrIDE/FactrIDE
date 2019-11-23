using System;
using System.Collections.Generic;
using System.Windows.Input;

using Caliburn.Micro;

using Gemini.Framework.Commands;

namespace Idealde.Framework.ProjectExplorer.Models
{
    public abstract class ProjectItemDefinition : PropertyChangedBase
    {
        public abstract IEnumerable<CommandDefinition> CommandDefinitions { get; }

        public abstract ICommand ActiveCommand { get; }

        public abstract string GetTooltip(object tag);

        public abstract Uri GetIcon(bool isOpen, object tag);
    }
}