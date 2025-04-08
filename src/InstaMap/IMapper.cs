namespace InstaMap;

/// <summary>
/// Provides functionality to map objects and collections.
/// </summary>
public interface IMapper
{
    /// <summary>
    /// Maps an object of type TSource to an object of type TDestination.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    TDestination Map<TSource, TDestination>(TSource source) where TDestination : class, new();

    /// <summary>
    /// Maps an object to an object of type TDestination.
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    TDestination Map<TDestination>(object source) where TDestination : class, new();

    /// <summary>
    /// Maps an enumerable collection of type TSource to an enumerable collection of type TDestination.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source) where TDestination : class, new();

    /// <summary>
    /// Maps an enumerable collection of objects to an enumerable collection of type TDestination.
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    IEnumerable<TDestination> Map<TDestination>(IEnumerable<object> source) where TDestination : class, new();
}
