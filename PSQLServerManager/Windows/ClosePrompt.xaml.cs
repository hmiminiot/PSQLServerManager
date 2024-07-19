using PSQLServerManager.Service;
using PSQLServerManager.Service.Interfaces;
using System.Windows;

namespace PSQLServerManager.Windows
{
    /// <summary>
    /// Interaction logic for ClosePromptWindow.xaml
    /// </summary>
    public partial class ClosePrompt : Window
    {
        private readonly ICommandRunnerService _commandRunnerService;
        public ClosePrompt(ICommandRunnerService commandRunnerService)
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
