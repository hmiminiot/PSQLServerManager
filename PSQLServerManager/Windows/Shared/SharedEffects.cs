using System.Windows;

namespace PSQLServerManager.Windows.Shared
{
    public static class SharedEffects
    {
        public static string OpenFilePicker(this Window window, string initialPath = @"C:\")
        {
            FolderBrowserDialog openFileDlg = new()
            {
                InitialDirectory = initialPath
            };
            openFileDlg.ShowDialog();
            return openFileDlg.SelectedPath;
        }
    }
}
