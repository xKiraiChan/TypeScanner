using System;
using System.Collections.Generic;
using System.Reflection;

namespace TypeScanner.Types
{
    public class PropertyDef
    {
        public string ID;
        public string Name;
        public string Type;

        internal readonly List<Func<PropertyInfo, bool>> Checks = new();

        public void Build()
        {
            Checks.Clear();

            if (Name is not null) Checks.Add(x => x.Name == Name);
            if (Type is not null)
                if (System.Type.GetType(Type) is Type t)
                    Checks.Add(x => x.PropertyType == t);
                else TypeScanner.Logger.LogWarning($"Failed to resolve type: {Type}");
        }
    }
}
