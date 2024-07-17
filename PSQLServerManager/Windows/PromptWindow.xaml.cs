using PSQLServerManager.Extensions;
using PSQLServerManager.Properties;
using System.Windows;

namespace PSQLServerManager
{
    /// <summary>
    /// Interaction logic for PromptWindow.xaml
    /// </summary>
    public partial class PromptWindow : Window
    {
        public event Action<string> OnDirectorySelected = (workingDirectory) => { };
        private readonly string promptDirectory = Settings.Default.WorkingPath;
        public PromptWindow()
        {            
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbBinDirectory.Text = promptDirectory;
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var chosenDirectory = this.OpenFolderBrowser(promptDirectory);
            tbBinDirectory.Text = chosenDirectory;
            OnDirectorySelected(chosenDirectory);
        }

        private void PromptWindowOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
