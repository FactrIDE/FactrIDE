using System.IO;
using System.Text;

namespace FactrIDE.Gemini.Modules.FactorioModificationOutput.ViewModels
{
    internal class OutputWriter : TextWriter
    {
        private readonly IFactorioModificationOutput _output;

        public override Encoding Encoding => Encoding.Default;

        public OutputWriter(IFactorioModificationOutput output) => _output = output;

        public override void WriteLine() => _output.AppendLine(string.Empty);

        public override void WriteLine(string value) => _output.AppendLine(value);

        public override void Write(string value) => _output.Append(value);
    }
}