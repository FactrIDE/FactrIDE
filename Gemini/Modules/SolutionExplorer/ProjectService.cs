using Caliburn.Micro;

using Idealde.Framework.ProjectExplorer.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Idealde.Framework.Projects
{
    [Export(typeof(IProjectService))]
    public class ProjectService : IProjectService
    {
        private Dictionary<Type, ProjectItemDefinition> ProjectItemTypeToDefinitionLookup { get; } = new Dictionary<Type, ProjectItemDefinition>();

        public ProjectItemDefinition GetProjectItemDefinition(Type projectItemDefinitionType)
        {
            if (!ProjectItemTypeToDefinitionLookup.TryGetValue(projectItemDefinitionType, out ProjectItemDefinition projectItemDefinition))
            {
                projectItemDefinition = (ProjectItemDefinition) IoC.GetInstance(projectItemDefinitionType, string.Empty);
                ProjectItemTypeToDefinitionLookup.Add(projectItemDefinitionType, projectItemDefinition);
            }
            return projectItemDefinition;
        }
    }
}