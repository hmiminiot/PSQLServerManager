using PSQLServerManager.Extensions;
using PSQLServerManager.Properties;
using System.Windows;

namespace PSQLServerManager
{
    /// <summary>
    /// Interaction logic for PromptWindow.xaml
    /// </summary>
    public partial class OpenPrompt : Window
    {
        public event Action<string> OnDirectorySelected = (workingDirectory) => { };
        public OpenPrompt()
        {            
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbBinDirectory.Text = Settings.Default.WorkingPath;
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var chosenDirectory = this.OpenFolderBrowser(Settings.Default.WorkingPath);
            tbBinDirectory.Text = Settings.Default.WorkingPath = chosenDirectory;
            OnDirectorySelected(chosenDirectory);
        }

        private void PromptWindowOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
