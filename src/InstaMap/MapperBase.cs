
namespace InstaMap;

public class MapperBase : IMapper
{
    private static readonly Type _interfaceType = typeof(IObjectMapper<,>);

    private readonly Dictionary<(Type, Type), IObjectMapper> _mappers = [];

    public TDestination Map<TSource, TDestination>(TSource source) where TDestination : class, new()
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        if (_mappers.TryGetValue((typeof(TSource), typeof(TDestination)), out var mapper))
        {
            if (mapper is IObjectMapper<TSource, TDestination> typedMapper)
            {
                return typedMapper.Map(source, this);
            }
        }

        throw new NotImplementedException($"Mapper not found for {typeof(TSource)} to {typeof(TDestination)}");
    }

    public TDestination Map<TDestination>(object source) where TDestination : class, new()
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        var sourceType = source.GetType();
        var destinationType = typeof(TDestination);

        if (_mappers.TryGetValue((sourceType, destinationType), out var mapper))
        {
            var mapperType = _interfaceType.MakeGenericType(sourceType, destinationType);
            var method = mapperType.GetMethod("Map");
            if (method != null)
            {
                if (method.Invoke(mapper, [source, this]) is not TDestination result)
                {
                    throw new InvalidOperationException($"Mapping failed for {source.GetType()} to {typeof(TDestination)}");
                }

                return result;
            }
        }

        throw new NotImplementedException($"Mapper not found for {source.GetType()} to {typeof(TDestination)}");
    }

    public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source) where TDestination : class, new()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TDestination> Map<TDestination>(IEnumerable<object> source) where TDestination : class, new()
    {
        throw new NotImplementedException();
    }
}
