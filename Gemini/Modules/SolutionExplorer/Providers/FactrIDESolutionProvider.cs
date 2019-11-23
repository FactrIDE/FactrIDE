using FactrIDE.Gemini.Modules.SolutionExplorer.Extensions;

using Idealde.Framework.Projects;

using Newtonsoft.Json;

using PCLExt.FileStorage;
using PCLExt.FileStorage.Extensions;
using PCLExt.FileStorage.Folders;

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Providers
{
    [Export(typeof(ISolutionProvider))]
    [Export(typeof(FactrIDESolutionProvider))]
    public class FactrIDESolutionProvider : SolutionProviderBase
    {
        public override string Name => "FactrIDE Solution";

        public override IEnumerable<ExtensionType> ExtensionTypes
        {
            get { yield return new ExtensionType("FactrIDE Solution", ".fms"); }
        }

        public override async Task<IFile> Create(SolutionDataBase solutionData, IFolder solutionFolder)
        {
            foreach (var project in solutionData.Projects)
            {
                var projectProvider = new FactorioProjectProvider();
                await projectProvider.Create(new FactorioProjectData() { Name = project.Name }, new FolderFromPath(solutionFolder.Path, project.Path)).ConfigureAwait(false);
            }

            var solutionFile = await solutionFolder.CreateFileAsync(solutionData.Name + ExtensionTypes.First().Extension, CreationCollisionOption.ReplaceExisting).ConfigureAwait(false);
            var solutionFileContent = JsonConvert.SerializeObject(solutionData, Formatting.Indented);
            await solutionFile.WriteAllTextAsync(solutionFileContent).ConfigureAwait(false);
            return solutionFile;
        }

        public override async Task<SolutionInfoBase> Load(IFile solutionFile)
        {
            if (!solutionFile.Exists || !ValidateExtension(solutionFile))
                return null;

            var solutionFolder = new FolderFromPath(Path.GetDirectoryName(solutionFile.Path));
            var solutionDataContent = await solutionFile.ReadAllTextAsync().ConfigureAwait(false);
            var solutionData = JsonConvert.DeserializeObject<SolutionDataBase>(solutionDataContent);

            return new FactrIDESolutionInfo(solutionFolder, solutionData, this);
        }

        public override async Task<IFile> Save(SolutionInfoBase info, IFolder folder)
        {
            if (info is FactrIDESolutionInfo solutionInfo)
            {
                var data = new SolutionDataBase() { Name = solutionInfo.Name };
                foreach (var projectInfo in solutionInfo.Projects.OfType<FactorioProjectInfo>())
                {
                    var projectFolderPathUri = new Uri(projectInfo.Folder.Path.GetFullPathWithEndingSlashes(), UriKind.Absolute);
                    var solutionFolderPathUri = new Uri(solutionInfo.Folder.Path.GetFullPathWithEndingSlashes(), UriKind.Absolute);
                    string projectPath;
                    // Try to get relative path
                    try { projectPath = solutionFolderPathUri.MakeRelativeUri(projectFolderPathUri).ToString(); }
                    catch (Exception e) when (e is InvalidOperationException) { projectPath = projectInfo.Folder.Path; }

                    data.Projects.Add(new ProjectData
                    {
                        Name = projectInfo.Name,
                        Path = projectPath,
                    });
                }

                var solutionFileContent = JsonConvert.SerializeObject(data, Formatting.Indented);
                var solutionFile = await folder.CreateFileAsync(info.Name + ExtensionTypes.First().Extension, CreationCollisionOption.OpenIfExists).ConfigureAwait(false);
                await solutionFile.WriteAllTextAsync(solutionFileContent).ConfigureAwait(false);

                var timeToLives = 2000;
                while (timeToLives > 0 && !solutionFile.Exists)
                {
                    await Task.Delay(25).ConfigureAwait(false);
                    timeToLives -= 25;
                }

                return solutionFile;
            }

            return new NonExistingFile("");
        }
    }
}