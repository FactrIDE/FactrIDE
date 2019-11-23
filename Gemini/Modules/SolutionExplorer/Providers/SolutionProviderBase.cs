using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Idealde.Framework.Projects;

using PCLExt.FileStorage;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Providers
{
    public abstract class SolutionProviderBase : ISolutionProvider
    {
        public abstract string Name { get; }

        public abstract IEnumerable<ExtensionType> ExtensionTypes { get; }

        public abstract Task<IFile> Create(SolutionDataBase solutionData, IFolder folder);
        public abstract Task<SolutionInfoBase> Load(IFile solutionFile);
        public abstract Task<IFile> Save(SolutionInfoBase info, IFolder folder);

        [DebuggerStepThrough]
        protected bool ValidateExtension(IFile file)
        {
            var extension = Path.GetExtension(file.Name);
            foreach (var solutionType in ExtensionTypes)
            {
                if (string.Equals(solutionType.Extension, extension, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}