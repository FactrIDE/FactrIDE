using Idealde.Framework.ProjectExplorer.Models;
using Idealde.Framework.Projects;

using Newtonsoft.Json;

using PCLExt.FileStorage;
using PCLExt.FileStorage.Extensions;
using PCLExt.FileStorage.Files;
using PCLExt.FileStorage.Folders;

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Providers
{
    [Export(typeof(IProjectProvider))]
    [Export(typeof(FactorioProjectProvider))]
    public class FactorioProjectProvider : ProjectProviderBase
    {
        public override string Name => "Factorio Modification Project";

        public override IEnumerable<ExtensionType> ExtensionTypes
        {
            get { yield return new ExtensionType("Factorio Modification Info", ".json"); }
        }

        [DebuggerStepThrough]
        private void CreateExtensionFilesIfNotExist(IFolder projectFolder)
        {
            foreach (var childFiles in new[] {
                "control.lua",
                "data-final-fixes.lua",
                "data-updates.lua",
                "data.lua",
                "settings.lua",
                //"thumbnail.png"
            })
            {
                new FileFromPath(Path.Combine(projectFolder.Path, childFiles), true);
            }
        }
        [DebuggerStepThrough]
        private void CreateExtensionDirectoriesIfNotExist(IFolder projectFolder)
        {
            foreach (var childDirectory in new[] {
                "graphics",
                Path.Combine("graphics", "entity"),
                Path.Combine("graphics", "icons"),
                Path.Combine("graphics", "technology"),
                "libs",
                "locale",
                "migrations",
                Path.Combine("locale", "en"),
                "prototypes",
                Path.Combine("prototypes", "buildings"),
                Path.Combine("prototypes", "items"),
                Path.Combine("prototypes", "recipes"),
                Path.Combine("prototypes", "technology"),
                "scenarios",
                "scripts",
                "sound",
                "utils"
            })
            {
                new FolderFromPath(Path.Combine(projectFolder.Path, childDirectory), true);
            }
        }

        public override async Task<IFile> Create(ProjectDataBase projectData, IFolder projectFolder)
        {
            CreateExtensionFilesIfNotExist(projectFolder);
            CreateExtensionDirectoriesIfNotExist(projectFolder);

            var projectFile = await projectFolder.CreateFileAsync($"info{ExtensionTypes.First().Extension}", CreationCollisionOption.ReplaceExisting).ConfigureAwait(false);
            var projectFileContent = JsonConvert.SerializeObject(projectData, Formatting.Indented);
            projectFile.WriteAllText(projectFileContent);
            return projectFile;
        }
        public override ProjectInfoBase Load(IFile projectFile)
        {
            if (!projectFile.Exists || !Path.GetFileName(projectFile.Name).Equals($"info{ExtensionTypes.First().Extension}", StringComparison.OrdinalIgnoreCase))
                return null;

            var projectFolder = new FolderFromPath(Path.GetDirectoryName(projectFile.Path), true);
            return new FactorioProjectInfo(projectFolder, projectFile, this);
        }
        public override Task<ProjectInfoBase> LoadAsync(IFile projectFile) => Task.FromResult(Load(projectFile));

        public override async Task<IFile> Save(ProjectInfoBase info, IFolder projectFolder)
        {
            if (info is FactorioProjectInfo projectInfo)
            {
                var projectFileContent = JsonConvert.SerializeObject(projectInfo.Data, Formatting.Indented);
                var projectFile = projectFolder.GetFile($"info{ExtensionTypes.First().Extension}");
                projectFile.WriteAllText(projectFileContent);

                var timeToLives = 2000;
                while (timeToLives > 0 && !projectFile.Exists)
                {
                    await Task.Delay(25).ConfigureAwait(false);
                    timeToLives -= 25;
                }

                return projectFile;
            }

            return new NonExistingFile("");
        }
    }
}