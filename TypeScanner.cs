using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using BepInEx.Logging;

namespace TypeScanner
{
    [BepInPlugin(GUID, "TypeScanner", "0.2.0")]
    public class TypeScanner : BasePlugin
    {
        public const string GUID = "com.github.xKiraiChan.TypeScanner";

        internal static ManualLogSource Logger;
        internal static ConfigFile Configuration;

        public override void Load()
        {
            Logger = Log;
            Configuration = Config;
        }
    }
}
