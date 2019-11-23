using FactrIDE.Properties;

using System.Resources;

namespace FactrIDE
{
    public static class GeminiResources
    {
        private static ResourceManager resourceMan;
        private static ResourceManager ResourceManager => resourceMan ?? (resourceMan = new ResourceManager("Gemini.Properties.Resources", typeof(global::Gemini.AppBootstrapper).Assembly));

        public static string SettingsPathEnvironment => ResourceManager.GetString("SettingsPathEnvironment", Resources.Culture);
        public static string SettingsDisplayName => ResourceManager.GetString("SettingsDisplayName", Resources.Culture);
    }
}