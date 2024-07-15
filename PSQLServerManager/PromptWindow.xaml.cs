using System.Windows;

namespace PSQLServerManager
{
    /// <summary>
    /// Interaction logic for PromptWindow.xaml
    /// </summary>
    public partial class PromptWindow : Window
    {
        public string binDirectory = "";
        public PromptWindow()
        {            
            InitializeComponent();
        }

        private void btnBinDirectory_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openFileDlg = new();
            openFileDlg.InitialDirectory = "C:";
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                binDirectory = openFileDlg.SelectedPath;
                tbBinDirectory.Text = binDirectory;
            }
        }

        private void Button_DoneClick(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.workingDir = binDirectory;
        }
    }
}
