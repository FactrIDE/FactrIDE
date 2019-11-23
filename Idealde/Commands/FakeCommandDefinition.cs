using Gemini.Framework.Commands;

namespace Idealde.Framework.Commands
{
    public class FakeCommandDefinition : CommandDefinition
    {
        public override string Name { get; }
        public override string Text { get; }
        public override string ToolTip { get; }

        public FakeCommandDefinition(string name) => Name = name;
    }
}