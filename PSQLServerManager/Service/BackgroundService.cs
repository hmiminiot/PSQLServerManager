using System.ComponentModel;
using System.Diagnostics;

namespace PSQLServerManager.Service
{
    public class BackgroundService
    {
        private readonly BackgroundWorker _worker = new();

        public bool? IsRunning { get; private set; } = null;
        public event Action<bool> OnRunningChanged = (value) => { };

        public BackgroundService()
        {
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += DoWork;
        }

        public void Start()
        {
            _worker.RunWorkerAsync();
        }

        public void Stop()
        {
            _worker.CancelAsync();
        }

        private void DoWork(object? sender, DoWorkEventArgs e)
        {
            Process[] processes; 
            while (e.Cancel is false)
            {
                processes = Process.GetProcesses();
                var isRunning = processes.Any(p => p.ProcessName.Equals("postgres", StringComparison.OrdinalIgnoreCase));
                if (isRunning != IsRunning)
                {
                    OnRunningChanged(isRunning);
                }
                IsRunning = isRunning;
                Thread.Sleep(500);
            }
        }
    }
}
