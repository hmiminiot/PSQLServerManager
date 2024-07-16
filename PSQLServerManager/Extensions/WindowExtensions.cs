using System.Windows;

namespace PSQLServerManager.Extensions
{
    public static class WindowExtensions
    {
        public static string OpenFolderBrowser(this Window window, string initialPath = @"C:\")
        {
            FolderBrowserDialog openFileDialog = new()
            {
                InitialDirectory = initialPath
            };
            var result = openFileDialog.ShowDialog();
            return result == DialogResult.OK ? openFileDialog.SelectedPath : initialPath;
        }
    }
}
