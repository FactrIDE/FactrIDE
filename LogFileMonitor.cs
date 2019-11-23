using Gemini.Modules.Output;

using System;
using System.IO;
using System.Threading;
using System.Timers;

namespace FactrIDE
{
    public class LogWatcher : FileSystemWatcher
    {
        public IOutput Output { get; }
        private System.Timers.Timer Timer { get; }

        public LogWatcher(IOutput output, string path) : base(System.IO.Path.GetDirectoryName(path), System.IO.Path.GetFileName(path))
        {
            Output = output;

            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.Attributes;
            Renamed += LogWatcher_Renamed;

            Timer = new System.Timers.Timer(1000 / 5);
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();
        }

        private long CurrentPosition;
        private DateTime PreviousCreationTime = DateTime.MinValue;
        private DateTime PreviousWriteTime = DateTime.MinValue;

        private int _lockFlag = 0;
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!EnableRaisingEvents)
                return;

            if (Interlocked.CompareExchange(ref _lockFlag, 1, 0) == 0)
            {
                var fileInfo = new FileInfo(System.IO.Path.Combine(Path, Filter));

                var creationTime = fileInfo.CreationTimeUtc;
                if (creationTime > PreviousCreationTime)
                {
                    Output.Clear();
                    CurrentPosition = 0;
                    PreviousWriteTime = DateTime.MinValue;
                    PreviousCreationTime = creationTime;
                }

                var writeTime = fileInfo.LastWriteTimeUtc;
                if (writeTime > PreviousWriteTime)
                {
                    using var stream = new FileStream(System.IO.Path.Combine(Path, Filter), FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete | FileShare.Inheritable);
                    stream.Seek(CurrentPosition, SeekOrigin.Begin);
                    using var reader = new StreamReader(stream);
                    string line;
                    while ((line = reader.ReadLine()) != null)
                        Output.AppendLine(line);
                    CurrentPosition = stream.Position;
                    PreviousWriteTime = writeTime;
                }

                Interlocked.Decrement(ref _lockFlag);
            }
        }

        private void LogWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            Output.Clear();
            CurrentPosition = 0;
            PreviousCreationTime = DateTime.MinValue;
            PreviousWriteTime = DateTime.MinValue;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Timer?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}