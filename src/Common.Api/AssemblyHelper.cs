using System.Reflection;
using System.Runtime.Loader;

namespace Common.Api
{
    public static class AssemblyHelper
    {
        private static readonly string[] _moduleApiNames = [
            "Admin.Api",
            "Authen.Api",
            "CentralData.MigrateDbContext",
            "Client.Api",
            "Import.Api",
            "Legacy.Module"
            ];

        /// <summary>
        /// Get module assembly
        /// </summary>
        /// <returns></returns>
        public static List<Assembly> GetModuleAssemblies()
        {
            var result = new List<Assembly>();
            var baseDir = AppContext.BaseDirectory;
            var modulesDir = Path.Combine(baseDir, "Modules");

            // Reuse already-loaded assemblies if present (avoids duplicates)
            var loaded = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => a.GetName().Name.EndsWith(".Api"))
                .ToDictionary(a => a.GetName().Name!, a => a, StringComparer.OrdinalIgnoreCase);
            foreach (var name in _moduleApiNames)
            {
                if (!loaded.TryGetValue(name, out var asm))
                {
                    // Try /<output>/<name>.dll
                    var path = Path.Combine(baseDir, $"{name}.dll");

                    // Try /<output>/Modules/<name>.dll
                    if (!File.Exists(path) && Directory.Exists(modulesDir))
                        path = Path.Combine(modulesDir, $"{name}.dll");

                    // Try /<output>/Modules/<name>/<name>.dll
                    if (!File.Exists(path))
                    {
                        var nested = Path.Combine(modulesDir, name, $"{name}.dll");
                        if (File.Exists(nested)) path = nested;
                    }

                    if (!File.Exists(path))
                        continue;

                    asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.GetFullPath(path));
                }

                if (asm != null)
                {
                    result.Add(asm);
                }
            }

            return [.. result.Where(a => a != null).DistinctBy(a => a.FullName)];
        }
    }
}
