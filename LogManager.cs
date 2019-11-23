using FactrIDE.Storage.Files;

using System;
using System.IO;

namespace FactrIDE
{
    internal static class LogManager
    {
        private static LogFile LogFile { get; } = new LogFile();

        public static void WriteLine(string message)
        {
            lock (LogFile)
            {
                using var stream = LogFile.Open(PCLExt.FileStorage.FileAccess.ReadAndWrite);
                using var writer = new StreamWriter(stream);
                writer.BaseStream.Seek(0, SeekOrigin.End);
                writer.WriteLine($"{DateTime.Now:yyyy-MM-dd_HH.mm.ss}:{message}");
                writer.Flush();
            }
        }
    }
}