using System;
using System.Diagnostics;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MessageBox = System.Windows.MessageBox;

namespace PSQLServerManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string workingDir = "";
        private Process? process = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckForExistingProcess()
        {
            process?.Dispose();
        }

        private async Task RunCmdCommand(string command)
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
                        Dispatcher.Invoke(() =>
                        {
                            tbOutput.Text += dataEvent.Data + Environment.NewLine;
                        });
                    }
                });

                process.ErrorDataReceived += new DataReceivedEventHandler((sender, dataEvent) =>
                {
                    if (!string.IsNullOrEmpty(dataEvent.Data))
                    {
                        Dispatcher.Invoke(() => 
                        {
                            tbError.Text += dataEvent.Data + Environment.NewLine;
                        });
                    }
                });

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                await process.WaitForExitAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}");
            }
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            ResetUi();
            string command = $"{GetExecutablePath()} -D \"C:\\Program Files\\PostgreSQL\\16\\data\" start";
            await RunCmdCommand(command);
        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            ResetUi();
            string command = $"{GetExecutablePath()} -D \"C:\\Program Files\\PostgreSQL\\16\\data\" stop";
            await RunCmdCommand(command);
        }

        private async void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            ResetUi();
            string command = $"{GetExecutablePath()} -D \"C:\\Program Files\\PostgreSQL\\16\\data\" restart";
            await RunCmdCommand(command);
        }

        private void ResetUi()
        {
            tbError.Text = tbOutput.Text = "";
        }

        private void binDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openFileDlg = new FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                postgresBinDirectoryTextBox.Text = openFileDlg.SelectedPath;
            }
        }

        //private void postgresBinDirectoryTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        //{
        //    workingDir = postgresBinDirectoryTextBox.Text;
        //}

        private string GetExecutablePath()
        {
            return $"\"{workingDir}\\pg_ctl.exe\"";
        }
    }
}