using PCLExt.FileStorage;
using PCLExt.FileStorage.Files;
using PCLExt.FileStorage.Folders;

using System.IO;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Providers
{
    public class FactrIDESolutionInfo : SolutionInfoBase
    {
        public override string Name { get => Data.Name; set => Data.Name = value; }
        protected SolutionDataBase Data { get; }

        public FactrIDESolutionInfo(IFolder solutionFolder, SolutionDataBase solutionData, ISolutionProvider provider = null) : base(solutionFolder, provider ?? new FactrIDESolutionProvider())
        {
            Data = solutionData;

            var projectProvider = new FactorioProjectProvider();
            foreach (var project in Data.Projects)
            {
                if (Path.IsPathRooted(project.Path))
                    Projects.Add(projectProvider.Load(new FileFromPath(Path.Combine(project.Path, "info.json"))));
                else
                    Projects.Add(projectProvider.Load(new FileFromPath(Path.Combine(Folder.Path, project.Path, "info.json"))));
            }
        }

        public override async Task Create()
        {
            foreach (var project in Data.Projects)
            {
                var projectFolder = Path.IsPathRooted(project.Path)
                    ? new FolderFromPath(Path.Combine(project.Path), true)
                    : new FolderFromPath(Path.Combine(Folder.Path, project.Path), true);
                var projectFile = projectFolder.CreateFile("info.json", CreationCollisionOption.FailIfExists);

                var projectInfo = new FactorioProjectInfo(projectFolder, projectFile);

                await projectInfo.Create().ConfigureAwait(false);

                Projects.Add(projectInfo);
            }
        }
    }
}