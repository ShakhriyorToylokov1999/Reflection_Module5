using System.Reflection;

namespace Reflection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MemberInfo info = typeof(Setting);
            var attributes = info.GetCustomAttributes(true);

            foreach (var attribute in attributes)
            {
                if (attribute is ConfigurationManagerConfigurationItemAttribute)
                {
                    var setting = attribute as ConfigurationManagerConfigurationItemAttribute;
                    if (setting != null)
                    {
                        Console.WriteLine($"Setting '{setting._settingName}': {setting.Setting}");
                        setting.Setting = $"new value blah.blah";
                        Console.WriteLine($"Updated value for setting '{setting._settingName}': {setting.Setting}");
                    }
                }
                if (attribute is FileConfigurationItemAttribute)
                {
                    var setting = attribute as FileConfigurationItemAttribute;
                    if (setting != null)
                    {
                        Console.WriteLine($"Setting '{setting._settingName}': {setting.Setting}");
                        setting.Setting = $"new value 7";
                        Console.WriteLine($"Updated value for setting '{setting._settingName}': {setting.Setting}");
                    }
                }
            }

        }
    }


    [ConfigurationManagerConfigurationItemAttribute("BatchFile")]
    [FileConfigurationItemAttribute("LogLevel")]
    public class Setting
    {
    }
}