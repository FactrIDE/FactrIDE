using Gemini.Modules.Output;

using System.Diagnostics;

namespace FactrIDE.Gemini.Modules.FactorioStandardOutput
{
    public interface IFactorioStandardOutput : IOutput
    {
        void SetOutput(Process process);
    }
}