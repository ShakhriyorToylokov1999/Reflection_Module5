using Reflection;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflection
{
    [AttributeUsage(AttributeTargets.All)]
    public class FileConfigurationItemAttribute : Attribute
    {
        public readonly string _settingName;

        public FileConfigurationItemAttribute(string settingName)
        {
            _settingName = settingName;
        }

        public string Setting
        {
            get { return GetSettingValue(); }
            set { SetSettingValue(_settingName, value); }
        }

        public string GetSettingValue()
        {
            string configFilePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "config.txt");
            string[] lines = File.ReadAllLines(configFilePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split('=');
                if (parts.Length == 2 && parts[0].Trim() == _settingName)
                {
                    return parts[1].Trim();
                }
            }
            return "Not Found";
        }

        public void SetSettingValue(string key, string value)
        {
            string configFilePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "config.txt");
            string[] lines = File.ReadAllLines(configFilePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split('=');
                if (parts.Length == 2 && parts[0].Trim() == key)
                {
                    lines[i] = key + " = " + value;
                    File.WriteAllLines(configFilePath, lines);
                    return;
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class ConfigurationManagerConfigurationItemAttribute : Attribute
    {
        public readonly string _settingName;

        public ConfigurationManagerConfigurationItemAttribute(string settingName)
        {
            _settingName = settingName;
        }

        public string Setting
        {
            get { return GetSettingValue(); }
            set { SetSettingValue(_settingName, value); }
        }

        private string GetSettingValue()
        {
            var appSettings = ConfigurationManager.AppSettings;
            return appSettings[_settingName] ?? "Not found";
        }

        private static void SetSettingValue(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
    }
}
