using PCLExt.FileStorage;

namespace FactrIDE.Gemini.Modules.SolutionExplorer
{
    public interface IProjectEntity
    {
        IFolder Project { get; }

        string ProjectLocalPath { get; }
    }
}