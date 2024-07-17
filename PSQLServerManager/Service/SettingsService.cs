using System.IO;

namespace PSQLServerManager.Service
{
    public class SettingsService
    {
        public string GetSettings()
        {
            CreateFileIfNotExist();
            StreamReader StreamReader = new("Settings.txt");
            var settingsString = StreamReader.ReadLine();
            StreamReader.Close();
            return settingsString;
        }

        public void SaveSettings(string settings)
        {
            StreamWriter StreamWriter = new("Settings.txt");
            if (string.IsNullOrWhiteSpace(settings))
            {
                settings = @"C:\";
            }
            StreamWriter.WriteLine(settings);
            StreamWriter.Close();
        }
        private static void CreateFileIfNotExist()
        {
            if (!File.Exists("Settings.txt"))
            {
                File.AppendAllText("Settings.txt", @"C:\");
            }
        }
    }
}
