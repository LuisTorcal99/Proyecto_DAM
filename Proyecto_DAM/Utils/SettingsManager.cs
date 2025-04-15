using System.IO;
using System.Text.Json;

public class SettingsManager
{
    private const string ConfigFilePath = "AppSettings.json";

    public string SelectedTheme { get; set; }
    public static SettingsManager LoadSettings()
    {
        if (File.Exists(ConfigFilePath))
        {
            string json = File.ReadAllText(ConfigFilePath);
            return JsonSerializer.Deserialize<SettingsManager>(json);
        }
        return new SettingsManager { SelectedTheme = "Claro" }; 
    }

    public void SaveSettings()
    {
        string json = JsonSerializer.Serialize(this);
        File.WriteAllText(ConfigFilePath, json);
    }
}
