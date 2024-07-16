using PSQLServerManager.Service;
using PSQLServerManager.Windows.Shared;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace PSQLServerManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ServerHubWindow : Window
    {
        public string ServerHubDirectory = @"C:\";
        private readonly CommandRunnerService _commandRunnerService = new();

        public ServerHubWindow()
        {
            InitializeComponent();
            _commandRunnerService.OnOutput += HandleOnOutput;
            _commandRunnerService.OnException += HandleOnException;
            _commandRunnerService.OnRunningChanged += HandleOnRunningChanged;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenPromptWindow();
            tbWorkingDirectory.Text = ServerHubDirectory;
            _commandRunnerService.RunServerCheck();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _commandRunnerService.StopServerCheck();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            ResetUi();
            string command = $"{GetExecutablePath()} -D {GetDirectoryPath()} start"; 
            await _commandRunnerService.RunCommand(command);
        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            ResetUi();
            string command = $"{GetExecutablePath()} -D {GetDirectoryPath()} stop";
            await _commandRunnerService.RunCommand(command);
        }

        private async void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            ResetUi();
            string command = $"{GetExecutablePath()} -D {GetDirectoryPath()} restart";
            await _commandRunnerService.RunCommand(command);
        }

        private void Button_ChangeDirectory_Click(object sender, RoutedEventArgs e)
        {
            ServerHubDirectory = tbWorkingDirectory.Text = this.OpenFilePicker();
        }

        private void HandleOnOutput(string output) 
        {
            Dispatcher.Invoke(() => tbOutput.Text += output);
        }

        private void HandleOnException(Exception ex)
        {
            Dispatcher.Invoke(() => MessageBox.Show($"Error: {ex.Message}"));
        }

        private void HandleOnRunningChanged(bool isRunning)
        {
            Dispatcher.Invoke(() => statusBar.Fill = isRunning ? System.Windows.Media.Brushes.Green : System.Windows.Media.Brushes.Red);
        }

        private void OpenPromptWindow()
        {
            PromptWindow promptWindow = new(ServerHubDirectory);
            promptWindow.OnDirectorySelected += (directoryValue) => ServerHubDirectory = tbWorkingDirectory.Text = directoryValue;
            promptWindow.ShowDialog();
        }

        private void ResetUi()
        {
            tbOutput.Text = "";
        }

        private string GetExecutablePath()
        {
            return $"\"{ServerHubDirectory}\\pg_ctl.exe\"";
        }

        private string GetDirectoryPath()
        {
            var isBinDirectory = ServerHubDirectory.EndsWith("bin", StringComparison.OrdinalIgnoreCase);
            if (isBinDirectory)
            {
                return $"\"{ServerHubDirectory.Replace("bin", "data")}\"";
            }
            if (isBinDirectory is false)
            {
                Dispatcher.Invoke(() => MessageBox.Show($"Path: {ServerHubDirectory} is not valid!\nPlease use the PostgreSQL Bin Directory."));
            }
            return "";
        }
    }
}