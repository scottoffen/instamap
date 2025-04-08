using System.Linq.Expressions;

namespace InstaMap;

/// <summary>
/// Provides methods to build a mapping function for the source and destination types.
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDestination"></typeparam>
public interface IObjectMapBuilder<TSource, TDestination> where TDestination : class, new()
{
    /// <summary>
    /// Adds a callback that will be invoked after the mapping is complete.
    /// </summary>
    /// <remarks>
    /// Multiple callbacks can be added. They will be invoked in the order they were added.
    /// </remarks>
    /// <param name="callback"></param>
    /// <returns></returns>
    IObjectMapBuilder<TSource, TDestination> AfterMap(Action<TSource, TDestination, IMapper> callback);

    /// <summary>
    /// Adds a mapping expression for the destination property. Overrides any existing mapping for the same property.
    /// This prevents the creation of an automatic mapping for the destination property.
    /// </summary>
    /// <typeparam name="TMember"></typeparam>
    /// <param name="destinationProperty"></param>
    /// <param name="sourceExpression"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    IObjectMapBuilder<TSource, TDestination> AndMap<TMember>(
        Expression<Func<TDestination, TMember>> destinationProperty,
        Expression<Func<TSource, IMapper, TMember>> sourceExpression);

    /// <summary>
    /// Adds a destination property to the list of properties that should be ignored when adding expressions to the expression tree.
    /// </summary>
    /// <typeparam name="TMember"></typeparam>
    /// <param name="destinationProperty"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    IObjectMapBuilder<TSource, TDestination> Ignore<TMember>(
        Expression<Func<TDestination, TMember>> destinationProperty);
}
