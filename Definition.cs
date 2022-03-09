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

            if (Cache.Table.TryGetValue(def.ID, out Type t))
            {
                Stopwatch sw = new();
                sw.Start();

                def.Build();
                if (def.Checks.TrueForAll(x => x(t)))
                {
                    def._cached = t;
                    def._scanned = true;

                    sw.Stop();
                    TypeScanner.Logger.LogInfo($"Loaded {def.ID} from cache in {sw.ElapsedMilliseconds} ms");
                }
            }

            Table.Add(def.ID, def);
            ClassDefSetup?.Invoke(def);
        }
    }
}
