namespace InstaMap;

public interface IObjectMapper
{
    // This interface is used to mark interfaces as mappers.
}

/// <summary>
/// Provides a mapping function for the source and destination types.
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDestination"></typeparam>
public interface IObjectMapper<TSource, TDestination> : IObjectMapper where TDestination : class, new()
{
    /// <summary>
    /// Maps the source object to the destination object.
    /// </summary>
    /// <remarks>
    /// If an explicit mapper function was not provided in the constructor, the mapper will be built
    /// from expressions. This method is thread-safe and will only build the mapper function once.
    /// </remarks>
    /// <param name="source"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    TDestination Map(TSource source, IMapper mapper);
}
