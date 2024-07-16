using PSQLServerManager.Windows.Shared;
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

        private void btnBinDirectory_Click(object sender, RoutedEventArgs e)
        {
            var chosenDirectory = this.OpenFilePicker();
            OnDirectorySelected(chosenDirectory);
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbBinDirectory.Text = promptDirectory;
        }
    }
}
