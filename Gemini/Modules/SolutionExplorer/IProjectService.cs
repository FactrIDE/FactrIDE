using Idealde.Framework.ProjectExplorer.Models;

using System;

namespace Idealde.Framework.Projects
{
    public interface IProjectService
    {
        ProjectItemDefinition GetProjectItemDefinition(Type projectItemDefinitionType);
    }
}