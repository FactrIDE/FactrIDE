using Gemini.Modules.Output;

namespace FactrIDE.Gemini.Modules.FactorioModificationOutput
{
    public interface IFactorioModificationOutput : IOutput
    {
        void SetFilePath(string path);
    }
}