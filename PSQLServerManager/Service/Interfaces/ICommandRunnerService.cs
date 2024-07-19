namespace PSQLServerManager.Service.Interfaces
{
    public interface ICommandRunnerService
    {
        public Task RunCommand(string command);
        public void SendCommandToServer(string command);
        public void RunServerCheck();
        public void StopServerCheck();
        public void HandleOnRunningChanged(bool isRunning);
        public bool IsServerRunning();
        public event Action<string> OnOutput;
        public event Action<Exception> OnException;
        public event Action<bool> OnRunningChanged;
        public event Action OnInvalidDirectory;
    }
}
