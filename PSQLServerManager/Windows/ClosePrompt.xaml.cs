using PSQLServerManager.Service;
using System.Windows;

namespace PSQLServerManager.Windows
{
    /// <summary>
    /// Interaction logic for ClosePromptWindow.xaml
    /// </summary>
    public partial class ClosePrompt : Window
    {
        private readonly CommandRunnerService _commandRunnerService;
        public ClosePrompt(CommandRunnerService commandRunnerService)
        {
            InitializeComponent();
            _commandRunnerService = commandRunnerService;
        }

        private void ButtonYesCloseServer_Click(object sender, RoutedEventArgs e)
        {
            _commandRunnerService.SendCommandToServer("stop");
            Close();
        }

        private void ButtonNoCloseServer_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
