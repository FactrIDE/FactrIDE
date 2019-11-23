using Gemini.Framework.Commands;

using System;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Extensions
{
    public static class ICommandServiceExtensions
    {
        public static CommandDefinitionBase GetCommandDefinition<TCommandDefinition>(this ICommandService commandService) =>
            commandService.GetCommandDefinition(typeof(TCommandDefinition));

        public static Command GetCommand<TCommandDefinition>(this ICommandService commandService) => GetCommand(commandService, typeof(TCommandDefinition));
        public static Command GetCommand(this ICommandService commandService, Type commandDefinitionType)
        {
            var commandDefinition = commandService.GetCommandDefinition(commandDefinitionType);
            return commandService.GetCommand(commandDefinition);
        }
    }
}