using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Qase.Csharp.Commons.Utils
{
    /// <summary>
    /// Utility class for resolving Type and MethodInfo from string names
    /// via assembly scanning with ConcurrentDictionary caching.
    /// Replaces duplicated assembly-scanning logic in NUnit and MSTest reporters.
    /// </summary>
    public static class TypeMethodResolver
    {
        private const BindingFlags AllMethods =
            BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.Static;

        private static readonly ConcurrentDictionary<string, Type?> _typeCache = new ConcurrentDictionary<string, Type?>();

        /// <summary>
        /// Resolves a Type from loaded assemblies by its full class name.
        /// Results (including null for not-found) are cached for performance.
        /// Assemblies that throw on GetTypes() are silently skipped.
        /// </summary>
        /// <param name="fullClassName">Fully qualified class name, e.g., "Namespace.ClassName"</param>
        /// <returns>The resolved Type, or null if not found or input is null/empty</returns>
        public static Type? ResolveType(string? fullClassName)
        {
            if (string.IsNullOrEmpty(fullClassName))
                return null;

            return _typeCache.GetOrAdd(fullClassName, key =>
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a =>
                    {
                        try { return a.GetTypes(); }
                        catch { return Array.Empty<Type>(); }
                    })
                    .FirstOrDefault(t => t.FullName == key));
        }

        /// <summary>
        /// Resolves a MethodInfo from a Type by method name and optional parameter types.
        /// When parameter type names are provided, resolves the specific overload.
        /// Falls back to simple name lookup when no parameter types are given.
        /// Handles AmbiguousMatchException gracefully for overloaded methods.
        /// </summary>
        /// <param name="type">The type to search for the method</param>
        /// <param name="methodName">The method name to resolve</param>
        /// <param name="paramTypeNames">Optional fully qualified parameter type names for overload resolution</param>
        /// <returns>The resolved MethodInfo, or null if not found or inputs are invalid</returns>
        public static MethodInfo? ResolveMethod(Type? type, string? methodName, string[]? paramTypeNames = null)
        {
            if (type == null || string.IsNullOrEmpty(methodName))
                return null;

            // When parameter types are specified, attempt exact overload resolution
            if (paramTypeNames != null && paramTypeNames.Length > 0)
            {
                var paramTypes = paramTypeNames
                    .Select(name => ResolveType(name))
                    .ToArray();

                // Only attempt overload resolution if ALL param types resolved
                if (paramTypes.All(t => t != null))
                {
                    var method = type.GetMethod(
                        methodName,
                        AllMethods,
                        null,
                        paramTypes.Cast<Type>().ToArray(),
                        null);

                    if (method != null)
                        return method;
                }
            }

            // Fallback: simple name lookup, handling AmbiguousMatchException
            try
            {
                return type.GetMethod(methodName, AllMethods);
            }
            catch (AmbiguousMatchException)
            {
                // Safe fallback: return the first matching method
                return type.GetMethods(AllMethods)
                    .FirstOrDefault(m => m.Name == methodName);
            }
        }

        /// <summary>
        /// Clears the type cache. Used for test isolation since the cache is static.
        /// Internal visibility -- accessible from test project via InternalsVisibleTo.
        /// </summary>
        internal static void ClearCache()
        {
            _typeCache.Clear();
        }
    }
}
