using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace TypeScanner.Types
{
    public class ClassDef
    {
        public string ID;
        public Type Derives;
        public Assembly Assembly;
        public Dictionary<string, int> Counts = new();
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
            _cached = Assembly.GetExportedTypes()
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
                Checks.Add(x => x.BaseType == Derives);

            if (Counts is not null)
            {
                if (Counts.TryGetValue("Constructor", out int cc)) Checks.Add(x => x.GetConstructors().Count() == cc);
            }

            if (Methods is not null)
                foreach (var m in Methods)
                {
                    m.Build();
                    Checks.Add(x => m.Absent ^ x.GetMethods().Any(i => m.Checks.All(c => c(i))));
                }

            if (Properties is not null)
                foreach (var m in Properties)
                {
                    m.Build();
                    Checks.Add(x => m.Absent ^ x.GetProperties().Any(i => m.Checks.All(c => c(i))));
                }
        }

        public static ClassDef Create(string id) => new() { ID = id };
        public ClassDef FromAssembly(Assembly assembly) { Assembly = assembly; return this; }
        public ClassDef DerivesFrom<T>() { Derives = typeof(T); return this; }
        public ClassDef DerivesFrom(Type type) { Derives = type; return this; }
        public ClassDef ConstructorCount(int count) { Counts["Constructor"] = count; return this; }
        public ClassDef WithMethods(params MethodDef[] methods) { Methods = methods; return this; }
        public ClassDef WithProperties(params PropertyDef[] props) { Properties = props; return this; }
        public ClassDef Setup() { Definition.Setup(this); return this; }
    }
}
