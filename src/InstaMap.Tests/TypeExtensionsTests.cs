using System.Collections;

namespace InstaMap.Tests;

public class TypeExtensionsTests
{
    [Theory]
    [InlineData(typeof(int), typeof(double), true)]
    [InlineData(typeof(string), typeof(int), true)]
    [InlineData(typeof(object), typeof(int), false)]
    [InlineData(typeof(void), typeof(int), false)]
    public void IsConvertible_ShouldReturnExpectedResult(Type sourceType, Type destinationType, bool expected)
    {
        sourceType.IsConvertible(destinationType).ShouldBe(expected);
    }

    [Fact]
    public void IsDictionary_ShouldReturnTrueForTwoDictionaries()
    {
        typeof(Dictionary<string, int>).IsDictionary(typeof(Dictionary<Guid, bool>)).ShouldBeTrue();
    }

    [Fact]
    public void IsDictionary_ShouldReturnFalseIfOneIsNotDictionary()
    {
        typeof(Dictionary<string, int>).IsDictionary(typeof(List<int>)).ShouldBeFalse();
    }

    [Fact]
    public void IsDictionary_ShouldReturnFalseIfNotGeneric()
    {
        typeof(object).IsDictionary(typeof(Dictionary<int, string>)).ShouldBeFalse();
    }

    [Fact]
    public void IsEnumerable_ShouldReturnTrueAndSetElementTypes()
    {
        var sourceType = typeof(List<int>);
        var destinationType = typeof(List<string>);

        var result = sourceType.IsEnumerable(destinationType, out var sourceElementType, out var destinationElementType);

        result.ShouldBeTrue();
        sourceElementType.ShouldBe(typeof(int));
        destinationElementType.ShouldBe(typeof(string));
    }

    [Fact]
    public void IsEnumerable_ShouldReturnFalseForNonGenericTypes()
    {
        var result = typeof(ArrayList).IsEnumerable(typeof(ArrayList), out var _, out var _);
        result.ShouldBeFalse();
    }

    [Fact]
    public void IsEnumerable_ShouldReturnFalseIfNotAssignableFromIEnumerable()
    {
        var result = typeof(Tuple<int>).IsEnumerable(typeof(Tuple<string>), out var _, out var _);
        result.ShouldBeFalse();
    }

    [Theory]
    [InlineData(typeof(int), true)]
    [InlineData(typeof(byte), true)]
    [InlineData(typeof(long), true)]
    [InlineData(typeof(float), false)]
    [InlineData(typeof(decimal), false)]
    public void IsIntegerType_ShouldReturnExpectedResult(Type type, bool expected)
    {
        type.IsIntegerType().ShouldBe(expected);
    }

    [Theory]
    [InlineData(typeof(string), true)]
    [InlineData(typeof(char), false)]
    [InlineData(typeof(object), false)]
    public void IsStringType_ShouldReturnExpectedResult(Type type, bool expected)
    {
        type.IsStringType().ShouldBe(expected);
    }
}
