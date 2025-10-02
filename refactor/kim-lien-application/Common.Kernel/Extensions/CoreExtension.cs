using System.Reflection;

namespace Common.Kernel.Extensions
{
    public static class CoreExtension
    {
        /// <summary>
        /// Returns { name -> value } for the given Type.
        /// - Enums: all enum members.
        /// - Non-enums: all static const fields (IsLiteral && !IsInitOnly).
        /// Only members convertible/assignable to T are included.
        /// </summary>
        public static Dictionary<string, T> GetValues<T>(this Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            var result = new Dictionary<string, T>(StringComparer.Ordinal);

            // Enum case
            if (type.IsEnum)
            {
                foreach (var name in Enum.GetNames(type))
                {
                    var boxedEnum = Enum.Parse(type, name); // boxed enum
                    if (typeof(T) == typeof(string))
                    {
                        result[name] = (T)(object)name;
                        continue;
                    }

                    if (typeof(T).IsEnum && typeof(T) == type)
                    {
                        result[name] = (T)boxedEnum;
                        continue;
                    }

                    var underlying = Enum.GetUnderlyingType(type);
                    var underlyingValue = Convert.ChangeType(boxedEnum, underlying);
                    var target = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                    result[name] = (T)Convert.ChangeType(underlyingValue!, target);
                }
                return result;
            }

            // Non-enum: collect static const fields
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy;
            foreach (var f in type.GetFields(flags))
            {
                if (f.IsLiteral && !f.IsInitOnly) // const
                {
                    var raw = f.GetRawConstantValue(); // works for const
                    if (TryConvert(raw, out T? converted))
                        result[f.Name] = converted!;
                }
                // (Optional) include static readonly fields:
                // else if (f.IsStatic && f.IsInitOnly)
                // {
                //     var val = f.GetValue(null);
                //     if (TryConvert(val, out T? converted))
                //         result[f.Name] = converted!;
                // }
            }

            return result;
        }

        /// <summary>
        /// For any object, returns { PropertyName -> Value } for all public instance readable properties.
        /// Only properties whose values can be assigned/converted to T are included.
        /// </summary>
        public static Dictionary<string, T> GetValues<T>(this object obj)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj));

            var result = new Dictionary<string, T>(StringComparer.Ordinal);
            var type = obj.GetType();

            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!prop.CanRead) continue;
                if (prop.GetIndexParameters().Length != 0) continue; // skip indexers

                var value = prop.GetValue(obj, null);
                if (TryConvert<T>(value, out var converted))
                {
                    result[prop.Name] = converted!;
                }
            }

            return result;
        }

        // ---- helpers ----

        private static bool TryConvert<T>(object? value, out T? converted)
        {
            var targetType = typeof(T);

            // Null handling
            if (value is null)
            {
                if (IsNullable(targetType))
                {
                    converted = default;
                    return true;
                }
                converted = default;
                return false;
            }

            // Direct assignment
            if (targetType.IsInstanceOfType(value))
            {
                converted = (T)value;
                return true;
            }

            try
            {
                // Nullable<T> -> unwrap
                var dest = Nullable.GetUnderlyingType(targetType) ?? targetType;

                // Enums: convert numeric/string to enum if possible
                if (dest.IsEnum)
                {
                    if (value is string s)
                    {
                        converted = (T)Enum.Parse(dest, s, ignoreCase: true);
                        return true;
                    }
                    var numeric = Convert.ChangeType(value, Enum.GetUnderlyingType(dest));
                    converted = (T)Enum.ToObject(dest, numeric!);
                    return true;
                }

                // General conversion (numbers/strings/etc.)
                var changed = Convert.ChangeType(value, dest);
                converted = (T)changed!;
                return true;
            }
            catch
            {
                converted = default;
                return false;
            }
        }

        private static bool IsNullable(Type t) =>
            !t.IsValueType || Nullable.GetUnderlyingType(t) is not null;
    }
}
