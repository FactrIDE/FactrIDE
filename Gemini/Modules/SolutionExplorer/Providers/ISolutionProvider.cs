using Idealde.Framework.Projects;

using PCLExt.FileStorage;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Providers
{
    public interface ISolutionProvider
    {
        string Name { get; }

        IEnumerable<ExtensionType> ExtensionTypes { get; }

        Task<IFile> Create(SolutionDataBase solutionData, IFolder folder);
        Task<SolutionInfoBase> Load(IFile solutionFile);
        Task<IFile> Save(SolutionInfoBase info, IFolder folder);
    }
}