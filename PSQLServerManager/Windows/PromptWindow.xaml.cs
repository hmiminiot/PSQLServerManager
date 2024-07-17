using PSQLServerManager.Extensions;
using System.Windows;

namespace PSQLServerManager
{
    /// <summary>
    /// Interaction logic for PromptWindow.xaml
    /// </summary>
    public partial class PromptWindow : Window
    {
        public event Action<string> OnDirectorySelected = (value) => { };
        private string promptDirectory;
        public PromptWindow(string promptDirectory)
        {            
            InitializeComponent();
            this.promptDirectory = promptDirectory;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbBinDirectory.Text = promptDirectory;
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var chosenDirectory = this.OpenFolderBrowser(promptDirectory);
            OnDirectorySelected(chosenDirectory);
            Close();
        }
    }
}
