using PSQLServerManager.Extensions;
using PSQLServerManager.Properties;
using PSQLServerManager.Service;
using PSQLServerManager.Windows;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace PSQLServerManager
{
    /// <summary>
    /// Interaction logic for ServerHub.xaml
    /// </summary>
    public partial class ServerHub : Window
    {
        private readonly CommandRunnerService _commandRunnerService = new();

        public ServerHub()
        {
            InitializeComponent();
            _commandRunnerService.OnOutput += HandleOnOutput;
            _commandRunnerService.OnException += HandleOnException;
            _commandRunnerService.OnRunningChanged += HandleOnRunningChanged;
            _commandRunnerService.OnInvalidDirectory += HandleOnInvalidDirectory;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenPromptWindow();
            tbWorkingDirectory.Text = Settings.Default.WorkingPath;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClosePromptWindow();
            Settings.Default.WorkingPath = Settings.Default.WorkingPath;
            Settings.Default.Save();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            ResetUi();
            _commandRunnerService.SendCommandToServer("start");
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            ResetUi();
            _commandRunnerService.SendCommandToServer("stop");
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            ResetUi();
            _commandRunnerService.SendCommandToServer("restart");
        }

        private void Button_ChangeDirectory_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.WorkingPath = tbWorkingDirectory.Text = this.OpenFolderBrowser(Settings.Default.WorkingPath);
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

        private void HandleOnInvalidDirectory()
        {
            Dispatcher.Invoke(() => MessageBox.Show(
            this,
            $"Path: {Settings.Default.WorkingPath} is not valid!\nPlease use the PostgreSQL Bin Directory.",
            "Error with Path!",
            MessageBoxButton.OK,
            MessageBoxImage.Error));
        }

        private void OpenPromptWindow()
        {
            OpenPrompt openPrompt = new();
            openPrompt.OnDirectorySelected += (workingDirectory) => Settings.Default.WorkingPath = workingDirectory;
            openPrompt.ShowDialog();
            _commandRunnerService.RunServerCheck();
        }

        private void ClosePromptWindow()
        {
            if (_commandRunnerService.IsServerRunning())
            {
                ClosePrompt closePrompt = new(_commandRunnerService);
                closePrompt.ShowDialog();
            }
            _commandRunnerService.StopServerCheck();
        }

        private void ResetUi()
        {
            tbOutput.Text = "";
        }
    }
}