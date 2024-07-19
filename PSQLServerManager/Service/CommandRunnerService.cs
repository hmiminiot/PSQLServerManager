using PSQLServerManager.Properties;
using System.Diagnostics;
using System.IO;

namespace PSQLServerManager.Service
{
    public class CommandRunnerService
    {

        private Process process = null;
        private readonly BackgroundService _backgroundService = new();

        public CommandRunnerService()
        {
            _backgroundService.OnRunningChanged += HandleOnRunningChanged;
        }

        public event Action<string> OnOutput = (stringValue) => { };
        public event Action<Exception> OnException = (exceptionValue) => { };
        public event Action<bool> OnRunningChanged = (booleanValue) => { };
        public event Action OnInvalidDirectory = () => { };

        private void CheckForExistingProcess()
        {
            process?.Dispose();
        }

        public async Task RunCommand(string command)
        {
            try
            {
                CheckForExistingProcess();
                ProcessStartInfo processStartInfo = new()
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = @"cmd",
                    Arguments = $"/c \"{command}\""
                };

                process = new()
                {
                    StartInfo = processStartInfo,
                    EnableRaisingEvents = true
                };

                process.OutputDataReceived += new DataReceivedEventHandler((sender, dataEvent) =>
                {
                    if (!string.IsNullOrEmpty(dataEvent.Data))
                    {
                        OnOutput(dataEvent.Data + Environment.NewLine);
                    }
                });

                process.ErrorDataReceived += new DataReceivedEventHandler((sender, dataEvent) =>
                {
                    if (!string.IsNullOrEmpty(dataEvent.Data))
                    {
                        OnOutput(dataEvent.Data + Environment.NewLine);
                    }
                });

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                await process.WaitForExitAsync();
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }

        public void SendCommandToServer(string command)
        {
            RunActionIfValidDirectory(async () => await RunCommand($"{GetExecutablePath()} -D {GetDirectoryPath()} {command}"));
        }

        private static string GetExecutablePath()
        {
            return $"\"{Settings.Default.WorkingPath}\\pg_ctl.exe\"";
        }

        private static string GetDirectoryPath()
        {
            return $"\"{Settings.Default.WorkingPath.Replace("bin", "data")}\"";
        }

        private void RunActionIfValidDirectory(Action action)
        {
            DirectoryInfo directoryInfo = new(Settings.Default.WorkingPath);
            var files = directoryInfo.GetFiles();
            var isValid = files.Any(f => f.Name.Contains("pg_ctl", StringComparison.OrdinalIgnoreCase));
            if (isValid is false)
            {
                OnInvalidDirectory();
                return;
            }
            action();
        }

        public void RunServerCheck()
        {
            _backgroundService.Start();
        }

        public void StopServerCheck()
        {
            _backgroundService.Stop();
        }

        public void HandleOnRunningChanged(bool isRunning)
        {
            OnRunningChanged(isRunning);
        }

        public bool IsServerRunning()
        {
            return _backgroundService.IsRunning ?? false;
        }
    }
}
