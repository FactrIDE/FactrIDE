using FactrIDE.Gemini.Modules.Factorio.Commands;
using FactrIDE.Gemini.Modules.SolutionExplorer;
using FactrIDE.Properties;
using FactrIDE.Storage.Folders;
using Gemini.Framework.Commands;

using PCLExt.FileStorage.Files;
using PCLExt.FileStorage.Folders;

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.ViewModels
{
    public partial class SolutionExplorerViewModel
        //: ICommandHandler<RunFactorioCommandDefinition>
        //, ICommandHandler<StopFactorioCommandDefinition>
    {
        /*
        void ICommandHandler<RunFactorioCommandDefinition>.Update(Command command)
        {
            if(SolutionInfo == null)
            {
                command.Enabled = false;
                return;
            }
            command.Enabled = Process.GetProcessesByName("factorio").Length == 0;
        }
        async Task ICommandHandler<RunFactorioCommandDefinition>.Run(Command command)
        {
            await ArchiveProjects();

            if (Settings.Default.RunFactorioViaSteam)
            {
                var process = Process.Start(new ProcessStartInfo("steam://rungameid/427520")
                {
                    UseShellExecute = true,
                    Verb = "open"
                });
                process.EnableRaisingEvents = true;
                process.Exited += (s, e) => CommandManager.InvalidateRequerySuggested();

                Shell.ShowTool(LogOutput);
            }
            else
            {
                var processStartInfo = new ProcessStartInfo("CMD.exe", $@"/c ""{Settings.Default.FactorioExecutableFilePath}""")
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                };
                var process = Process.Start(processStartInfo);
                process.EnableRaisingEvents = true;
                process.Exited += (s, e) => CommandManager.InvalidateRequerySuggested();

                StandardOutput.SetOutput(process);
                Shell.ShowTool(StandardOutput);
            }
        }
        */

        /*
        void ICommandHandler<StopFactorioCommandDefinition>.Update(Command command)
        {
            if (SolutionInfo == null)
            {
                command.Enabled = false;
                return;
            }
            command.Enabled = Process.GetProcessesByName("factorio").Length >= 1;
        }
        Task ICommandHandler<StopFactorioCommandDefinition>.Run(Command command)
        {
            var process = Process.GetProcessesByName("factorio").SingleOrDefault();
            process.EnableRaisingEvents = true;
            process.Exited += (s, e) => CommandManager.InvalidateRequerySuggested();
            process.CloseMainWindow();

            return Task.CompletedTask;
        }
        */


        // Either archive every project or the current selected?
        public async void Run()
        {
            foreach (var project in SolutionInfo.Projects)
            {
                var folderName = $"{project.Name}_{project.Version}";

                var files = Directory.EnumerateFiles(project.Folder.Path, "*", SearchOption.AllDirectories)
                    .Where(s =>
                        s.EndsWith(".json") ||
                        s.EndsWith(".txt") ||
                        s.EndsWith(".md") ||
                        s.EndsWith(".lua") ||
                        s.EndsWith(".cfg") ||
                        s.EndsWith(".png") ||
                        s.EndsWith(".ogg") ||
                        !Path.HasExtension(s))
                    .Select(f => new ProjectFile(project.Folder, new FileFromPath(f))).ToList();

                //var modsFolder = new FolderFromPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)), "Factorio", "mods");
                var modsFolder = new FactorioModsFolder();
                var modificationFile = await modsFolder.CreateFileAsync($"{folderName}.zip", PCLExt.FileStorage.CreationCollisionOption.ReplaceExisting).ConfigureAwait(false);
                using var archive = new ZipArchive(await modificationFile.OpenAsync(PCLExt.FileStorage.FileAccess.ReadAndWrite).ConfigureAwait(false), ZipArchiveMode.Create, false);
                foreach (var file in files)
                {
                    var entry = archive.CreateEntryFromFile(file.Path,
                        Path.Combine(folderName, file.ProjectLocalPath)
                        // Use linux paths for cross-platform support
                        .Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                        );
                }
            }
        }
    }
}