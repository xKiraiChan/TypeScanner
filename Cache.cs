using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TypeScanner
{
    public static class Cache
    {
        private const string FilePath = "BepInEx/cache/typescanner.dat";

        public static Dictionary<string, Type> Table;

        static Cache()
        {
            if (File.Exists(FilePath))
                Table = File.ReadAllLines(FilePath)
                    .Select(x => x.Split('='))
                    .Where(x => x.Length >= 2)
                    .Select(x => (x[0], Type.GetType(string.Join("=", x.Skip(1)))))
                    .Where(x => x.Item1 != null && x.Item2 != null)
                    .ToDictionary(x => x.Item1, x => x.Item2);
            else Table = new();
        }

        public static void Save() => File.WriteAllLines(FilePath, Table.Select(x => $"{x.Key}={x.Value.AssemblyQualifiedName}"));
    }

}
