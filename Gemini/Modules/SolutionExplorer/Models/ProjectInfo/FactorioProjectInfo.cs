using FactrIDE.Gemini.Modules.SolutionExplorer.Extensions;

using Idealde.Framework.Projects;

using Newtonsoft.Json;

using PCLExt.FileStorage;
using PCLExt.FileStorage.Extensions;
using PCLExt.FileStorage.Files;
using PCLExt.FileStorage.Folders;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Providers
{
    public class FactorioProjectInfo : ProjectInfoBase
    {
        public override event Action OnProjectChange;

        public FactorioProjectData Data { get; set; }
        public override string Name { get => Data.Name; set => Data.Name = value; }
        public override Version Version { get => Data.Version; set => Data.Version = value; }

        private ExtendedFileSystemWatcher ExtendedFileSystemWatcher { get; }
        private ManualResetEventSlim SavingLock { get; } = new ManualResetEventSlim(true);
        private ManualResetEventSlim ReloadLock { get; } = new ManualResetEventSlim(true);

        public FactorioProjectInfo(IFolder projectFolder, IFile projectFile, FactorioProjectProvider provider = null) : base(projectFolder, provider ?? new FactorioProjectProvider())
        {
            File = projectFile;
            Reload();

            var files = Directory.GetFiles(projectFolder.Path, "*", SearchOption.AllDirectories);
            foreach (var file in files
                .Where(@if => IgnoreFiles.All(f => f.ProjectLocalPath != @if))
                .Select(f => new ProjectFile(projectFolder, new FileFromPath(f.NormalizePath()))))
            {
                Files.Add(file);
            }
            var projectFileIgnore = Files.SingleOrDefault(f => f.Name.Equals("info.json", StringComparison.OrdinalIgnoreCase));
            if (projectFileIgnore != null)
                Files.Remove(projectFileIgnore);
            var solutionFile = Files.SingleOrDefault(f => Path.GetExtension(f.Name).Equals(".fms", StringComparison.OrdinalIgnoreCase));
            if(solutionFile != null)
                Files.Remove(solutionFile);

            var directories = Directory.GetDirectories(projectFolder.Path, "*", SearchOption.AllDirectories);
            foreach (var directory in directories
                .Where(@id => IgnoreFolders.All(f => f.ProjectLocalPath != @id))
                .Select(d => new ProjectFolder(projectFolder, new FolderFromPath(d.NormalizePath().GetFullPathWithEndingSlashes()))))
            {
                Folders.Add(directory);
            }

            foreach (var dependency in Data.Dependencies.Select(d => (DependencyBase) new FactorioModificationDependency(d)))
                Dependencies.Add(dependency);

            ExtendedFileSystemWatcher = new ExtendedFileSystemWatcher(Folder.Path);
            ExtendedFileSystemWatcher.HandleCreated += ExtendedFileSystemWatcher_HandleCreated;
            ExtendedFileSystemWatcher.HandleDeleted += ExtendedFileSystemWatcher_HandleDeleted;
            ExtendedFileSystemWatcher.HandleRenamed += ExtendedFileSystemWatcher_HandleRenamed;
            ExtendedFileSystemWatcher.HandleChanged += ExtendedFileSystemWatcher_HandleChanged;
            ExtendedFileSystemWatcher.GroupingCompleted += ExtendedFileSystemWatcher_GroupingCompleted;

            IgnoreFiles.CollectionChanged += CollectionChanged1;
            IgnoreFolders.CollectionChanged += CollectionChanged1;
            Dependencies.CollectionChanged += CollectionChanged1;
        }
        private void CollectionChanged1(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ReloadLock.IsSet)
            {
                // As ExtendedFileSystemWatcher is grouping events within n ms, we can use it to trigger non fs
                // related changes to the project too, so it will refresh only once.
                // WIth this workaround we don't need a similar implementation in this class, too
                ExtendedFileSystemWatcher.TriggerCustom();
            }
        }

        private void ExtendedFileSystemWatcher_GroupingCompleted(bool projectChanged)
        {
            if (projectChanged)
                OnProjectChange?.Invoke();
        }

        private bool ExtendedFileSystemWatcher_HandleCreated(FileSystemEventArgs args)
        {
            return true;
            /*
            var attr = System.IO.File.GetAttributes(args.FullPath);
            if ((attr & FileAttributes.Directory) != 0)
            {
                Folders.Add(new ProjectFolder(Folder, new FolderFromPath(args.FullPath)));

                var fullPath = args.FullPath.NormalizePath();
                foreach (var file in Directory.GetFiles(fullPath, "*", SearchOption.AllDirectories))
                    Files.Add(new ProjectFile(Folder, new FileFromPath(file)));
                foreach (var folder in Directory.GetDirectories(fullPath, "*", SearchOption.AllDirectories))
                    Folders.Add(new ProjectFolder(Folder, new FolderFromPath(folder)));
            }
            else
                Files.Add(new ProjectFile(Folder, new FileFromPath(args.FullPath)));

            return true;
            */
        }
        private bool ExtendedFileSystemWatcher_HandleDeleted(FileSystemEventArgs args)
        {
            return true;
            /*
            var fullPath = args.FullPath.NormalizePath();
            var triggerChange = false;

            var rootFolder = Folders.SingleOrDefault(f => f.Path.NormalizePath().Equals(fullPath, StringComparison.InvariantCulture));
            if (rootFolder != null) // Is Folder
            {
                var tFiles = GetPathsInFolder(fullPath.GetFullPathWithEndingSlashes(), Files.Select(f => f.Path));
                var tFolders = GetPathsInFolder(fullPath.GetFullPathWithEndingSlashes(), Folders.Select(f => f.Path));

                foreach (var folderPath in GetPathsInFolder(fullPath.GetFullPathWithEndingSlashes(), Folders.Select(f => f.Path)))
                {
                    var folder = Folders.SingleOrDefault(f => f.Path.NormalizePath().Equals(folderPath, StringComparison.InvariantCulture));
                    Folders.Remove(folder);
                }
                foreach (var filePath in GetPathsInFolder(fullPath.GetFullPathWithEndingSlashes(), Files.Select(f => f.Path)))
                {
                    var file = Files.SingleOrDefault(f => f.Path.NormalizePath().Equals(filePath, StringComparison.InvariantCulture));
                    Files.Remove(file);
                }

                Folders.Remove(rootFolder);
                triggerChange = true;
            }

            var rootFile = Files.SingleOrDefault(f => f.Path.NormalizePath().Equals(fullPath, StringComparison.InvariantCulture));
            if (rootFile != null) // is File
            {
                Files.Remove(rootFile);
                triggerChange = true;
            }

            var ignoreFile = IgnoreFiles.SingleOrDefault(f => f.Path.NormalizePath().Equals(fullPath, StringComparison.InvariantCulture));
            if (ignoreFile != null)
                IgnoreFiles.Remove(ignoreFile);
            var ignoreFolder = IgnoreFolders.SingleOrDefault(f => f.Path.NormalizePath().Equals(fullPath, StringComparison.InvariantCulture));
            if (ignoreFolder != null)
                IgnoreFolders.Remove(ignoreFolder);

            return triggerChange;
            */
        }
        private bool ExtendedFileSystemWatcher_HandleRenamed(RenamedEventArgs args)
        {
            return true;
            /*
            var attr = System.IO.File.GetAttributes(args.FullPath);
            if ((attr & FileAttributes.Directory) != 0)
            {
                // Root Folder
                var oldRootFolder = Folders.SingleOrDefault(f => f.Path.NormalizePath().Equals(args.OldFullPath, StringComparison.InvariantCulture));
                if (oldRootFolder != null)
                {
                    Folders.Remove(oldRootFolder);
                    Folders.Add(new ProjectFolder(Folder, new FolderFromPath(args.FullPath)));
                }
                var oldRootIgnoreFolder = IgnoreFolders.SingleOrDefault(f => f.Path.NormalizePath().Equals(args.OldFullPath, StringComparison.InvariantCulture));
                if (oldRootIgnoreFolder != null)
                {
                    IgnoreFolders.Remove(oldRootIgnoreFolder);
                    IgnoreFolders.Add(new ProjectFolder(Folder, new FolderFromPath(args.FullPath)));
                }
                // Root Folder


                // Root Folder Children
                var fullPath = args.FullPath.NormalizePath().GetFullPathWithEndingSlashes();
                var filesDict = GetOldPaths(fullPath, args.OldName, Directory.GetFiles(fullPath, "*", SearchOption.AllDirectories));
                var foldersDict = GetOldPaths(fullPath, args.OldName, Directory.GetDirectories(fullPath, "*", SearchOption.AllDirectories));

                foreach (var file in filesDict)
                {
                    var oldFilePath = file.Value.NormalizePath();
                    var newFilePath = file.Key.NormalizePath();

                    var oldFile = Files.SingleOrDefault(f => f.Path.NormalizePath().Equals(oldFilePath, StringComparison.InvariantCulture));
                    if (oldFile != null)
                    {
                        Files.Remove(oldFile);
                        Files.Add(new ProjectFile(Folder, new FileFromPath(newFilePath)));
                    }
                    var oldIgnoreFile = IgnoreFiles.SingleOrDefault(f => f.Path.NormalizePath().Equals(oldFilePath, StringComparison.InvariantCulture));
                    if (oldIgnoreFile != null)
                    {
                        IgnoreFiles.Remove(oldIgnoreFile);
                        IgnoreFiles.Add(new ProjectFile(Folder, new FileFromPath(newFilePath)));
                    }
                }

                foreach (var folder in foldersDict)
                {
                    var oldFolderPath = folder.Value;
                    var newFolderPath = folder.Key;

                    var oldFolder = Folders.SingleOrDefault(f => f.Path.NormalizePath().Equals(oldFolderPath, StringComparison.InvariantCulture));
                    if (oldFolder != null)
                    {
                        Folders.Remove(oldFolder);
                        Folders.Add(new ProjectFolder(Folder, new FolderFromPath(newFolderPath)));
                    }
                    var oldIgnoreFolder = IgnoreFolders.SingleOrDefault(f => f.Path.NormalizePath().Equals(oldFolderPath, StringComparison.InvariantCulture));
                    if (oldIgnoreFolder != null)
                    {
                        IgnoreFolders.Remove(oldIgnoreFolder);
                        IgnoreFolders.Add(new ProjectFolder(Folder, new FolderFromPath(newFolderPath)));
                    }
                }
                // Root Folder Children
            }
            else
            {
                var oldFile = Files.SingleOrDefault(f => f.Path.NormalizePath().Equals(args.OldFullPath, StringComparison.InvariantCulture));
                if (oldFile != null)
                {
                    Files.Remove(oldFile);
                    Files.Add(new ProjectFile(Folder, new FileFromPath(args.FullPath)));
                }
                var oldIgnoreFile = IgnoreFiles.SingleOrDefault(f => f.Path.NormalizePath().Equals(args.OldFullPath, StringComparison.InvariantCulture));
                if (oldIgnoreFile != null)
                {
                    IgnoreFiles.Remove(oldIgnoreFile);
                    IgnoreFiles.Add(new ProjectFile(Folder, new FileFromPath(args.FullPath)));
                }
            }
            return true;
            */
        }
        private bool ExtendedFileSystemWatcher_HandleChanged(FileSystemEventArgs args)
        {
            if (SavingLock.IsSet && args.Name.Equals("info.json", StringComparison.InvariantCulture))
            {
                Reload();
                return true;
            }
            return false;
        }

        protected override void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ReloadLock.IsSet)
            {
                base.CollectionChanged(sender, e);
            }
        }

        public void Reload()
        {
            ReloadLock.Wait();
            ReloadLock.Reset();
            Data = JsonConvert.DeserializeObject<FactorioProjectData>(File.ReadAllText()) ?? new FactorioProjectData();
            Data.PropertyChanged += (s, e) =>
            {
                if (SavingLock.IsSet)
                {
                    HasChanges = true;
                }
            };

            IgnoreFiles.Clear();
            foreach (var fileToIgnore in Data.FilesToIgnore.Select(f => new ProjectFile(Folder, f)))
                IgnoreFiles.Add(fileToIgnore);

            IgnoreFolders.Clear();
            foreach (var folderToIgnore in Data.FoldersToIgnore.Select(d => new ProjectFolder(Folder, d)))
                IgnoreFolders.Add(folderToIgnore);
            ReloadLock.Set();
        }

        public override async Task Create()
        {
            await Provider.Create(Data, Folder).ConfigureAwait(false);
        }
        public override async Task Save()
        {
            SavingLock.Wait();
            SavingLock.Reset();
            Data.Dependencies = Dependencies.OfType<FactorioModificationDependency>().Select(d => d.JsonRaw()).ToList();
            Data.FilesToIgnore = IgnoreFiles.Select(f => f.ProjectLocalPath).ToList();
            Data.FoldersToIgnore = IgnoreFolders.Select(f => f.ProjectLocalPath).ToList();
            await base.Save().ConfigureAwait(false);
            HasChanges = false;
            SavingLock.Set();
        }

        private static IDictionary<string, string> GetOldPaths(string renamedFolder, string oldRenamedFolderName, IEnumerable<string> filesOrFolders)
        {
            var renamedFolderUri = new Uri(renamedFolder, UriKind.Absolute);
            var dict = new Dictionary<string, string>();
            foreach (var file in filesOrFolders)
            {
                var fileUri = new Uri(file, UriKind.Absolute);

                var relativePath = renamedFolderUri.MakeRelativeUri(fileUri).ToString();
                var renamedFolderParent = new DirectoryInfo(renamedFolder).Parent.FullName;
                var oldPath = Path.Combine(renamedFolderParent, oldRenamedFolderName, relativePath);
                dict.Add(file.NormalizePath(), oldPath.NormalizePath());
            }
            return dict;
        }
        private static List<string> GetPathsInFolder(string folder, IEnumerable<string> filesOrFolders)
        {
            var renamedFolderUri = new Uri(folder, UriKind.Absolute);
            var dict = new List<string>();
            foreach (var file in filesOrFolders)
            {
                var fileUri = new Uri(file, UriKind.Absolute);
                var relativePath = renamedFolderUri.MakeRelativeUri(fileUri).ToString();
                if (!relativePath.StartsWith($"..{Path.DirectorySeparatorChar}") && !relativePath.StartsWith($"..{Path.AltDirectorySeparatorChar}"))
                    dict.Add(file);
            }
            return dict;
        }
    }
}