using System.Collections;

namespace InstaMap;

internal static class TypeExtensions
{
    private static readonly Type _convertible = typeof(IConvertible);

    private static readonly Type _dictionary = typeof(Dictionary<,>);

    private static readonly Type _enumerable = typeof(IEnumerable);

    private static readonly Type _string = typeof(string);

    private static readonly List<Type> _integerTypes =
    [
        typeof(byte),
        typeof(sbyte),
        typeof(short),
        typeof(ushort),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong)
    ];

    /// <summary>
    /// Returns true if both the source and destination types are convertible.
    /// </summary>
    /// <remarks>
    /// This method is used to determine if a type can be converted to another type. See <see cref="IConvertible"/>
    /// </remarks>
    /// <param name="sourceType"></param>
    /// <param name="destinationType"></param>
    /// <returns></returns>
    public static bool IsConvertible(this Type sourceType, Type destinationType)
    {
        return _convertible.IsAssignableFrom(sourceType) && _convertible.IsAssignableFrom(destinationType);
    }

    /// <summary>
    /// Returns true if both the source and destination types are dictionaries.
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destinationType"></param>
    /// <returns></returns>
    public static bool IsDictionary(this Type sourceType, Type destinationType)
    {
        return sourceType.IsGenericType &&
            destinationType.IsGenericType &&
            sourceType.GetGenericTypeDefinition() == _dictionary &&
            destinationType.GetGenericTypeDefinition() == _dictionary;
    }

    /// <summary>
    /// Returns true if both the source and destination types are enumerable.
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destinationType"></param>
    /// <param name="sourceElementType"></param>
    /// <param name="destinationElementType"></param>
    /// <returns></returns>
    public static bool IsEnumerable(this Type sourceType, Type destinationType, out Type? sourceElementType, out Type? destinationElementType)
    {
        sourceElementType = destinationElementType = null;
        if (!sourceType.IsGenericType || !destinationType.IsGenericType) return false;

        if (_enumerable.IsAssignableFrom(sourceType) && _enumerable.IsAssignableFrom(destinationType))
        {
            sourceElementType = sourceType.GetGenericArguments()[0];
            destinationElementType = destinationType.GetGenericArguments()[0];

            return sourceElementType != null && destinationElementType != null;
        }

        return false;
    }

    /// <summary>
    /// Returns true if the type is an integer type.
    /// </summary>
    /// <remarks>
    /// Integer types include byte, sbyte, short, ushort, int, uint, long, and ulong.
    /// </remarks>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsIntegerType(this Type type)
    {
        return _integerTypes.Contains(type);
    }

    /// <summary>
    /// Returns true if the type is <see cref="string" />
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsStringType(this Type type)
    {
        return type == _string;
    }
}
