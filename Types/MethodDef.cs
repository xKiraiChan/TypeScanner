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
        public string Returns;
        public List<string> Parameters;

        internal readonly List<Func<MethodInfo, bool>> Checks = new();

        public void Build()
        {
            Checks.Clear();

            if (Name is not null) Checks.Add(x => x.Name == Name);
            if (Returns is not null)
                if (Type.GetType(Returns) is Type t)
                    Checks.Add(x => x.ReturnType == t);
                else TypeScanner.Logger.LogWarning($"Failed to resolve type: {Returns}");

            if (Parameters is not null)
            {
                Type[] ps = Parameters.Select(x => x is string str ? Type.GetType(str) : null).ToArray();

                Checks.Add(x =>
                {
                    ParameterInfo[] parms = x.GetParameters();
                    if (parms.Length != ps.Length) return false;
                    for (int i = 0; i < parms.Length; i++)
                        if (ps[i] != null && ps[i] != parms[i].ParameterType) return false;
                    return true;
                });
            }
        }
    }
}
