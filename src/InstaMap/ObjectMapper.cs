using System.Linq.Expressions;

namespace InstaMap;

public class ObjectMapper<TSource, TDestination> : IObjectMapBuilder<TSource, TDestination>, IObjectMapper<TSource, TDestination> where TSource : class where TDestination : class, new()
{
    /// <summary>
    /// Lock object for thread safety.
    /// </summary>
    private readonly object _lock = new();

    /// <summary>
    /// The function used to map the source object to the destination object.
    /// </summary>
    private Func<TSource, IMapper, TDestination>? _mapperFunc;

    /// <summary>
    /// A list of properties that should be ignored when adding expressions to the expression tree.
    /// </summary>
    private readonly HashSet<string> _ignoreProperties = [];

    /// <summary>
    /// A dictionary of explicit custom mappings for the destination properties.
    /// </summary>
    private readonly Dictionary<string, LambdaExpression> _customMappings = [];

    /// <summary>
    /// A list of callbacks to be executed in order after the mapping is complete.
    /// </summary>
    private readonly List<Action<TSource, TDestination, IMapper>> _afterMapCallbacks = [];

    /// <summary>
    /// Creates a new instance of the <see cref="ObjectMapper{TSource, TDestination}"/> class.
    /// When using this constructor, the mapper function is built from expressions. If mapping
    /// expressions for a give member are not provided, the mapper will attempt to create
    /// an automatic mapping for the member.
    /// </summary>
    public ObjectMapper() { }

    /// <summary>
    /// Creates a new instance of the <see cref="ObjectMapper{TSource, TDestination}"/> class using the specified mapper function.
    /// When using this constructor to provide an explicit mapper function, the mapping function cannot be modified.
    /// </summary>
    /// <remarks>
    /// This constructor is intended to be used when the mapper function is already known and does not need to be built from expressions.
    /// </remarks>
    /// <param name="mapperFunc"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ObjectMapper(Func<TSource, IMapper, TDestination> mapperFunc)
    {
        _mapperFunc = mapperFunc
            ?? throw new ArgumentNullException(nameof(mapperFunc));
    }

    public IObjectMapBuilder<TSource, TDestination> AfterMap(Action<TSource, TDestination, IMapper> callback)
    {
        // Throw an exception if the mapper function already exists, because this method is only
        // intended to be used when the mapper function is not yet created.
        if (_mapperFunc != null)
        {
            throw new InvalidOperationException("The mapper function cannot be modified after it has been created.");
        }

        _afterMapCallbacks.Add(callback ?? throw new ArgumentNullException(nameof(callback)));

        return this;
    }

    public IObjectMapBuilder<TSource, TDestination> AndMap<TMember>(Expression<Func<TDestination, TMember>> destinationProperty, Expression<Func<TSource, IMapper, TMember>> sourceExpression)
    {
        // Throw an exception if the mapper function already exists, because this method is only
        // intended to be used when the mapper function is not yet created.
        if (_mapperFunc != null)
        {
            throw new InvalidOperationException("The mapper function cannot be modified after it has been created.");
        }

        // Throw an exception if the destination property or source expression is null.
        ArgumentNullException.ThrowIfNull(destinationProperty, nameof(destinationProperty));
        ArgumentNullException.ThrowIfNull(sourceExpression, nameof(sourceExpression));

        // Throw an exception if the destination property is not a member expression.
        if (destinationProperty.Body is not MemberExpression destinationMember)
        {
            throw new ArgumentException("The destination member must be a property.", nameof(destinationProperty));
        }

        // Throw an exception if the destination property is already being ignored.
        var name = destinationMember.Member.Name;
        if (_ignoreProperties.Contains(name))
        {
            throw new ArgumentException($"The destination property '{name}' is ignored.", nameof(destinationProperty));
        }

        _customMappings[name] = sourceExpression;
        return this;
    }

    public IObjectMapBuilder<TSource, TDestination> Ignore<TMember>(Expression<Func<TDestination, TMember>> destinationProperty)
    {
        // Throw an exception if the mapper function already exists, because this method is only
        // intended to be used when the mapper function is not yet created.
        if (_mapperFunc != null)
        {
            throw new InvalidOperationException("The mapper function cannot be modified after it has been created.");
        }

        // Throw an exception if the destination property is null.
        ArgumentNullException.ThrowIfNull(destinationProperty, nameof(destinationProperty));

        // Throw an exception if the destination property is not a member expression.
        if (destinationProperty.Body is not MemberExpression destinationMember)
        {
            throw new ArgumentException("The destination member must be a property.", nameof(destinationProperty));
        }

        // Throw an exception if the destination property has already been mapped.
        if (_customMappings.ContainsKey(destinationMember.Member.Name))
        {
            throw new ArgumentException($"The destination property '{destinationMember.Member.Name}' is already mapped.", nameof(destinationProperty));
        }

        _ignoreProperties.Add(destinationMember.Member.Name);
        return this;
    }

    public TDestination Map(TSource source, IMapper mapper)
    {
        if (_mapperFunc == null)
        {
            lock (_lock)
            {
                _mapperFunc ??= BuildMapperFunc();
            }
        }

        if (_mapperFunc == null)
            throw new InvalidOperationException($"The function to map between {typeof(TSource)} and {typeof(TDestination)} is not defined.");

        return _mapperFunc(source, mapper);
    }

    private Func<TSource, IMapper, TDestination>? BuildMapperFunc()
    {
        throw new NotImplementedException();
    }
}
