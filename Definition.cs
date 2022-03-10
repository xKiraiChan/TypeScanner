using System;
using System.Collections.Generic;
using System.Diagnostics;
using TypeScanner.Types;

namespace TypeScanner
{
    public static class Definition
    {
        public static Dictionary<string, ClassDef> Table = new();

        public static ClassDef Get(string id) => Table[id];

        public static event Action<ClassDef> ClassDefSetup;

        public static void Setup(ClassDef def)
        {
            if (def.ID is null)
            {
                TypeScanner.Logger.LogError($"Failed to setup a ClassDef due to missing ID");
                return;
            }

            TypeScanner.Logger.LogInfo("Definition loaded: " + def.ID);

            if (Cache.Mode.Value != Cache.CachingMode.Disabled && Cache.Table.TryGetValue(def.ID, out Type t))
            {
                Stopwatch sw = new();
                sw.Start();

                // returning without setting means that the resolved property will cause a scan
                if (t == null) return;

                if (Cache.Mode.Value == Cache.CachingMode.Standard)
                {
                    def.Build();
                    if (!def.Checks.TrueForAll(x => x(t)))
                        return;
                }

                def._cached = t;
                def._scanned = true;
                sw.Stop();
                TypeScanner.Logger.LogInfo($"Loaded {def.ID} from cache in {sw.ElapsedMilliseconds} ms");
            }

            Table.Add(def.ID, def);
            ClassDefSetup?.Invoke(def);
        }
    }
}
