namespace PSQLServerManager.Service.Interfaces
{
    public interface IBackgroundService
    {
        public void Start();
        public void Stop();
        public bool IsRunning();
        public event Action<bool> OnRunningChanged;
    }
}
