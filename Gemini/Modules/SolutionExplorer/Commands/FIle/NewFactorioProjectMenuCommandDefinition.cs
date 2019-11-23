using System;

using FactrIDE.Properties;

using Gemini.Framework.Commands;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Commands
{
    /*
    [CommandDefinition]
    public class NewFactrIDESolutionMenuCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.New.FactrIDESolution";

        public override string Name => CommandName;
        public override string Text => Resources.FactrIDESolutionText;
        public override string ToolTip => Resources.FactrIDESolutionToolTip;
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/FactrIDESolution_16x.png");
    }
    */
    [CommandDefinition]
    public class NewFactorioProjectMenuCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.New.FactorioModification";

        public override string Name => CommandName;
        public override string Text => Resources.FactorioModificationText;
        public override string ToolTip => Resources.FactorioModificationToolTip;
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/VS17/FactorioProject_16x.png");
    }
}