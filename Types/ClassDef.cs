using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScanner.Types
{
    public class ClassDef
    {
        public string ID;
        public string Derives;
        public string Assembly;
        public Dictionary<string, int> Counts;
        public MethodDef[] Methods;
        public PropertyDef[] Properties;

        internal readonly List<Func<Type, bool>> Checks = new();

        public Type _cached;
        public bool _scanned;
        public bool _built;
        public Type Resolved
        {
            get
            {
                if (!_scanned)
                    Rescan();
                return _cached;
            }
        }

        public void Rescan()
        {
            Stopwatch sw = new();
            sw.Start();

            if (!_built) Build();

            _scanned = true;
            _cached = System.Reflection.Assembly.Load(Assembly)
                .GetExportedTypes()
                .FirstOrDefault(x => Checks.All(f => f(x)));

            Cache.Table[ID] = _cached; 
            Cache.Save();

            sw.Stop();
            TypeScanner.Logger.LogDebug($"Scanned definition {ID} in {sw.ElapsedMilliseconds} ms");
        }

        public void Build()
        {
            _built = true;

            if (Derives is not null)
            {
                if (Type.GetType(Derives) is Type t)
                    Checks.Add(x => x.BaseType == t);
                else TypeScanner.Logger.LogWarning($"Failed to resolve type: {Derives}");
            }

            if (Counts is not null)
            {
                if (Counts["Constructor"] is int cc) Checks.Add(x => x.GetConstructors().Count() == cc);
            }

            if (Methods is not null)
                foreach (var m in Methods)
                {
                    m.Build();
                    Checks.Add(x => m.Checks.All(c => x.GetMethods().Any(i => c(i))));
                }

            if (Properties is not null)
                foreach (var m in Properties)
                {
                    m.Build();
                    Checks.Add(x => m.Checks.All(c => x.GetProperties().Any(i => c(i))));
                }
        }
    }
}
