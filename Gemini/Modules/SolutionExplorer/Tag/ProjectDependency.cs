using FactrIDE.Gemini.Modules.SolutionExplorer.Providers;

using Newtonsoft.Json;

using PCLExt.FileStorage;

using System;
using System.IO;
using System.IO.Compression;

namespace FactrIDE.Gemini.Modules.SolutionExplorer
{
    public class ProjectDependency : BaseFile
    {
        public FactorioProjectData Data { get; }

        public ProjectDependency(IFile factorioDependency) : base(factorioDependency)
        {
            using var archive = ZipFile.OpenRead(Path);
            foreach (var entry in archive.Entries)
            {
                if (entry.Name.Equals("info.json", StringComparison.OrdinalIgnoreCase))
                {
                    var serializer = new JsonSerializer();
                    using var sr = new StreamReader(entry.Open());
                    using var jsonTextReader = new JsonTextReader(sr);
                    Data = serializer.Deserialize<FactorioProjectData>(jsonTextReader);
                    break;
                }
            }
        }

        public override string ToString() => Data.Name;
    }
}