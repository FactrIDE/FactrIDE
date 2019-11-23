using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FactrIDE
{
    public class ExtendedFileSystemWatcher : FileSystemWatcher
    {
        public event Func<FileSystemEventArgs, bool> HandleCreated;
        public event Func<FileSystemEventArgs, bool> HandleDeleted;
        public event Func<FileSystemEventArgs, bool> HandleChanged;
        public event Func<RenamedEventArgs, bool> HandleRenamed;
        //public event Func<ErrorEventArgs, bool> HandleError;

        public event Action<bool> GroupingCompleted;

        private readonly MemoryCache DeferredUpdater = new MemoryCache(new MemoryCacheOptions());
        private readonly int CacheTimeMilliseconds; // All actions with files/folders within <CacheTimeMilliseconds> ms will be grouped into one call

        public ExtendedFileSystemWatcher(string path, int cacheTimeMilliseconds = 500) : base(path)
        {
            CacheTimeMilliseconds = cacheTimeMilliseconds;
            IncludeSubdirectories = true;
            EnableRaisingEvents = true;
            InternalBufferSize = ushort.MaxValue;
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.CreationTime | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size;
            Created += FileSystemWatcher_Created;
            Renamed += FileSystemWatcher_Renamed;
            Deleted += FileSystemWatcher_Deleted;
            Changed += FileSystemWatcher_Changed;
        }
        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs args)
        {
            var actions = GetActions();
            if (actions.ContainsKey(args.FullPath)) return; // Prevent duplication
            actions.Add(args.FullPath, new Queue<Func<bool>>());
            actions[args.FullPath].Enqueue(() => HandleChanged?.Invoke(args) == true);
        }
        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs args)
        {
            var actions = GetActions();
            actions["Unified"].Enqueue(() => HandleCreated?.Invoke(args) == true);
        }
        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs args)
        {
            var actions = GetActions();
            actions["Unified"].Enqueue(() => HandleDeleted?.Invoke(args) == true);
        }
        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs args)
        {
            var actions = GetActions();
            actions["Unified"].Enqueue(() => HandleRenamed?.Invoke(args) == true);
        }
        public void TriggerCustom()
        {
            var actions = GetActions();
            actions["Custom"].Enqueue(() => true);
        }

        private readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1);
        private IDictionary<string, Queue<Func<bool>>> GetActions()
        {
            _cacheLock.Wait();
            var value = DeferredUpdater.GetOrCreate("Dictionary_Actions", e =>
            {
                e.AddExpirationToken(new CancellationChangeToken(new CancellationTokenSource(CacheTimeMilliseconds).Token));
                e.RegisterPostEvictionCallback(PostEvictionCallback);
                return new Dictionary<string, Queue<Func<bool>>>() { { "Unified", new Queue<Func<bool>>() }, { "Custom", new Queue<Func<bool>>() } };
            });
            _cacheLock.Release();
            return value;
        }

        private void PostEvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            if (reason != EvictionReason.TokenExpired) return;

            if (value is IDictionary<string, Queue<Func<bool>>> dictionary)
            {
                var projectChanged = false;
                foreach (var keyValue in dictionary)
                {
                    var actions = keyValue.Value;
                    while (actions.Count > 0)
                    {
                        var action = actions.Dequeue();
                        if (!projectChanged)
                            projectChanged = action?.Invoke() == true;
                        else
                            action?.Invoke();
                    }
                }
                GroupingCompleted?.Invoke(projectChanged);
            }
        }
    }
}