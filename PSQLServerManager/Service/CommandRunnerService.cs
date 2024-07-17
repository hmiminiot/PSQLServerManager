using System.ComponentModel;
using System.Diagnostics;

namespace PSQLServerManager.Service
{
    public class CommandRunnerService
    {

        private Process? process = null;
        private readonly BackgroundService _backgroundService = new();

        public CommandRunnerService()
        {
            _backgroundService.OnRunningChanged += HandleOnRunningChanged;
        }

        public event Action<string> OnOutput = (stringValue) => { };
        public event Action<Exception> OnException = (exceptionValue) => { };
        public event Action<bool> OnRunningChanged = (booleanValue) => { };

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
                    FileName = @"C:\Windows\System32\cmd.exe",
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
    }
}
