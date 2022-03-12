using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TypeScanner.Types
{
    public class MethodDef
    {
        public string ID;
        public string Name;
        public Type Returns;
        public Type[] Parameters;
        public bool Absent;

        internal readonly List<Func<MethodInfo, bool>> Checks = new();

        public void Build()
        {
            Checks.Clear();

            if (Name is not null) Checks.Add(x => x.Name == Name);
            if (Returns is not null)
                Checks.Add(x => x.ReturnType == Returns);

            if (Parameters is not null)
            {
                Checks.Add(x =>
                {
                    ParameterInfo[] parms = x.GetParameters();
                    if (parms.Length != Parameters.Length) return false;
                    for (int i = 0; i < parms.Length; i++)
                        if (Parameters[i] != null && Parameters[i] != parms[i].ParameterType) return false;
                    return true;
                });
            }
        }

        public static MethodDef Create(string id = null) => new() { ID = id };
        public MethodDef WithName(string name) { Name = name; return this; }
        public MethodDef ReturnsType<T>() { Returns = typeof(T); return this; }
        public MethodDef ReturnsType(Type type) { Returns = type; return this; }
        public MethodDef WithParameters(params Type[] parms) { Parameters = parms; return this; }
        public MethodDef ExpectAbsent() { Absent = true; return this; }
    }
}
