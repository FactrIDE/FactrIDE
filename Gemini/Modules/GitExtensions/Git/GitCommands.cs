using Microsoft.Win32;

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

using Xceed.Wpf.Toolkit;

namespace FactrIDE.Gemini.Modules.GitExtensions.Git
{
    public static class GitCommands
    {
        private static string GetRegistryValue(RegistryKey root, string subkey, string key)
        {
            try
            {
                var rk = root.OpenSubKey(subkey, false);

                var value = string.Empty;

                if (rk?.GetValue(key) is string str)
                {
                    value = str;
                    rk.Flush();
                    rk.Close();
                }

                return value;
            }
            catch (Exception e) when (e is UnauthorizedAccessException)
            {
                MessageBox.Show("GitExtensions has insufficient permissions to check the registry.");
            }

            return "";
        }

        private static string GetGitExRegValue(string key)
        {
            var result = GetRegistryValue(Registry.CurrentUser, "Software\\GitExtensions", key);

            if (string.IsNullOrEmpty(result))
            {
                result = GetRegistryValue(Registry.Users, "Software\\GitExtensions", key);
            }

            return result;
        }

        private static ProcessStartInfo CreateStartInfo(string command, string arguments, string workingDir, Encoding encoding = null) => new ProcessStartInfo
        {
            UseShellExecute = false,
            ErrorDialog = false,
            CreateNoWindow = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = encoding,
            StandardErrorEncoding = encoding,
            FileName = command,
            Arguments = arguments,
            WorkingDirectory = workingDir
        };

        public static Process RunGitEx(string command, string filename, string[] arguments = null)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                if (filename.EndsWith("\\"))
                {
                    // Escape the final backslash to avoid escaping the quote.
                    // This is a problem for drive roots on Windows, such as "C:\".
                    filename += "\\";
                }

                command += $" \"{filename}\"";
            }

            if (arguments?.Length > 0)
            {
                command += $" {string.Join(" ", arguments)}";
            }

            var path = GetGitExRegValue("InstallDir");
            var workDir = Path.GetDirectoryName(filename);
            var startInfo = CreateStartInfo(Path.Combine(path, "GitExtensions.exe"), command, workDir, Encoding.UTF8);

            try
            {
                return Process.Start(startInfo);
            }
            catch (Exception e) when (e is Exception)
            {
                if (!File.Exists(Path.Combine(path, "GitExtensions.exe")))
                {
                    MessageBox.Show("This plugin requires Git Extensions to be installed. This application can be downloaded from http://gitextensions.github.io/");
                }

                return null;
            }
        }

        public static string RunGitExWait(string command, string filename)
        {
            using var process = RunGitEx(command, filename);
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

        private static string RunGit(string arguments, string filename, out int exitCode)
        {
            var gitcommand = GetGitExRegValue("gitcommand");

            var startInfo = CreateStartInfo(gitcommand, arguments, filename);

            using var process = Process.Start(startInfo);
            var output = process.StandardOutput.ReadToEnd();
            exitCode = process.ExitCode;
            process.WaitForExit();
            return output;
        }

        public static string GetCurrentBranch(string fileName)
        {
            try
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    string head;
                    string headFileName = $"{FindGitWorkingDir(fileName)}.git\\HEAD";
                    if (File.Exists(headFileName))
                    {
                        head = File.ReadAllText(headFileName);
                        if (!head.Contains("ref:"))
                        {
                            head = "no branch";
                        }
                    }
                    else
                    {
                        head = RunGit("symbolic-ref HEAD", new FileInfo(fileName).DirectoryName, out var exitCode);
                        if (exitCode == 1)
                        {
                            head = "no branch";
                        }
                    }

                    if (!string.IsNullOrEmpty(head))
                    {
                        return head.Replace("ref:", "").Trim().Replace("refs/heads/", string.Empty);
                    }
                }
            }
            catch (Exception e) when (e is Exception)
            {
                // ignore
            }

            return string.Empty;
        }

        private static string FindGitWorkingDir(string startDir)
        {
            if (string.IsNullOrEmpty(startDir))
            {
                return string.Empty;
            }

            if (!startDir.EndsWith("\\") && !startDir.EndsWith("/"))
            {
                startDir += "\\";
            }

            var dir = startDir;

            while (dir.LastIndexOfAny(new[] { '\\', '/' }) > 0)
            {
                dir = dir.Substring(0, dir.LastIndexOfAny(new[] { '\\', '/' }));

                if (ValidWorkingDir(dir))
                {
                    return dir + "\\";
                }
            }

            return startDir;
        }

        private static bool ValidWorkingDir(string dir)
        {
            if (string.IsNullOrEmpty(dir))
            {
                return false;
            }

            if (Directory.Exists(Path.Combine(dir, ".git")) || File.Exists(Path.Combine(dir, ".git")))
            {
                return true;
            }

            return !dir.Contains(".git") &&
                   Directory.Exists(Path.Combine(dir, "info")) &&
                   Directory.Exists(Path.Combine(dir, "objects")) &&
                   Directory.Exists(Path.Combine(dir, "refs"));
        }

        public static bool GetShowCurrentBranchSetting()
        {
            string showCurrentBranchSetting = GetGitExRegValue("ShowCurrentBranchInVS");
            return string.IsNullOrEmpty(showCurrentBranchSetting) || showCurrentBranchSetting.Equals("true", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}