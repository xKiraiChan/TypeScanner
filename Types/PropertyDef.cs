using System;
using System.Collections.Generic;
using System.Reflection;

namespace TypeScanner.Types
{
    public class PropertyDef
    {
        public string ID;
        public string Name;
        public Type Type;
        public bool Absent;

        internal readonly List<Func<PropertyInfo, bool>> Checks = new();

        public void Build()
        {
            Checks.Clear();

            if (Name is not null) Checks.Add(x => x.Name == Name);
            if (Type is not null) Checks.Add(x => x.PropertyType == Type);
        }

        public static PropertyDef Create(string id = null) => new() { ID = id };
        public PropertyDef WithName(string name) { Name = name; return this; }
        public PropertyDef WithType<T>() { Type = typeof(T); return this; }
        public PropertyDef WithType(Type type) { Type = type; return this; }
        public PropertyDef ExpectAbsent() { Absent = true; return this; }
    }
}
