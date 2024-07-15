using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PSQLServerManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string command = "pg_ctl -D \"C:\\Program Files\\PostgreSQL\\16\\data\" start";

            textBox1.Text = command;

            RunCmdCommand(command);
        }

        private void RunCmdCommand(string command)
        {
            try
            {
                string cmdText = $"\"{command}\"";
                ProcessStartInfo processStartInfo = new("cmd", $"/c {command}")
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = "C:\\Program Files\\PostgreSQL\\16\\bin"
                };

                Process process = new()
                {
                    StartInfo = processStartInfo
                };

                process.Start();

                string output = "";

                do
                {
                    //var processTest = process.StandardOutput;
                    //var test = processTest.Peek();
                    //output += processTest.ReadLine() ?? "";
                } while (process.StandardOutput.EndOfStream is false);

                string error = "";//process.StandardError.ReadToEnd();

                process.WaitForExit();

                MessageBox.Show($"{(string.IsNullOrWhiteSpace(error) ? $"Output: {output}" : $"\nError: {error}")}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}");
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            string command = "pg_ctl -D \"C:\\Program Files\\PostgreSQL\\16\\data\" stop";

            textBox1.Text = command;

            RunCmdCommand(command);
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            string command = "pg_ctl -D \"C:\\Program Files\\PostgreSQL\\16\\data\" restart";

            textBox1.Text = command;

            RunCmdCommand(command);
        }
    }
}