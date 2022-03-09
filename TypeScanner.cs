using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;

namespace TypeScanner
{
    [BepInPlugin(GUID, "TypeScanner", "0.1.0")]
    public class TypeScanner : BasePlugin
    {
        public const string GUID = "com.github.xKiraiChan.TypeScanner";
        public const string TypeDefsPath = "BepInEx/TypeDefs";

        internal static ManualLogSource Logger;

        public override void Load() => Logger = Log;
    }
}
