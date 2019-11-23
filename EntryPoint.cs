using FactrIDE.Properties;
using FactrIDE.Storage.Files;

using Firebase.Storage;

using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;

namespace FactrIDE
{
    public static class EntryPoint
    {
        [STAThread]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancy", "RCS1163:Unused parameter.", Justification = "<Pending>")]
        public static void Main(string[] args)
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += CatchException;

                /*
                if (Properties.Settings.Default.RunFactorioViaSteam)
                {
                    if (Steamworks.SteamAPI.Init())
                    {
                        LogManager.WriteLine("SteamAPI.Init() success.");
                    }
                    else
                    {
                        LogManager.WriteLine("SteamAPI.Init() failure.");
                    }
                }
                */

                var app = new App();
                app.InitializeComponent();
                app.Run();
            }
            catch (Exception e) when (e is Exception)
            {
#if DEBUG
                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(e).Throw();
                //throw e;
#else
                CatchException(e);
#endif
            }
            finally
            {
                AppDomain.CurrentDomain.UnhandledException -= CatchException;
            }
        }

        private static void CatchException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception ?? new NotSupportedException($"Unhandled exception doesn't derive from System.Exception: {e.ExceptionObject}");
            CatchError(exception);
        }
        private static void CatchException(Exception exception)
        {
            var exceptionText = CatchError(UnwrapCompositionException(exception));
            ReportErrorLocal(exceptionText);
            ReportErrorWeb(exceptionText);
        }
        public static Exception UnwrapCompositionException(Exception exception)
        {
            if (exception is CompositionException compositionException)
            {
                var unwrapped = compositionException;
                while (unwrapped != null)
                {
                    var firstError = unwrapped.Errors.FirstOrDefault();
                    if (firstError == null)
                    {
                        break;
                    }

                    var currentException = firstError.Exception;
                    if (currentException == null)
                    {
                        break;
                    }

                    if (currentException is ComposablePartException composablePartException && composablePartException.InnerException != null)
                    {
                        if (composablePartException.InnerException is CompositionException innerCompositionException)
                        {
                            currentException = innerCompositionException;
                        }
                        else
                        {
                            return currentException.InnerException ?? exception;
                        }
                    }

                    unwrapped = currentException as CompositionException;
                }

                return exception;
            }

            return exception;
        }

        private static string CatchError(Exception ex)
        {
            var osInfo = SystemInfoLibrary.OperatingSystem.OperatingSystemInfo.GetOperatingSystemInfo();

            return
$@"{Assembly.GetExecutingAssembly().GetName().Name} Crash Log v {Assembly.GetExecutingAssembly().GetName().Version}
Software:
    OS: {osInfo.Name} {osInfo.Architecture}
    Language: {CultureInfo.CurrentCulture.EnglishName}
    Framework:
        CompiledWith: {Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName}
        Runtime: {osInfo.Runtime}
Hardware:
{RecursiveCPU(osInfo.Hardware.CPUs, 0)}
{RecursiveGPU(osInfo.Hardware.GPUs, 0)}
    RAM:
        Memory Total: {osInfo.Hardware.RAM.Total} KB
        Memory Free: {osInfo.Hardware.RAM.Free} KB
{RecursiveException(ex)}
You should report this error if it is reproduceable or you could not solve it by yourself.
Go To: REPORTURL to report this crash there.";
        }
        private static string RecursiveCPU(System.Collections.Generic.IList<SystemInfoLibrary.Hardware.CPU.CPUInfo> cpus, int index)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(
                $@"    CPU{index}:
        Name: {cpus[index].Name}
        Brand: {cpus[index].Brand}
        Architecture: {cpus[index].Architecture}
        Physical Cores: {cpus[index].PhysicalCores}
        Logical Cores: {cpus[index].LogicalCores}");

            if (index + 1 < cpus.Count)
                sb.AppendFormat(RecursiveCPU(cpus, ++index));

            return sb.ToString();
        }
        private static string RecursiveGPU(System.Collections.Generic.IList<SystemInfoLibrary.Hardware.GPU.GPUInfo> gpus, int index)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(
                $@"    GPU{index}:
        Name: {gpus[index].Name}
        Brand: {gpus[index].Brand}
        Memory Total: {gpus[index].MemoryTotal} KB");

            if (index + 1 < gpus.Count)
                sb.AppendFormat(RecursiveGPU(gpus, ++index));

            return sb.ToString();
        }
        private static string RecursiveException(Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(
                $@"Exception information:
Type: {ex.GetType().FullName}
Message: {ex.Message}
HelpLink: {(string.IsNullOrWhiteSpace(ex.HelpLink) ? "Empty" : ex.HelpLink)}
Source: {ex.Source}
TargetSite : {ex.TargetSite}
CallStack:
{ex.StackTrace}");

            if (ex.InnerException != null)
            {
                sb.AppendFormat($@"
--------------------------------------------------
InnerException:
{RecursiveException(ex.InnerException)}");
            }

            return sb.ToString();
        }

        private static void ReportErrorLocal(string exception)
        {
            using var stream = new CrashLogFile().Open(PCLExt.FileStorage.FileAccess.ReadAndWrite);
            using var writer = new StreamWriter(stream);
            writer.Write(exception);
        }
        private static void ReportErrorWeb(string exception)
        {
            if (Settings.Default.ReportToWeb)
            {
                try
                {
                    using var cts = new CancellationTokenSource(2000);
                    using var ms = new MemoryStream(Encoding.UTF8.GetBytes(exception));
                    new FirebaseStorage("factride-66653.appspot.com")
                        .Child($"CrashLogs")
                        .Child($"{Assembly.GetExecutingAssembly().GetName().Name} {Assembly.GetExecutingAssembly().GetName().Version}")
                        .Child($"{DateTime.UtcNow:yyyy-MM-dd_HH.mm.ss}.log")
                        .PutAsync(ms, cts.Token, "text/plain").GetAwaiter().GetResult();
                }
                catch (Exception e) when (e is FirebaseStorageException) { }
            }
        }
    }
}